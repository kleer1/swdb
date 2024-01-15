using Game.Actions.Interfaces;
using Newtonsoft.Json;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;
using Action = SWDB.Game.Actions.Action;

namespace Game.Actions
{
    public class GameAction : IGameAction
    {
        public Action Action { get; private set; }
        public int? CardId { get; private set; } = null;
        public Stats? Stats { get; private set; } = null;
        public ResourceOrRepair? ResourceOrRepair { get; private set; } = null;

        [JsonConstructor]
        public GameAction(Action action, int? cardId, Stats? stats, ResourceOrRepair? resourceOrRepair)
        {
            Action = action;
            CardId = cardId;
            Stats = stats;
            ResourceOrRepair = resourceOrRepair;            
        }

        public GameAction(Action action, int cardId)
        {
            Action = action;
            CardId = cardId;          
        }

        public GameAction(Action action, Stats stats)
        {
            Action = action;
            Stats = stats;          
        }

        public GameAction(Action action, ResourceOrRepair resourceOrRepair)
        {
            Action = action;
            ResourceOrRepair = resourceOrRepair;          
        }

        public GameAction(Action action)
        {
            Action = action;        
        }

        public override string ToString()
        {
            return "{ Action: " + Action + ", CardID: " + CardId + ", Stats: " + Stats + ", ResourceOrRepair: " + ResourceOrRepair + " }";
        }

        public override bool Equals(object? obj)
        {
            return obj is GameAction action &&
                   Action == action.Action &&
                   CardId == action.CardId &&
                   Stats == action.Stats &&
                   ResourceOrRepair == action.ResourceOrRepair;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Action, CardId, Stats, ResourceOrRepair);
        }
    }
}