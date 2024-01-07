namespace SWDB.Cards.Common.Models.Interface
{
    public interface IHasAbility
    {
        bool AbilityActive();
        void ApplyAbility();
    }
}