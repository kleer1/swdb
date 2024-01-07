using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Game;
using SWDB.Common;

namespace SWDB.Cards.Empire.Units
{
    public class ScoutTrooper : EmpireGalaxyUnit, IHasAbility
    {
        public ScoutTrooper(int id, SWDBGame game) :
            base(id, 2, 0, 2, 0, "Scout Trooper", new List<Trait>{ Trait.trooper }, false, game){}

        public override bool AbilityActive() {
            return base.AbilityActive() && Game.GalaxyDeck.Any();
        }

        public override void ApplyAbility() {
            base.ApplyAbility();
            PlayableCard card = Game.GalaxyDeck[0];
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

        public override int GetTargetValue() {
            return 2;
        }

        public override void ApplyReward() {
            AddExilePendingAction(Game.Rebel, 1);
        }
    }
}