using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Game;

namespace SWDB.Cards.Neutral.Units
{
    public class TwilekSmuggler : NeutralGalaxyUnit, IHasAbility
    {
        public TwilekSmuggler(int id, SWDBGame game) :
            base(id, 3, 0, 3, 0, "Twi'lek Smuggler", new List<Trait>{ Trait.scoundrel }, false, game) {}

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            Game.StaticEffects.Add(StaticEffect.BuyNextToTopOfDeck);
        }
    }
}