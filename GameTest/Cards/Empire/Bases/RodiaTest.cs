using Game.Cards.Common.Models.Interface;
using GameTest.Cards.Bases.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Empire.Bases
{
    [TestFixture]
    public class RodiaTest : EmpireAvailableBaseTest, IHasOnRevealTest
    {
        private IPlayableCard? rebel1;
        private IPlayableCard? rebel2;
        private IPlayableCard? empire1;
        private IPlayableCard? empire2;
        private IPlayableCard? neutral1;
        private IPlayableCard? neutral2;

        public override int Id => 126;

        public void PreChooseBaseSetup() 
        {
            rebel1 = (IPlayableCard) Game.CardMap[BaseTest.REBEL_GALAXY_CARD];
            rebel2 = (IPlayableCard) Game.CardMap[BaseTest.REBEL_GALAXY_CARD + 1];
            empire1 = (IPlayableCard) Game.CardMap[BaseTest.EMPIRE_GALAXY_CARD];
            empire2 = (IPlayableCard) Game.CardMap[BaseTest.EMPIRE_GALAXY_CARD + 1];
            neutral1 = (IPlayableCard) Game.CardMap[BaseTest.NEUTRAL_GALAXY_CARD];
            neutral2 = (IPlayableCard) Game.CardMap[BaseTest.NEUTRAL_GALAXY_CARD + 1];

            for (int i = Game.GalaxyRow.BaseList.Count - 1; i >= 0; i--)
            {
                IPlayableCard card = Game.GalaxyRow.BaseList[i];
                Game.GalaxyRow.RemoveAt(i);
                card.MoveToTopOfGalaxyDeck();
            }

            That(Game.GalaxyRow, Has.Count.EqualTo(0));

            rebel1.MoveToGalaxyRow();
            rebel2.MoveToGalaxyRow();
            empire1.MoveToGalaxyRow();
            empire2.MoveToGalaxyRow();
            neutral1.MoveToGalaxyRow();
            neutral2.MoveToGalaxyRow();
            That(GetPlayer().Opponent?.CurrentBase?.CurrentDamage, Is.EqualTo(0));
            That(Game.GalaxyRow, Has.Count.EqualTo(6));
        }

        public void AssertAfterChooseBase() 
        {
            That(GetPlayer().Opponent?.CurrentBase?.CurrentDamage, Is.EqualTo(2));
            That(rebel1?.Location, Is.EqualTo(CardLocation.GalaxyDiscard));
            That(rebel2?.Location, Is.EqualTo(CardLocation.GalaxyDiscard));
            That(empire1?.Location, Is.EqualTo(CardLocation.GalaxyRow));
            That(empire2?.Location, Is.EqualTo(CardLocation.GalaxyRow));
            That(neutral1?.Location, Is.EqualTo(CardLocation.GalaxyRow));
            That(neutral2?.Location, Is.EqualTo(CardLocation.GalaxyRow));
            That(Game.GalaxyRow, Has.Count.EqualTo(6));
        }
        
    }
}