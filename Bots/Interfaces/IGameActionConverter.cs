using Game.Actions.Interfaces;

namespace Bots.Interfaces
{
    public interface IGameActionConverter
    {
        IGameAction ConvertToGameACtion(string action);
    }
}
