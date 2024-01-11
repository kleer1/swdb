namespace Game.Cards.Common.Models.Interface
{
    public interface IBaseCapitalShip : IBasePlayableCard
    {
        int HitPoints { get; }
        int CurrentDamage { get; }
        int GetRemainingHealth();
    }
}