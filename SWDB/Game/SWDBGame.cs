using SWDB.Cards.Common.Models;
using SWDB.Common;
using SWDB.Game.Actions;

namespace SWDB.Game
{
    public class SWDBGame
    {
        public Player Empire { get; } = new Player(Faction.empire);
        public Player Rebel { get; } = new Player(Faction.rebellion);
        public ForceBalance ForceBalance { get; } = new ForceBalance();
        public IList<PlayableCard> GalaxyDeck { get; } = new List<PlayableCard>();
        public IList<PlayableCard> GalaxyRow { get; private set; } = new List<PlayableCard>();
        public IList<PlayableCard> GalaxyDiscard { get; } = new List<PlayableCard>();
        public IList<PlayableCard> OuterRimPilots { get; } = new List<PlayableCard>();
        public IList<PlayableCard> ExiledCards { get; } = new List<PlayableCard>();
        public IDictionary<int, Card> CardMap { get; } = new Dictionary<int, Card>();
        private Faction CurrentPlayersAction { get; set; } = Faction.empire;
        private Faction CurrentPlayersTurn {get; set; } = Faction.empire;
        internal IList<StaticEffect> StaticEffects { get; } = new List<StaticEffect>();
        internal IList<PendingAction> PendingActions { get; } = new List<PendingAction>();
        private Card? LastCardPlayed { get; set; }
        private Card? LastCardActivated { get; set; }
        private IDictionary<Faction, int> KnowsTopCardOfDeck { get; } = new Dictionary<Faction, int>();
        private IList<PlayableCard> Attackers { get; } = new List<PlayableCard>();
        private Card? AttackTarget { get; set; }
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


    }
}