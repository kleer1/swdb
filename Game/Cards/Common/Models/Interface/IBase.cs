namespace Game.Cards.Common.Models.Interface
{
    public interface IBase : ICard , IPublicBase
    {
        void AddDamage(int damage);
        void MakeCurrentBase();

    }
}