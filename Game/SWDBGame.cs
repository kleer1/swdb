using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;
using SWDB.Game.Common.Utils;
using SWDB.Game.Actions;

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
        private Faction CurrentPlayersAction { get; set; } = Faction.empire;
        private Faction CurrentPlayersTurn {get; set; } = Faction.empire;
        internal IList<StaticEffect> StaticEffects { get; } = new List<StaticEffect>();
        internal IList<PendingAction> PendingActions { get; } = new List<PendingAction>();
        private Card? LastCardPlayed { get; set; }
        private Card? LastCardActivated { get; set; }
        internal IDictionary<Faction, int> KnowsTopCardOfDeck { get; } = new Dictionary<Faction, int>{ {Faction.empire, 0}, {Faction.rebellion, 0} };
        private IList<PlayableCard> Attackers { get; } = new List<PlayableCard>();
        internal Card? AttackTarget { get; private set; }
        public bool CanSeeOpponentsHand {get; private set; }
        private List<PlayableCard> ExileAtEndOfTurn { get; } = new List<PlayableCard>();
        private PlayableCard? ANewHope1 { get; set; } = null;
        private ISet<int> AvailableActions { get; } = new HashSet<int>();

        public SWDBGame()
        {
            Empire.Opponent = Rebel;
            Empire.Game = this;
            Rebel.Opponent = Empire;
            Rebel.Game = this;
        }

        public Player GetCurrentPlayer() => CurrentPlayersTurn == Faction.empire ? Empire : Rebel;

        public void RevealOpponentsHand() => CanSeeOpponentsHand = true;

        private void PassCurrentAction() => CurrentPlayersAction = CurrentPlayersAction == Faction.empire ? Faction.rebellion : Faction.empire;

        public void DrawGalaxyCard() 
        {
            if (!GalaxyDeck.Any()) 
            {
                GalaxyDeck = GalaxyDiscard;
                GalaxyDiscard = new List<PlayableCard>();
                GalaxyDeck = GalaxyDeck.OrderBy(x => Random.Shared.Next()).ToList();
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

        public void AssignDamageToBase(int damageDealt, Player player) 
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
    }
}