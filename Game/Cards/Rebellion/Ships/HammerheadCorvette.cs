using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Game.Cards.Rebellion.Ships
{
    public class HammerheadCorvette : RebelGalaxyShip, IHasAbility
    {
        public HammerheadCorvette(int id, SWDBGame game) :
            base(id, 4, 0, 2, 0, "Hammerhead Corvette", game, 4) {}

        public override bool AbilityActive() 
        {
            if (!base.AbilityActive()) return false;

            if (Owner == null || Owner.Opponent == null)
            {
                throw new ArgumentException("HammerheadCorvette cannot check if ability is active. It has no owner or opponent");
            }
            
            return Owner.Opponent.ShipsInPlay.Any() ||
                Game.GalaxyRow.Where(pc => pc is CapitalShip && pc.Faction == Owner.Opponent.Faction).Any();
        }

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            MoveToExile();
            Game.PendingActions.Add(PendingAction.Of(Action.HammerHeadAway));
        }
    }
}