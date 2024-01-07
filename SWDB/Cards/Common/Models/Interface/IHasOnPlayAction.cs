using SWDB.Game.Actions;

namespace SWDB.Cards.Common.Models.Interface
{
    public interface IHasOnPlayAction
    {
        IList<PendingAction> GetActions();
    }
}