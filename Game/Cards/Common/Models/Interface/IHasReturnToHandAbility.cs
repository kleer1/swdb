namespace SWDB.Game.Cards.Common.Models.Interface
{
    public interface IHasReturnToHandAbility : IHasAbility
    {
        bool IsValidTarget(PlayableCard card);
    }
}