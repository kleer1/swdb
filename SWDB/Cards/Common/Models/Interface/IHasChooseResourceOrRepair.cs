using SWDB.Common;

namespace SWDB.Cards.Common.Models.Interface
{
    public interface IHasChooseResourceOrRepair : IHasAbility
    {
        void ApplyChoice(ResourceOrRepair choice);
    }
}