using GameTest.Cards.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Empire.Ships;
using SWDB.Game.Cards.Rebellion.Units;
using SWDB.Game.Common;

namespace GameTest.Cards.IPlayableCards.Interfaces
{
    public interface ITargetableCardTest : IBaseIPlayableCard
    {
        [Test]
        void TestAttackedInCenterRow() 
        {
            // move card to galaxy row so it can be targeted
            Card.MoveToGalaxyRow();

            // Pass turn so opponent can attack
            Game.ApplyAction(SWDB.Game.Actions.Action.PassTurn);
            Faction faction = Game.CurrentPlayersTurn;

            if (faction == Faction.rebellion) 
            {
                // for rebels, two b-wings will kill anything in center
                MoveToInPlay(typeof(BWing), Game.Rebel, 2);
            } else if (faction == Faction.empire) 
            {
                // for empire, two ssds will do the same
                MoveToInPlay(typeof(StarDestroyer), Game.Empire, 2);
            }

            // choose card to attack
            Game.ApplyAction(SWDB.Game.Actions.Action.AttackCenterRow, Card.Id);
            // chose the attackers
            if (faction == Faction.rebellion) 
            {
                foreach (Card attacker in Game.Rebel.UnitsInPlay) 
                {
                    Game.ApplyAction(SWDB.Game.Actions.Action.SelectAttacker, attacker.Id);
                }
            } else
            {
                foreach (Card attacker in Game.Empire.ShipsInPlay) 
                {
                    Game.ApplyAction(SWDB.Game.Actions.Action.SelectAttacker, attacker.Id);
                }
            }
            PreAttackSetup();
            // chose to confirm attackers
            Game.ApplyAction(SWDB.Game.Actions.Action.ConfirmAttackers);
            AssertReward();
        }

        void AssertReward();

        void PreAttackSetup() 
        {

        }
    }
}