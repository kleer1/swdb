using Game.Cards.Common.Models.Interface;
using Game.State.Interfaces;

namespace Game.State
{
    public class PublicPlayer : IPublicPlayer
    {
        public PublicPlayer()
        {
        }

        public IReadOnlyCollection<IBasePlayableCard> Hand { get; set; }

        public IReadOnlyCollection<IBasePlayableCard> Deck { get; set; }

        public IReadOnlyCollection<IBasePlayableCard> Discard { get; set; }

        public IReadOnlyCollection<IPublicBase> AvailableBases { get; set; }

        public IReadOnlyCollection<IPublicBase> DestroyedBases { get; set; }

        public IReadOnlyCollection<IBaseUnit> UnitsInPlay { get; set; }

        public IReadOnlyCollection<IBaseCapitalShip> ShipsInPlay { get; set; }

        public IPublicBase? CurrentBase { get; set; }

        
    }
}