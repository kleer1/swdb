using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;
using SWDB.Game.Utils;
using SWDB.Game.Actions;
using SWDB.Cards.Utils;
using Action = SWDB.Game.Actions.Action;
using Game.Utils;
using SWDB.Game.Cards.Common.Models.Interface;
using static SWDB.Game.Utils.ListExtension;
using Game.Cards.Common.Models.Interface;
using Game.State;
using Game.Common.Interfaces;
using Game.State.Interfaces;
using Game.Actions;
using Game.Actions.Interfaces;

namespace SWDB.Game
{
    public class SWDBGame
    {
        public IPlayer Empire { get; } = new Player(Faction.empire);
        public IPlayer Rebel { get; } = new Player(Faction.rebellion);
        public virtual ForceBalance ForceBalance { get; } = new ForceBalance();
        public CastedList<ICard, IPlayableCard> GalaxyDeck { get; private set; } = new CastedList<ICard, IPlayableCard>();
        public CastedList<ICard, IPlayableCard> GalaxyRow { get; private set; } = new CastedList<ICard, IPlayableCard>();
        public CastedList<ICard, IPlayableCard> GalaxyDiscard { get; private set; } = new CastedList<ICard, IPlayableCard>();
        public CastedList<ICard, IPlayableCard> OuterRimPilots { get; } = new CastedList<ICard, IPlayableCard>();
        public CastedList<ICard, IPlayableCard> ExiledCards { get; } = new CastedList<ICard, IPlayableCard>();
        public IDictionary<int, ICard> CardMap { get; } = new Dictionary<int, ICard>();
        public Faction CurrentPlayersAction { get; private set; } = Faction.empire;
        public Faction CurrentPlayersTurn {get; private set; } = Faction.empire;
        public IList<StaticEffect> StaticEffects { get; } = new List<StaticEffect>();
        public IList<PendingAction> PendingActions { get; } = new List<PendingAction>();
        public ICard? LastCardPlayed { get; private set; }
        public ICard? LastCardActivated { get; private set; }
        public IDictionary<Faction, int> KnowsTopCardOfDeck { get; } = new Dictionary<Faction, int>{ {Faction.empire, 0}, {Faction.rebellion, 0} };
        public IList<IPlayableCard> Attackers { get; } = new List<IPlayableCard>();
        public ICard? AttackTarget { get; internal set; }
        public bool CanSeeOpponentsHand {get; private set; }
        public List<IPlayableCard> ExileAtEndOfTurn { get; } = new List<IPlayableCard>();
        public IPlayableCard? ANewHope1Card { get; private set; } = null;
        public bool IsGameOver { get; private set; } = false;
        public IGameState GameState { get; private set; }

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
            GameState = BuildCurrentGameState();
        }

        public IPlayer GetCurrentPlayer() => CurrentPlayersTurn == Faction.empire ? Empire : Rebel;

        public void RevealOpponentsHand() => CanSeeOpponentsHand = true;

        private void PassCurrentAction() => CurrentPlayersAction = CurrentPlayersAction == Faction.empire ? Faction.rebellion : Faction.empire;

