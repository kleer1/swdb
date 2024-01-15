using Game.State.Interfaces;

namespace Bots.Interfaces
{
    public interface IGameStateTranformer
    {
        string TransformGameState(IGameState gameState);
    }
}
