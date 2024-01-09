using GameTest.Cards.Bases.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Empire.Ships;
using SWDB.Game.Cards.Rebellion.Ships;
using SWDB.Game.Common;

namespace GameTest.Cards.Empire.Bases
{
    [TestFixture]
    public class DeathStarTest : EmpireAvailableBaseTest, IHasOnRevealTest, IHasAbilityTest
    {
        public override int Id => 121;

        public void PreChooseBaseSetup() 
        {
            EmptyGalaxyRow();
        }

        public void AssertAfterChooseBase() 
        {
            Player player = GetPlayer();
            if (player == null || player.Opponent == null) return;
            That(Base.AbilityActive(), Is.EqualTo(false));
            GetPlayer().AddResources(4);
            That(Base.AbilityActive(), Is.EqualTo(false));
            PlayableCard monCal = MoveToInPlay(typeof(MonCalamariCruiser), player.Opponent).ElementAt(0);
            That(Base.AbilityActive(), Is.EqualTo(true));
            monCal.MoveToGalaxyRow();
            That(Base.AbilityActive(), Is.EqualTo(true));
            GetPlayer().AddResources(-1);
            That(Base.AbilityActive(), Is.EqualTo(false));
        }

        public void SetupAbility() 
        {
            Player player = GetPlayer();
            if (player == null || player.Opponent == null) return;
            EmptyGalaxyRow();
            MoveToInPlay(typeof(MonCalamariCruiser), player.Opponent);
            GetPlayer().AddResources(4);
        }

        public void VerifyAbility() 
        {
            Player player = GetPlayer();
            if (player == null || player.Opponent == null) return;
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.FireWhenReady));

            PlayableCard? monCal = GetPlayer().Opponent?.ShipsInPlay.BaseList.ElementAt(0);
            Game.ApplyAction(Action.FireWhenReady, monCal?.Id);

            That(Game.PendingActions, Has.Count.EqualTo(0));
            That(monCal?.Location, Is.EqualTo(CardLocationHelper.GetDiscard(player.Opponent.Faction)));
            That(GetPlayer().Resources, Is.EqualTo(0));
        }

        [Test]
        public void TestAttackEmpireCenterRow() 
        {
            EmptyGalaxyRow();
            MoveToGalaxyRow(typeof(StarDestroyer));
            GetPlayer().AddResources(5);
            ((IHasAbilityTest) this).UseCardAbility();

            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.FireWhenReady));
            PlayableCard ssd = Game.GalaxyRow.BaseList.ElementAt(0);
            Game.ApplyAction(Action.FireWhenReady, ssd.Id);

            That(Game.PendingActions, Has.Count.EqualTo(0));
            That(ssd.Location, Is.EqualTo(CardLocation.GalaxyDiscard));
            That(GetPlayer().Resources, Is.EqualTo(1));
        }
    }
}