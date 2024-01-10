using SWDB.Game;
using SWDB.Game.Common;

namespace Game.Cards.Common.Models.Interface
{
    public interface ICard
    {
        int Id { get; }
        Faction Faction { get; }
        string Title { get; }
        bool IsUnique { get; }
        Player? Owner { get; set; }
        SWDBGame Game { get; }
        CardLocation Location { get; set; }
        IList<ICard>? CardList { get; set; }
        bool AbilityActive();
        void ApplyAbility();
    }
}