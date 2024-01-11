using Game.Common.Interfaces;
using SWDB.Game.Common;

namespace GameTest.Cards.Interfaces
{
    public interface IHasPlayer
    {
        IPlayer GetPlayer();
    }
}