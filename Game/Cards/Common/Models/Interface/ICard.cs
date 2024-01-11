using Game.Common.Interfaces;
using SWDB.Game;

namespace Game.Cards.Common.Models.Interface
{
    public interface ICard : IBaseCard
    {
        IPlayer? Owner { get; set; }
        SWDBGame Game { get; }
        IList<ICard>? CardList { get; set; }
        void ApplyAbility();
    }
}