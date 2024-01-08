using System.Collections.Generic;
using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Common;
using SWDB.Game;

namespace SWDB.Cards.Neutral.Units
{
    public class OuterRimPilot : Unit, IHasAbility
    {
        public OuterRimPilot(int id, SWDBGame game) :
            base(id, 2, 0, 2, 0, Faction.neutral, "Outer Rim Pilot", Array.Empty<Trait>(), false,
                null, CardLocation.OuterRimPilots, (IList<Card>) game.OuterRimPilots, game) {}

        public override bool AbilityActive() 
        {
            return base.AbilityActive() && (!Owner?.DoesPlayerHaveFullForce() ?? false);
        }

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            Owner?.AddForce(1);
            MoveToExile();
        }
    }
}