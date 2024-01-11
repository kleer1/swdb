using Game.Cards.Common.Models.Interface;

namespace Game.State.Interfaces
{
    public interface IPublicPlayer
    {
        IReadOnlyCollection<IBasePlayableCard> Hand { get; }
        IReadOnlyCollection<IBasePlayableCard> Deck { get; }
        IReadOnlyCollection<IBasePlayableCard> Discard { get; }
        IReadOnlyCollection<IPublicBase> AvailableBases { get; }
        IReadOnlyCollection<IPublicBase> DestroyedBases { get; }
        IReadOnlyCollection<IBaseUnit> UnitsInPlay { get; }
        IReadOnlyCollection<IBaseCapitalShip> ShipsInPlay { get; }
        IPublicBase? CurrentBase { get; }
    }
}