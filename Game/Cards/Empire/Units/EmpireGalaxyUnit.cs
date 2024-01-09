using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Common;

namespace SWDB.Game.Cards.Empire.Units
{
    public abstract class EmpireGalaxyUnit : Unit, ITargetable
    {
        protected EmpireGalaxyUnit(int id, int cost, int attack, int resources, int force, string title, List<Trait> traits, 
                bool isUnique, SWDBGame game) : 
            base(id, cost, attack, resources, force, Faction.empire, title, traits, isUnique, null, 
                CardLocation.GalaxyDeck, game.GalaxyDeck, game) {}

        public abstract void ApplyReward();
        public abstract int GetTargetValue();

        public override int Attack
        {
            get
            {
                int atk = base.Attack;
                if (Traits.Contains(Trait.fighter)) 
                {
                    atk += Game.StaticEffects.Where(se => se == StaticEffect.CarrierEffect).Count();
                }
                return atk;
            }
        }
    }
}