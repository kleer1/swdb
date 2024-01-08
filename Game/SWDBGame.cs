using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;
using SWDB.Game.Utils;
using SWDB.Game.Actions;
using SWDB.Cards.Utils;
using Action = SWDB.Game.Actions.Action;
using Game.Utils;
using static SWDB.Game.Actions.Action;
using SWDB.Game.Cards.Common.Models.Interface;

namespace SWDB.Game
{
    public class SWDBGame
    {
        public Player Empire { get; } = new Player(Faction.empire);
        public Player Rebel { get; } = new Player(Faction.rebellion);
        public ForceBalance ForceBalance { get; } = new ForceBalance();
        public IList<PlayableCard> GalaxyDeck { get; private set; } = new List<PlayableCard>();
        public IList<PlayableCard> GalaxyRow { get; private set; } = new List<PlayableCard>();
        public IList<PlayableCard> GalaxyDiscard { get; private set; } = new List<PlayableCard>();
        public IList<PlayableCard> OuterRimPilots { get; } = new List<PlayableCard>();
        public IList<PlayableCard> ExiledCards { get; } = new List<PlayableCard>();
        public IDictionary<int, Card> CardMap { get; } = new Dictionary<int, Card>();
        internal Faction CurrentPlayersAction { get; private set; } = Faction.empire;
        private Faction CurrentPlayersTurn {get; set; } = Faction.empire;
        internal IList<StaticEffect> StaticEffects { get; } = new List<StaticEffect>();
        internal IList<PendingAction> PendingActions { get; } = new List<PendingAction>();
        private Card? LastCardPlayed { get; set; }
        internal Card? LastCardActivated { get; private set; }
        internal IDictionary<Faction, int> KnowsTopCardOfDeck { get; } = new Dictionary<Faction, int>{ {Faction.empire, 0}, {Faction.rebellion, 0} };
        internal IList<PlayableCard> Attackers { get; } = new List<PlayableCard>();
        internal Card? AttackTarget { get; private set; }
        public bool CanSeeOpponentsHand {get; private set; }
        private List<PlayableCard> ExileAtEndOfTurn { get; } = new List<PlayableCard>();
        internal PlayableCard? ANewHope1Card { get; private set; } = null;
        private ISet<int> AvailableActions { get; } = new HashSet<int>();
        private bool GameComplete { get; set; } = false;

        public SWDBGame()
        {
            Empire.Opponent = Rebel;
            Empire.Game = this;
            Rebel.Opponent = Empire;
            Rebel.Game = this;
            CardMap = SetupUtils.Setup(this);
            DrawGalaxyCard(6);
            Empire.DrawCards(5);
            Rebel.DrawCards(5);
        }

        public Player GetCurrentPlayer() => CurrentPlayersTurn == Faction.empire ? Empire : Rebel;

        public void RevealOpponentsHand() => CanSeeOpponentsHand = true;

        private void PassCurrentAction() => CurrentPlayersAction = CurrentPlayersAction == Faction.empire ? Faction.rebellion : Faction.empire;

        public void DrawGalaxyCard(int num = 1) 
        {
            for (int i = 0; i < num; i++)
            {
                if (!GalaxyDeck.Any()) 
                {
                    GalaxyDeck = GalaxyDiscard;
                    GalaxyDiscard = new List<PlayableCard>();
                    GalaxyDeck.Shuffle();
                    foreach (PlayableCard c in GalaxyDeck) 
                    {
                        c.Location = CardLocation.GalaxyDeck;
                        c.CardList = (IList<Card>?) GalaxyDeck;
                    };
                }
                PlayableCard card = GalaxyDeck.Pop();
                GalaxyRow.Add(card);
                card.Location = CardLocation.GalaxyRow;
                card.CardList = (IList<Card>?) GalaxyRow;
                foreach (KeyValuePair<Faction, int> entry in KnowsTopCardOfDeck) 
                {
                    if (entry.Value > 0) 
                    {
                        KnowsTopCardOfDeck[entry.Key] = entry.Value - 1;
                    }
                }
            }  
        }

        public void LookAtTopCardOfDeck(Faction faction) 
        {
            if (KnowsTopCardOfDeck[faction] < 1) {
                KnowsTopCardOfDeck[faction] = 1;
            }
        }

        public void RevealTopCardOfDeck() 
        {
            foreach (KeyValuePair<Faction, int> entry in KnowsTopCardOfDeck) {
                if (entry.Value < 1) {
                    KnowsTopCardOfDeck[entry.Key] = 1;
                }
            }
        }

        public void ForgetTopCardOfDeck() 
        {
            foreach (KeyValuePair<Faction, int> entry in KnowsTopCardOfDeck) {
                if (entry.Value > 0) {
                    KnowsTopCardOfDeck[entry.Key] = entry.Value - 1;
                }
            }
        }

