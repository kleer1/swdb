using GameTest.Cards.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Bases
{
    public abstract class StartingBaseTest : BaseTest, IHasId, IHasPlayer
    {
        public virtual int Id => throw new NotImplementedException();

        public abstract Player GetPlayer();

        [Test]
        public void TestStartingBase()
        {
            Base _base = (Base) Game.CardMap[Id];
            Multiple(() =>
            {
                That(_base.Location, Is.EqualTo(CardLocationHelper.GetCurrentBase(GetPlayer().Faction)));
                That(GetPlayer().CurrentBase, Is.EqualTo(_base));
                That(_base.HitPoints, Is.EqualTo(8));
            });
        }
    }
}