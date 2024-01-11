namespace Game.Cards.Common.Models.Interface
{
    public interface IPublicBase : IBaseCard
    {
        int HitPoints { get; }
        int CurrentDamage { get; }
        int GetRemainingHealth();
    }
}