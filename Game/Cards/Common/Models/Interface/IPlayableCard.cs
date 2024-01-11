using Game.Common.Interfaces;
using SWDB.Game.Common;

namespace Game.Cards.Common.Models.Interface
{
    public interface IPlayableCard : IBasePlayableCard, ICard
    {
        void SetAttacked();
        void Buy(IPlayer newOwner);
        void BuyToTopOfDeck(IPlayer newOwner);
        void BuyToHand(IPlayer newOwner);
        void MoveToDiscard();
        void MoveToTopOfDeck();
        void MoveToHand();
        void MoveToExile();
        void MoveToInPlay();
        void MoveToGalaxyDiscard();
        void MoveToGalaxyRow();
        void MoveToTopOfGalaxyDeck();

    }
}