        public void DrawGalaxyCard(int num = 1) 
        {
            for (int i = 0; i < num; i++)
            {
                if (!GalaxyDeck.Any()) 
                {
                    GalaxyDeck = new CastedList<ICard, IPlayableCard>(GalaxyDiscard.BaseList);
                    GalaxyDiscard = new CastedList<ICard, IPlayableCard>(new List<IPlayableCard>());
                    GalaxyDeck.Shuffle();
                    foreach (PlayableCard c in GalaxyDeck) 
                    {
                        c.Location = CardLocation.GalaxyDeck;
                        c.CardList = GalaxyDeck;
                    };
                }
                ICard card = GalaxyDeck.Pop();
                GalaxyRow.Add(card);
                card.Location = CardLocation.GalaxyRow;
                card.CardList = GalaxyRow;
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

        public void AssignDamageToBase(int damageDealt, IPlayer player) 
        {
            int remainingDamage = damageDealt;
            if (player.ShipsInPlay.Any()) 
            {
                IEnumerable<ICapitalShip> temp = player.ShipsInPlay.BaseList.OrderBy(s => s.GetRemainingHealth()).Reverse();
                for (int i = temp.Count() - 1; i >= 0; i--)
                {
                    ICapitalShip ship = temp.ElementAt(i);
                    if (ship.GetRemainingHealth() <= remainingDamage) 
                    {
                        remainingDamage -= ship.GetRemainingHealth();
                        player.ShipsInPlay.BaseList.Remove(ship);
                        ship.MoveToDiscard();
                    }
                }
            }

            if (remainingDamage > 0)
            {
                if (player.ShipsInPlay.Any()) 
                {
                    player.ShipsInPlay.BaseList.Last().AddDamage(remainingDamage);
                } else 
                {
                    player.CurrentBase?.AddDamage(remainingDamage);
                }
            }
            
        }

        private void UpdateGameComplete() 
        {
            IsGameOver = Empire.DestroyedBases.Count >= 4 || Rebel.DestroyedBases.Count >= 4;
        }

        public IGameState ApplyAction(Action action, int? cardId = null, Stats? stats = null, ResourceOrRepair? resourceOrRepair = null)
        {
            return ApplyAction(new GameAction(action, cardId, stats, resourceOrRepair));
        }

        public IGameState ApplyAction(IGameAction gameAction)
        {
            if (IsGameOver) return GameState;

            ICard? card = null;
            if (gameAction.CardId != null && !CardMap.TryGetValue(gameAction.CardId.Value, out card))
            {
                return GameState;
            }

            if (!ValidActionUtil.IsValidAction(this, gameAction, card))
            {
                return GameState;
            }

            PlayableCard? playableCard = null;
            IBase? _base = null;
            IPlayer currentPlayer = GetCurrentPlayer();
            if (card is PlayableCard card1) 
            {
                playableCard = card1;
            } else if (card is Base @base) 
            {
                _base = @base;
            }
            bool isPendingAction = PendingActions.Any();
            bool endedTurn = false;

            switch(gameAction.Action)
            {
                case Action.PlayCard:
                    PlayCard(playableCard, currentPlayer);
                    break;
                case Action.PurchaseCard:
                    PurchaseCard(playableCard, currentPlayer);
                    break;
                case Action.UseCardAbility:
                    UseCardAbility(card);
                    break;
                case Action.AttackCenterRow:
                case Action.AttackBase:
                    AttackTarget = card;
                    PendingActions.Add(PendingAction.Of(Action.SelectAttacker));
                    break;
                case Action.SelectAttacker:
                    SelectAttacker(playableCard);
                    break;
                case Action.DiscardFromHand:
                case Action.DurosDiscard:
                case Action.BWingDiscard:
                    DiscardAction(playableCard);
                    break;
                case Action.DiscardCardFromCenter:
                    DiscardCardFromCenter(playableCard, currentPlayer);
                    break;
                case Action.ExileCard:
                case Action.JabbaExile:
                    playableCard?.MoveToExile();
                    break;
                case Action.ReturnCardToHand:
                    playableCard?.MoveToHand();
                    break;
                case Action.ChooseNextBase:
                    _base?.MakeCurrentBase();
                    if (_base is IHasOnReveal hasOnReveal) 
                    {
                        hasOnReveal.ApplyOnReveal();
                    }
                    break;
                case Action.SwapTopCardOfDeck:
                    GalaxyDeck.BaseList.First().MoveToGalaxyRow();
                    playableCard?.MoveToTopOfGalaxyDeck();
                    RevealTopCardOfDeck();
                    break;
                case Action.FireWhenReady:
                    FireWhenReady(playableCard, currentPlayer);
                    break;
                case Action.GalacticRule:
                    playableCard?.MoveToGalaxyDiscard();
                    KnowsTopCardOfDeck[Faction.empire] = 1;
                    break;
                case Action.ANewHope1:
                    ANewHope1Card = playableCard;
                    PendingActions.Add(PendingAction.Of(Action.ANewHope2));
                    break;
                case Action.ANewHope2:
                    playableCard?.MoveToGalaxyDiscard();
                    ANewHope1Card?.MoveToGalaxyRow();
                    ANewHope1Card = null;
                    Rebel.AddResources(1);
                    break;
                case Action.JynErsoTopDeck:
                    playableCard?.MoveToTopOfDeck();
                    RevealTopCardOfDeck();
                    break;
                case Action.LukeDestroyShip:
                    playableCard?.MoveToDiscard();
                    break;
                case Action.HammerHeadAway:
                    if (playableCard?.Location == CardLocation.GalaxyRow) 
                    {
                        playableCard.MoveToGalaxyDiscard();
                        DrawGalaxyCard();
                    } else {
                        playableCard?.MoveToDiscard();
                    }
                    break;
                case Action.PassTurn:
                    endedTurn = true;
                    break;
                case Action.DeclineAction:
                    // To Nothing
                    break;
                case Action.ChooseStatBoost:
                    if (LastCardPlayed is IHasChooseStats hasChooseStats && gameAction.Stats != null) 
                    {
                        hasChooseStats.ApplyChoice(gameAction.Stats.Value);
                    }
                    break;
                case Action.ChooseResourceOrRepair:
                    if (LastCardActivated is IHasChooseResourceOrRepair hasChooseResourceOrRepair && gameAction.ResourceOrRepair != null) 
                    {
                        hasChooseResourceOrRepair.ApplyChoice(gameAction.ResourceOrRepair.Value);
                    }
                    break;
                case Action.AttackNeutralCard:
                    PendingActions.Add(PendingAction.Of(Action.AttackCenterRow));
                    break;
                case Action.ConfirmAttackers:
                    ConfirmAttackers(currentPlayer);
                    break;
            }

            PendingAction? pendingAction = null;
            if (isPendingAction)
            {
                pendingAction = PendingActions.Pop();
                if (gameAction.Action != Action.DeclineAction)
                {
                    pendingAction.ExecuteCallback();
                }
            }

            UpdateGameComplete();
            if (IsGameOver)
            {
                GameState = BuildCurrentGameState();
                return GameState;
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

            GameState = BuildCurrentGameState();
            return GameState;
        }

        private void EndTurn(IPlayer player) 
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

        private void StartTurn(IPlayer? player) 
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

        private void PlayCard(PlayableCard? card, IPlayer currentPlayer)
        {
            if (card == null)
            {
                throw new ArgumentException("Can not play card. Playable card is null");
            }

            if (StaticEffects.Contains(StaticEffect.DrawOnFirstNeutralCard)) 
            {
                if (card.Faction == Faction.neutral) 
                {
                    currentPlayer.DrawCards(1);
                    StaticEffects.RemoveAll(StaticEffect.DrawOnFirstNeutralCard);
                }
            }

            card.MoveToInPlay();
            if (card is IHasOnPlayAction onPlay) 
            {
                PendingActions.AddRange(onPlay.GetActions());
            }
            
            LastCardPlayed = card;
        }

        private void PurchaseCard(PlayableCard? card, IPlayer currentPlayer)
        {
            if (card == null)
            {
                throw new ArgumentException("Can not purchase card. Playable card is null");
            }

            if (StaticEffects.Contains(StaticEffect.BuyNextToHand)) 
            {
                card.BuyToHand(currentPlayer);
            } 
            else if (StaticEffects.Contains(StaticEffect.BuyNextNeutralToHand) && card.Faction == Faction.neutral) 
            {
                card.BuyToHand(currentPlayer);
            } 
            else if (StaticEffects.Contains(StaticEffect.BuyNextToTopOfDeck)) 
            {
                card.BuyToTopOfDeck(currentPlayer);
            } 
            else 
            {
                card.Buy(currentPlayer);
            }

            StaticEffects.RemoveAll(new List<StaticEffect>{ StaticEffect.BuyNextToTopOfDeck, StaticEffect.BuyNextToHand });
            if (card.Faction == Faction.neutral) 
            {
                StaticEffects.RemoveAll(StaticEffect.BuyNextNeutralToHand);
            }

            if (!StaticEffects.Contains(StaticEffect.NextFactionPurchaseIsFree) && !StaticEffects.Contains(StaticEffect.NextFactionOrNeutralPurchaseIsFree)) 
            {
                currentPlayer.AddResources(-card.Cost);
            } 
            else 
            {
                StaticEffects.RemoveAll(new List<StaticEffect> { StaticEffect.NextFactionPurchaseIsFree, StaticEffect.NextFactionOrNeutralPurchaseIsFree });
            }

            if (StaticEffects.Contains(StaticEffect.ExileNextFactionPurchase) && card.Faction == CurrentPlayersAction) 
            {
                StaticEffects.RemoveAll(StaticEffect.ExileNextFactionPurchase);
                ExileAtEndOfTurn.Add(card);
            } 
            else if (StaticEffects.Contains(StaticEffect.ExileNextNeutralPurchase) && card.Faction == Faction.neutral) 
            {
                StaticEffects.RemoveAll(StaticEffect.ExileNextNeutralPurchase);
                ExileAtEndOfTurn.Add(card);
            }

            if (StaticEffects.Contains(StaticEffect.PurchaseFromDiscard)) 
            {
                StaticEffects.RemoveAll(StaticEffect.PurchaseFromDiscard);
            }

            if (card is IHasPurchaseAction purchaseAction) 
            {
                purchaseAction.ApplyPurchaseAction();
            }
            
            if (card.Location == CardLocation.GalaxyRow) 
            {
                DrawGalaxyCard();
            }
        }

        private void UseCardAbility(ICard? card)
        {
            if (card == null || card is not IHasAbility) 
            {
                throw new ArgumentException("Can not use card ablity. Card is null or has no ability");

            }
            ((IHasAbility) card).ApplyAbility();
            LastCardActivated = card;
        }

        private void SelectAttacker(PlayableCard? card)
        {
            if (card == null) 
            {
                throw new ArgumentException("Can not set as attacker. Card is null");

            }
            card.SetAttacked();
            Attackers.Add(card);
            PendingActions.Add(PendingAction.Of(Action.SelectAttacker));
        }

        private void DiscardAction(PlayableCard? card)
        {
            if (card == null) 
            {
                throw new ArgumentException("Cannot discard card. Card is null");

            }
            card.MoveToDiscard();
            if (StaticEffects.Contains(StaticEffect.Yavin4Effect) && CurrentPlayersAction == Faction.empire) 
            {
                Empire.CurrentBase?.AddDamage(2);
            }
        }

        private void DiscardCardFromCenter(PlayableCard? card, IPlayer currentPlayer)
        {
            if (card == null) 
            {
                throw new ArgumentException("Cannot discard card from center. Card is null");

            }
            card.MoveToGalaxyDiscard();
            DrawGalaxyCard();
        }

        private void FireWhenReady(PlayableCard? card, IPlayer currentPlayer)
        {
            if (card == null) 
            {
                throw new ArgumentException("Cannot fire hwne ready. Card is null");

            }
            currentPlayer.AddResources(-4);
            if (card.Location == CardLocation.GalaxyRow) 
            {
                card.MoveToGalaxyDiscard();
                DrawGalaxyCard();
            } else {
                card.MoveToDiscard();
            }
        }

        private void ConfirmAttackers(IPlayer currentPlayer)
        {
            if (AttackTarget is PlayableCard playableCardAttackTarget) 
            {
                    // Attack center row
                    if (playableCardAttackTarget is ITargetable targetable) 
                    {
                        targetable.ApplyReward();
                    } 
                    else 
                    {
                        currentPlayer.AddResources(playableCardAttackTarget.Cost);
                        StaticEffects.Remove(StaticEffect.CanBountyOneNeutral);
                    }

                    foreach (PlayableCard attacker in Attackers)
                    {
                        if (attacker is IBountyHunter bountyHunter) 
                        {
                            bountyHunter.ReceiveBounty();
                        }
                    }

                    playableCardAttackTarget.MoveToGalaxyDiscard();
                    DrawGalaxyCard();
                } 
                else if (AttackTarget is Base) 
                {
                    if (currentPlayer.Opponent == null)
                    {
                        throw new ArgumentException("Can not attack base. Opponent is null");
                    }
                    // Attack base
                    int totalAttack = Attackers.Sum(a => a.Attack);
                    AssignDamageToBase(totalAttack, currentPlayer.Opponent);
                }
                AttackTarget = null;
                Attackers.Clear();
        }

        private GameState BuildCurrentGameState()
        {
            return new GameState {
                Empire = BuildPublicPlayer(Empire),
                Rebel = BuildPublicPlayer(Rebel),
                IsGameOver = IsGameOver,
                TopOfGalaxyDeck = KnowsTopCardOfDeck[CurrentPlayersAction] > 0 ? (IBasePlayableCard) GalaxyDeck.First() : null,
                GalaxyRow = CastList<IBasePlayableCard, IPlayableCard>(GalaxyRow.BaseList),
                GalaxyDiscard = CastList<IBasePlayableCard, IPlayableCard>(GalaxyDiscard.BaseList),
                ExiledCards = CastList<IBasePlayableCard, IPlayableCard>(ExiledCards.BaseList),
                OuterRimPilots = CastList<IBasePlayableCard, IPlayableCard>(OuterRimPilots.BaseList),
                CurrentPlayersAction = CurrentPlayersAction,
                CurrentPlayersTurn = CurrentPlayersTurn,
                ValidActions = (IReadOnlyCollection<GameAction>)ValidActionUtil.GetValidGameActions(this)
            };
        }

        private IPublicPlayer BuildPublicPlayer(IPlayer player)
        {
            PublicPlayer publicPlayer = new PublicPlayer
            {
                Hand = CurrentPlayersAction == player.Faction || CanSeeOpponentsHand ?
                    CastList<IBasePlayableCard, IPlayableCard>(player.Hand.BaseList) : new List<IBasePlayableCard>(),
                Deck = CurrentPlayersAction == player.Faction ?
                    CastList<IBasePlayableCard, IPlayableCard>(player.Deck.BaseList) : new List<IBasePlayableCard>(),
                Discard = CastList<IBasePlayableCard, IPlayableCard>(player.Discard.BaseList),
                AvailableBases = CastList<IPublicBase, IBase>(player.AvailableBases.BaseList),
                DestroyedBases = CastList<IPublicBase, IBase>(player.DestroyedBases.BaseList),
                UnitsInPlay = CastList<IBaseUnit, IUnit>(player.UnitsInPlay.BaseList),
                ShipsInPlay = CastList<IBaseCapitalShip, ICapitalShip>(player.ShipsInPlay.BaseList),
                CurrentBase = player.CurrentBase
            };
            return publicPlayer;
        }

        private IReadOnlyCollection<T> CastList<T, U>(IList<U> list) where U : T
        {
            IList<T> newList = new List<T>();
            foreach (U u in list)
            {
                newList.Add((T)u);
            }
            return (IReadOnlyCollection<T>)newList;
        }
    }
}