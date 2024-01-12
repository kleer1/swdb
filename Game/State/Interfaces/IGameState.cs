using Game.Actions;
using Game.Cards.Common.Models.Interface;
using SWDB.Game.Common;

namespace Game.State.Interfaces
{
    public interface IGameState
    {
        IPublicPlayer Empire { get; }
        IPublicPlayer Rebel { get; }
        bool IsGameOver { get; }
        IBasePlayableCard? TopOfGalaxyDeck { get; }
        IReadOnlyCollection<IBasePlayableCard> GalaxyRow { get; }
        IReadOnlyCollection<IBasePlayableCard> GalaxyDiscard { get; }
        IReadOnlyCollection<IBasePlayableCard> ExiledCards { get; }
        IReadOnlyCollection<IBasePlayableCard> OuterRimPilots { get; }
        Faction CurrentPlayersAction { get; }
        Faction CurrentPlayersTurn { get; }
        IReadOnlyCollection<GameAction> ValidActions { get; }
    }
}
