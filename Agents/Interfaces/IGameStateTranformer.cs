using Game.State.Interfaces;

namespace Agents.Interfaces
{
    public interface IGameStateTranformer
    {
        string TransformGameState(IGameState gameState);
    }
}
