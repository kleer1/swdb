using SWDB.Game.Actions;

namespace SWDB.Game.Cards.Common.Models.Interface
{
    public interface IHasOnPlayAction
    {
        IList<PendingAction> GetActions();
    }
}