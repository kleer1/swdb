using Game.Actions.Interfaces;

namespace Agents.Interfaces
{
    public interface IGameActionConverter
    {
        IGameAction ConvertToGameAction(string action);
    }
}
