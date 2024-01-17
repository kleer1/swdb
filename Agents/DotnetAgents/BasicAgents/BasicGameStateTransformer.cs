using Agents.Interfaces;
using Game.Actions.Interfaces;
using Game.State.Interfaces;

namespace Agents.DotnetAgents.BasicAgents
{
    public class BasicGameStateTransformer : IGameStateTranformer
    {
        public object TransformGameState(IGameState gameState)
        {
            return BuildActionMap(gameState.ValidActions);
        }

        public static Dictionary<Action, List<IGameAction>> BuildActionMap(IReadOnlyCollection<IGameAction> actions)
        {
            return actions.GroupBy(a => a.Action).ToDictionary(a => a.Key, a => a.ToList());
        }
    }
}
