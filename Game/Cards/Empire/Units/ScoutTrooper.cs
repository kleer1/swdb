using Game.Cards.Common.Models.Interface;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Common;

namespace SWDB.Game.Cards.Empire.Units
{
    public class ScoutTrooper : EmpireGalaxyUnit, IHasAbility
    {
        public ScoutTrooper(int id, SWDBGame game) :
            base(id, 2, 0, 2, 0, "Scout Trooper", new List<Trait>{ Trait.trooper }, false, game){}

        public override bool AbilityActive() 
        {
            return base.AbilityActive() && Game.GalaxyDeck.Any();
        }

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            IPlayableCard card = Game.GalaxyDeck.BaseList[0];
            Game.RevealTopCardOfDeck();
            if (card.Faction == Faction.empire) 
            {
                Owner?.AddForce(1);
            } else if (card.Faction == Faction.rebellion) 
            {
                card.MoveToGalaxyDiscard();
                Game.ForgetTopCardOfDeck();
            }
        }

        public override int GetTargetValue() 
        {
            return 2;
        }

        public override void ApplyReward() 
        {
            AddExilePendingAction(Game.Rebel, 1);
        }
    }
}