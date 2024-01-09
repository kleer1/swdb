namespace GameTest.Cards.Bases.Interfaces
{
    public interface IHasAtStartOfTurnTest : IAvailableBaseCard
    {
        [Test]
        void TestAtStartOfTurn() 
        {
            TriggerStartOfTurn();
            AssertAfterStartOfTurn();
        }

        void TriggerStartOfTurn()
        {
            Base.MakeCurrentBase();
            Assert.Multiple(() =>
            {
                That(Game.PendingActions, Has.Count.EqualTo(0));
                That(Game.StaticEffects, Has.Count.EqualTo(0));
            });
            Game.ApplyAction(SWDB.Game.Actions.Action.PassTurn);
        }

        void AssertAfterStartOfTurn();
    }
}