        public static void AssignDamageToBase(int damageDealt, Player player) 
        {
            int remainingDamage = damageDealt;
            if (player.ShipsInPlay.Any()) 
            {
                player.ShipsInPlay = player.ShipsInPlay.OrderBy(s => s.GetRemainingHealth()).Reverse().ToList();
                for (int i = player.ShipsInPlay.Count - 1; i >= 0; i--)
                {
                    CapitalShip ship = player.ShipsInPlay[i];
                    if (ship.GetRemainingHealth() <= remainingDamage) 
                    {
                        remainingDamage -= ship.GetRemainingHealth();
                        player.ShipsInPlay.RemoveAt(i);
                        ship.MoveToDiscard();
                    }
                }
            }

            if (remainingDamage > 0)
            {
                if (player.ShipsInPlay.Any()) 
                {
                    player.ShipsInPlay.Last().AddDamage(remainingDamage);
                } else 
                {
                    player.CurrentBase?.AddDamage(remainingDamage);
                }
            }
            
        }

        public void ApplyAction(Action action, int? cardId = null, Stats? stats = null, ResourceOrRepair? resourceOrRepair = null)
        {
            if (GameComplete) return;

            Card? card = null;
            if (cardId != null && !CardMap.TryGetValue(cardId.Value, out card))
            {
                return;
            }

            if (!ValidActionUtil.IsValidAction(action, card, this, stats, resourceOrRepair))
            {
                return;
            }

            PlayableCard? playableCard = null;
            Base? _base = null;
            Player currentPlayer = GetCurrentPlayer();
            if (card is PlayableCard card1) 
            {
                playableCard = card1;
            } else if (card is Base @base) 
            {
                _base = @base;
            }
            bool isPendingAction = PendingActions.Any();
            bool endedTurn = false;

            switch(action)
            {
                case PlayCard:
                    break;
                case PurchaseCard:
                    break;
                case UseCardAbility:
                    break;
                case AttackCenterRow:
                case AttackBase:
                    break;
                case SelectAttacker:
                    break;
                case DiscardFromHand:
                case DurosDiscard:
                case BWingDiscard:
                    break;
                case DiscardCardFromCenter:
                    break;
                case ExileCard:
                case JabbaExile:
                    break;
                case ReturnCardToHand:
                    break;
                case ChooseNextBase:
                    break;
                case SwapTopCardOfDeck:
                    break;
                case FireWhenReady:
                    break;
                case GalacticRule:
                    break;
                case ANewHope1:
                    break;
                case ANewHope2:
                    break;
                case JynErsoTopDeck:
                    break;
                case LukeDestroyShip:
                    break;
                case HammerHeadAway:
                    break;
                case PassTurn:
                    break;
                case DeclineAction:
                    // To Nothing
                    break;
                case ChooseStatBoost:
                    break;
                case ChooseResourceOrRepair:
                    break;
                case AttackNeutralCard:
                    break;
                case ConfirmAttackers:
                    break;
                
            }

            PendingAction? pendingAction = null;
            if (isPendingAction)
            {
                pendingAction = PendingActions.Pop();
                if (action != Action.DeclineAction)
                {
                    pendingAction.ExecuteCallback();
                }
            }

            if (endedTurn) 
            {
                EndTurn(currentPlayer);
                StartTurn(currentPlayer?.Opponent);
            }

            if (pendingAction != null && pendingAction.ShouldPassAction) 
            {
                PassCurrentAction();
            }

            if (PendingActions.Any() && PendingActions.First().ShouldPassAction) 
            {
                PassCurrentAction();
            }
        }

        private void EndTurn(Player player) 
        {
            foreach (PlayableCard card in ExileAtEndOfTurn) 
            {
                if (card.Location != CardLocation.Exile) 
                {
                    card.MoveToExile();
                }
            }
            ExileAtEndOfTurn.Clear();
            player.DiscardUnits();
            player.DiscardHand();
            player.DrawCards(5);
            player.Resources = 0;
            StaticEffects.Clear();
            CurrentPlayersTurn = CurrentPlayersTurn == Faction.empire ? Faction.rebellion : Faction.empire;
            CurrentPlayersAction = CurrentPlayersTurn;
            PendingActions.Clear();
            CanSeeOpponentsHand = false;
            LastCardActivated = null;
        }

        private void StartTurn(Player? player) 
        {
            if (player == null)
            {
                throw new ArgumentException("Can not start a new turn. Player has no opponent");
            }

            // The only thing that need to happen at the start of turn is queueing up any start of turn pending actions.
            if (player.CurrentBase == null) 
            {
                // Start with choosing a new base
                PendingActions.Add(PendingAction.Of(Action.ChooseNextBase));
            } else 
            {
                // Check for start of turn base abilities
                if (player.CurrentBase is IHasAtStartOfTurn startOfTurn) 
                {
                    startOfTurn.ApplyAtStartOfTurn();
                }
            }

            // Add capital ship resources
            foreach (CapitalShip ship in player.ShipsInPlay)
            {
                player.AddResources(ship.Resources);
                if (ship is IHasAtStartOfTurn startOfTurn) 
                {
                    startOfTurn.ApplyAtStartOfTurn();
                }
            }

            if (player.DoesPlayerHaveFullForce()) 
            {
                player.AddResources(1);
            }
        }
    }
}