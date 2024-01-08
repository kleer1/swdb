using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Common;

namespace SWDB.Game.Cards.Rebellion.Bases
{
    public class Bespin : Base, IHasOnReveal, IHasAtStartOfTurn
    {
        public Bespin(int id, SWDBGame game) :
            base(id, Faction.rebellion, "Bespin", CardLocation.RebelAvailableBases, (IList<Card>) game.Rebel.AvailableBases,
                    game, game.Rebel, 14) {}

        public void ApplyAtStartOfTurn() 
        {
            Game.StaticEffects.Add(StaticEffect.DrawOnFirstNeutralCard);
        }

        public void ApplyOnReveal() 
        {
            Game.StaticEffects.Add(StaticEffect.DrawOnFirstNeutralCard);
        }
    }
}