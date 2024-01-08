using SWDB.Game.Common;

namespace SWDB.Game.Cards.Common.Models.Interface
{
    public interface IHasChooseResourceOrRepair : IHasAbility
    {
        void ApplyChoice(ResourceOrRepair choice);
    }
}