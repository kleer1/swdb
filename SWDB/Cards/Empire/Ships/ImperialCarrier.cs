using SWDB.Cards.Common.Models.Interface;
using SWDB.Game;

namespace SWDB.Cards.Empire.Ships
{
    public class ImperialCarrier : EmpireGalaxyShip, IHasAtStartOfTurn
    {
        public ImperialCarrier(int id, SWDBGame game) :
            base(id, 5, 0, 3, 0, "Imperial Carrier", game, 5) {} 
        
        public void ApplyAtStartOfTurn() 
        {
            Game.StaticEffects.Add(StaticEffect.CarrierEffect);
        }

        public override void MoveToInPlay() 
        {
            base.MoveToInPlay();
            Game.StaticEffects.Add(StaticEffect.CarrierEffect);
        }

        public override void MoveToDiscard() 
        {
            base.MoveToDiscard();
            Game.StaticEffects.Remove(StaticEffect.CarrierEffect);
        }
    }
}