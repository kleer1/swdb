using GameTest.Cards.Bases.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Bases
{
    public abstract class AvailableBaseCardTest : BaseTest, IAvailableBaseCard
    {
        public virtual int Id => throw new NotImplementedException();
        public abstract Player GetPlayer();
        public Base Base { get; set; }


        [SetUp]
        public void SetUp()
        {
            Player player = GetPlayer();
            Faction faction = player.Faction;
            Base = (Base) Game.CardMap[Id];
            Game.Empire.AddForce(3);

            // Start as the opposite faction so passing turn prompts a base choice
            if (faction == Faction.empire)
            {
                Game.ApplyAction(SWDB.Game.Actions.Action.PassTurn);
            }

            player.CurrentBase = null;
        }

        [Test]
        public void TestSetup()
        {
            Assert.Multiple(() =>
            {
                That(Game.PendingActions, Is.Empty);
                That(Game.StaticEffects, Is.Empty);
                That(GetPlayer().Resources, Is.EqualTo(0));
                That(GetPlayer()?.Opponent?.Resources, Is.EqualTo(0));
                That(GetPlayer().CurrentBase, Is.Null);
                That(Game.ForceBalance.Position, Is.EqualTo(3));
            });
            
        }
    }
}