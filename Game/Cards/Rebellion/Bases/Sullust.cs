using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Common;

namespace SWDB.Game.Cards.Rebellion.Bases
{
    public class Sullust : Base, IHasOnReveal, IHasAtStartOfTurn
    {
        public Sullust(int id, SWDBGame game) :
            base(id, Faction.rebellion, "Sullust", CardLocation.RebelAvailableBases, (IList<Card>) game.Rebel.AvailableBases,
                game, game.Rebel, 16) {}

        public void ApplyAtStartOfTurn() 
        {
            Game.StaticEffects.Add(StaticEffect.BuyNextToTopOfDeck);
        }

        public void ApplyOnReveal() 
        {
            Game.StaticEffects.Add(StaticEffect.BuyNextToTopOfDeck);
        }
    }
}