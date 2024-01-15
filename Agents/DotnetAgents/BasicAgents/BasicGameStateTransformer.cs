using Agents.Interfaces;
using Game.Actions.Interfaces;
using Game.State.Interfaces;
using Newtonsoft.Json;

namespace Agents.DotnetAgents.BasicAgents
{
    public class BasicGameStateTransformer : IGameStateTranformer
    {
        public string TransformGameState(IGameState gameState)
        {
            return JsonConvert.SerializeObject(BuildActionMap(gameState.ValidActions));
        }

        public static Dictionary<Action, List<IGameAction>> BuildActionMap(IReadOnlyCollection<IGameAction> actions)
        {
            return actions.GroupBy(a => a.Action).ToDictionary(a => a.Key, a => a.ToList());
        }
    }
}
