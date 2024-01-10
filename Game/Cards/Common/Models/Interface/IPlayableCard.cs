using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace Game.Cards.Common.Models.Interface
{
    public interface IPlayableCard : ICard
    {
        int Cost { get; }
        int Attack { get; }
        int Resources { get; }
        int Force { get; }
        IList<Trait> Traits { get; }
        bool AbleToAttack();
        void SetAttacked();
        void Buy(Player newOwner);
        void BuyToTopOfDeck(Player newOwner);
        void BuyToHand(Player newOwner);
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