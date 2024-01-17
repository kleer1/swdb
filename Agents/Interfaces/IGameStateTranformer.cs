using Game.State.Interfaces;

namespace Agents.Interfaces
{
    public interface IGameStateTranformer
    {
        object TransformGameState(IGameState gameState);
    }
}
