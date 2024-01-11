using Game.Actions;
using Game.Cards.Common.Models.Interface;
using Game.State.Interfaces;
using SWDB.Game.Common;

namespace Game.State
{
    public class GameState
    {
        public GameState()
        {
        }

        public IPublicPlayer Empire { get; internal set; }
        public IPublicPlayer Rebel { get; internal set; }
        public bool IsGameOver { get; internal set; } = false;
        public IBasePlayableCard? TopOfGalaxyDeck { get; internal set; }
        public IReadOnlyCollection<IBasePlayableCard> GalaxyRow { get; internal set; }
        public IReadOnlyCollection<IBasePlayableCard> GalaxyDiscard { get; internal set; }
        public IReadOnlyCollection<IBasePlayableCard> ExiledCards { get; internal set; }
        public IReadOnlyCollection<IBasePlayableCard> OuterRimPilots { get; internal set; }
        public Faction CurrentPlayersAction { get; internal set; }
        public Faction CurrentPlayersTurn {get; internal set; }
        public IReadOnlyCollection<GameAction> ValidActions { get; internal set; }
    }
}