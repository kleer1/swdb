namespace Game.Cards.Common.Models.Interface
{
    public interface ICapitalShip : IPlayableCard , IBaseCapitalShip
    {
        new int CurrentDamage { get; set; }
        void AddDamage(int damage);

    }
}