using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Common;
using SWDB.Game;

namespace SWDB.Cards.Rebellion.Bases
{
    public class Jedha : Base, IHasOnReveal, IHasAtStartOfTurn
    {
        public Jedha(int id, SWDBGame game) :
            base(id, Faction.rebellion, "Jedha", CardLocation.RebelAvailableBases, (List<Card>) game.Rebel.AvailableBases,
                game, game.Rebel, 14) {}

        public void ApplyAtStartOfTurn() 
        {
            Game.StaticEffects.Add(StaticEffect.ExileNextNeutralPurchase);
            Game.StaticEffects.Add(StaticEffect.BuyNextNeutralToHand);
        }

        public void ApplyOnReveal() 
        {
            Game.StaticEffects.Add(StaticEffect.ExileNextNeutralPurchase);
            Game.StaticEffects.Add(StaticEffect.BuyNextNeutralToHand);
        }
    }
}