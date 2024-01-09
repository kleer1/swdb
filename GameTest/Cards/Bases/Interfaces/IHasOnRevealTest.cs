using SWDB.Game.Common;

namespace GameTest.Cards.Bases.Interfaces
{
    public interface IHasOnRevealTest : IAvailableBaseCard
    {
        void PreChooseBaseSetup() 
        {
            // can be overridden for extra setup options
        }

        [Test]
        void TestChooseBase() 
        {
            PreChooseBaseSetup();
            ChooseBase();
            AssertAfterChooseBase();
        }

        void AssertAfterChooseBase();

        void ChooseBase()
        {
            Game.ApplyAction(SWDB.Game.Actions.Action.PassTurn);
            Assert.Multiple(() =>
            {
                That(Game.PendingActions, Has.Count.EqualTo(1));
                That(Game.PendingActions.First().Action, Is.EqualTo(SWDB.Game.Actions.Action.ChooseNextBase));
            });
            Game.ApplyAction(SWDB.Game.Actions.Action.ChooseNextBase, Id);
            Assert.Multiple(() =>
            {
                That(GetPlayer().CurrentBase, Is.EqualTo(Base));
                That(Base.Location, Is.EqualTo(CardLocationHelper.GetCurrentBase(GetPlayer().Faction)));
            });
        }
    }
}