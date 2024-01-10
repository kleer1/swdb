using GameTest.Cards.Bases.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Rebellion.Units;
using SWDB.Game.Common;

namespace GameTest.Cards.Rebellion.Bases
{
    [TestFixture]
    public class TatooineTest : RebelAvailableBaseTest, IHasOnRevealTest
    {
        private PlayableCard? rebel;

        public override int Id => 138;

        public void PreChooseBaseSetup() 
        {
            rebel = MoveToGalaxyRow(typeof(LukeSkywalker)).ElementAt(0);
            rebel.MoveToGalaxyDiscard();
        }

        public void AssertAfterChooseBase()
        {
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.ANewHope1));

            Game.ApplyAction(Action.ANewHope1, rebel?.Id);

            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.ANewHope2));

            PlayableCard rowCard = Game.GalaxyRow.BaseList.ElementAt(0);
            Game.ApplyAction(Action.ANewHope2, rowCard.Id);

            That(rebel?.Location, Is.EqualTo(CardLocation.GalaxyRow));
            That(rowCard.Location, Is.EqualTo(CardLocation.GalaxyDiscard));
            That(GetPlayer().Resources, Is.EqualTo(1));
        }
    }
}