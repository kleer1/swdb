using Agents.Interfaces;
using Game.Actions;
using Game.Actions.Interfaces;
using Newtonsoft.Json;

namespace Agents.DotnetAgents.BasicAgents
{
    internal class BasicGameActionConverter : IGameActionConverter
    {
        public IGameAction ConvertToGameACtion(string action)
        {
            return JsonConvert.DeserializeObject<GameAction>(action) ?? throw new Exception("Could not deserialize action");
        }
    }
}
