using Game.Cards.Common.Models.Interface;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Interfaces
{
    public interface IHasMoveToInPlay
    {
        IList<IPlayableCard> MoveToInPlay(Type type, Player player, int amount);
    }
}