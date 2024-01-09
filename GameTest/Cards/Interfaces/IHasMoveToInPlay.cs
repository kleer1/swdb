using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Interfaces
{
    public interface IHasMoveToInPlay
    {
        IList<PlayableCard> MoveToInPlay(Type type, Player player, int amount);
    }
}