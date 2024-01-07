using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Common;
using SWDB.Game;

namespace SWDB.Cards.Rebellion.Units
{
    public abstract class RebelGalaxyUnit : Unit, ITargetable
    {
        protected RebelGalaxyUnit(int id, int cost, int attack, int resources, int force, String title, List<Trait> traits, 
                bool isUnique, SWDBGame game) :
            base(id, cost, attack, resources, force, Faction.rebellion, title, traits, isUnique, null,
                CardLocation.GalaxyDeck, (IList<Card>) game.GalaxyDeck, game) {}
        
        public abstract int GetTargetValue();
        public abstract void ApplyReward();
    }
}