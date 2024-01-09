using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Common;

namespace SWDB.Game.Cards.Rebellion.Bases
{
    public class Jedha : Base, IHasOnReveal, IHasAtStartOfTurn
    {
        public Jedha(int id, SWDBGame game) :
            base(id, Faction.rebellion, "Jedha", CardLocation.RebelAvailableBases, game.Rebel.AvailableBases,
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