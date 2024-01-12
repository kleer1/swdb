using SWDB.Game.Cards.Common.Models;

namespace Game.Cards.Common.Models.Interface
{
    public interface IBasePlayableCard : IBaseCard
    {
        int Cost { get; }
        int Attack { get; }
        int Resources { get; }
        int Force { get; }
        IReadOnlyCollection<Trait> Traits { get; }
        bool AbleToAttack();
    }
}