using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Common;

namespace SWDB.Game.Cards.Empire.Units
{
    public class TieInterceptor : EmpireGalaxyUnit, IHasAbility
    {
        public TieInterceptor(int id, SWDBGame game) :
            base(id, 3, 3, 0, 0, "Tie Interceptor", new List<Trait>{ Trait.fighter }, false, game) {}

        public override bool AbilityActive() 
        {
            return base.AbilityActive() && Game.GalaxyDeck.Any();
        }

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            PlayableCard card = Game.GalaxyDeck.BaseList[0];
            Game.RevealTopCardOfDeck();
            if (card.Faction == Faction.empire) 
            {
                Owner?.DrawCards(1);
            } else if (card.Faction == Faction.rebellion) 
            {
                card.MoveToGalaxyDiscard();
                Game.ForgetTopCardOfDeck();
            }
        }

        public override int GetTargetValue() 
        {
            return 3;
        }

        public override void ApplyReward() 
        {
            Game.Rebel.AddResources(3);
        }
    }
}