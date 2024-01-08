using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;

namespace SWDB.Game.Cards.Neutral.Units
{
    public class Z95Headhunter : NeutralGalaxyUnit, IHasAbility
    {
        public Z95Headhunter(int id, SWDBGame game) :
            base(id, 1, 2, 0, 0, "Z-95 Headhunter", new List<Trait>{ Trait.fighter }, false, game) {}

        public override bool AbilityActive() 
        {
            return base.AbilityActive() && (Owner?.Opponent?.ShipsInPlay.Any() ?? false) &&
                ((Owner?.Deck.Any() ?? false) || (Owner?.Discard.Any() ?? false));
        }

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            Owner?.DrawCards(1);
        }
    }
}