using SWDB.Game.Common;

namespace Game.Cards.Common.Models.Interface
{
    public interface IBaseCard
    {
        int Id { get; }
        Faction Faction { get; }
        string Title { get; }
        bool IsUnique { get; }
         CardLocation Location { get; set; }
         bool AbilityActive();
    }
}