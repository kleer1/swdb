using Game.Actions;
using Game.Cards.Common.Models.Interface;
using Game.State.Interfaces;
using SWDB.Game.Common;

namespace Game.State
{
    public class GameState : IGameState
    {
        public required IPublicPlayer Empire { get; set; }
        public required IPublicPlayer Rebel { get; set; }
        public required bool IsGameOver { get; set; } = false;
        public required IBasePlayableCard? TopOfGalaxyDeck { get; set; }
        public required IReadOnlyCollection<IBasePlayableCard> GalaxyRow { get; set; }
        public required IReadOnlyCollection<IBasePlayableCard> GalaxyDiscard { get; set; }
        public required IReadOnlyCollection<IBasePlayableCard> ExiledCards { get; set; }
        public required IReadOnlyCollection<IBasePlayableCard> OuterRimPilots { get; set; }
        public required Faction CurrentPlayersAction { get; set; }
        public required Faction CurrentPlayersTurn {get; set; }
        public required IReadOnlyCollection<GameAction> ValidActions { get; set; }
    }
}