using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Game;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Cards.Rebellion.Units
{
    public class RebelCommando : RebelGalaxyUnit, IHasAbility
    {
        public RebelCommando(int id, SWDBGame game) :
            base(id, 3, 3, 0, 0, "Rebel Commando", new List<Trait>{ Trait.trooper }, false, game) {}

        public override bool AbilityActive() 
        {
            return base.AbilityActive() && (Owner?.Opponent?.Hand.Any() ?? false);
        }

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            if (Owner == null || Owner.Opponent == null) return;

            if (Owner?.IsForceWithPlayer() ?? false) 
            {
                Owner.Opponent.Hand[Random.Shared.Next(0, Owner.Opponent.Hand.Count - 1)].MoveToDiscard();
                if (Game.StaticEffects.Contains(StaticEffect.Yavin4Effect) && Owner.Opponent.CurrentBase != null) 
                {
                    Owner.Opponent.CurrentBase.AddDamage(2);
                }
            } else 
            {
                Game.PendingActions.Add(PendingAction.Of(Action.DiscardFromHand, true));
            }
        }

        public override int GetTargetValue() 
        {
            return 3;
        }

        public override void ApplyReward() 
        {
            Game.Empire.AddForce(2);
        }
    }
}