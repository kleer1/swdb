using Game.Cards.Common.Models.Interface;
using SWDB.Game.Cards.Common.Models;

namespace GameTest.Cards.Interfaces
{
    public interface IHasCard
    {
        IPlayableCard Card { get; }
    }
}