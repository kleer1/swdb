using Game.Cards.Common.Models.Interface;
using Game.Common.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Interfaces
{
    public interface IHasMoveToInPlay
    {
        IList<IPlayableCard> MoveToInPlay(Type type, IPlayer player, int amount);
    }
}