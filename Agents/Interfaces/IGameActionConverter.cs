using Game.Actions.Interfaces;

namespace Agents.Interfaces
{
    public interface IGameActionConverter
    {
        IGameAction ConvertToGameACtion(string action);
    }
}
