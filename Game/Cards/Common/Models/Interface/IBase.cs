namespace Game.Cards.Common.Models.Interface
{
    public interface IBase : ICard
    {
        int HitPoints { get; }
        int CurrentDamage { get; set; }
        int HetRemainingHealth();
        void AddDamage(int damage);
        void MakeCurrentBase();

    }
}