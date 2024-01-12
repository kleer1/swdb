using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;
using Action = SWDB.Game.Actions.Action;

namespace Game.Actions.Interfaces
{
    public interface IGameAction
    {
        Action Action { get; }
        int? CardId { get; }
        Stats? Stats { get; }
        ResourceOrRepair? ResourceOrRepair { get; }
    }
}
