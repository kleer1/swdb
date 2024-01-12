using Game.Cards.Common.Models.Interface;
using Game.State.Interfaces;

namespace Game.State
{
    public class PublicPlayer : IPublicPlayer
    {

        public required IReadOnlyCollection<IBasePlayableCard> Hand { get; set; }

        public required IReadOnlyCollection<IBasePlayableCard> Deck { get; set; }

        public required IReadOnlyCollection<IBasePlayableCard> Discard { get; set; }

        public required IReadOnlyCollection<IPublicBase> AvailableBases { get; set; }

        public required IReadOnlyCollection<IPublicBase> DestroyedBases { get; set; }

        public required IReadOnlyCollection<IBaseUnit> UnitsInPlay { get; set; }

        public required IReadOnlyCollection<IBaseCapitalShip> ShipsInPlay { get; set; }

        public required IPublicBase? CurrentBase { get; set; }

        
    }
}