namespace Game.Cards.Common.Models.Interface
{
    public interface ICapitalShip : IPlayableCard
    {
        int HitPoints { get; }
        int CurrentDamage { get; set; }
        void AddDamage(int damage);
        int GetRemainingHealth();

    }
}