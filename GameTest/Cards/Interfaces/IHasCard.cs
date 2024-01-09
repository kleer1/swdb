using SWDB.Game.Cards.Common.Models;

namespace GameTest.Cards.Interfaces
{
    public interface IHasCard
    {
        PlayableCard Card { get; }
    }
}