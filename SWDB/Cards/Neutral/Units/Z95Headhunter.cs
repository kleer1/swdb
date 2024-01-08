using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Game;

namespace SWDB.Cards.Neutral.Units
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