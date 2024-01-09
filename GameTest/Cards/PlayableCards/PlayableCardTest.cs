using GameTest.Cards.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.PlayableCards
{
    [TestFixture]
    public abstract class PlayableCardTest : BaseTest, IBasePlayableCard
    {
        public virtual int Id => throw new NotImplementedException();
        public abstract Player GetPlayer();
        public abstract void AssertAfterPlay();

        public  PlayableCard Card { get; private set; }

        [SetUp]
        public void Setup()
        {
            Card = (PlayableCard) Game.CardMap[Id];
            // Set force to neutral
            Game.Empire.AddForce(3);
            Player player = GetPlayer();
            Faction faction = player.Faction;
            if (faction == Faction.rebellion) 
            {
                Game.ApplyAction(Action.PassTurn);
            }
            if (Card.Location == CardLocation.GalaxyDeck || Card.Location == CardLocation.OuterRimPilots) 
            {
                Card.BuyToHand(player);
            } else if (Card.Location == CardLocation.GalaxyRow) 
            {
                Card.BuyToHand(player);
                // keep 6 cards in row
                Game.GalaxyDeck.BaseList.First().MoveToGalaxyRow();
            } else if (Card.Location == CardLocationHelper.GetDeck(faction)) 
            {
                Card.MoveToHand();
            }
        }

        protected virtual void PrePlaySetup() {
            // can be overridden for extra setup options
        }

        [Test]
        public void TestPlay() 
        {
            PrePlaySetup();
            Player player = GetPlayer();
            Faction faction = player.Faction;
            Game.ApplyAction(Action.PlayCard, Id);
            if (Card is CapitalShip)
            {
                Assert.Multiple(() =>
                {
                    That(Card.Location, Is.EqualTo(CardLocationHelper.GetShipsInPlay(faction)));
                    That(Card.CardList, Is.EquivalentTo(player.ShipsInPlay));
                    That(player.ShipsInPlay, Does.Contain(Card));
                });
            }
            else if (Card is Unit) 
            {
                Assert.Multiple(() =>
                {
                    That(Card.Location, Is.EqualTo(CardLocationHelper.GetUnitsInPlay(faction)));
                    That(Card.CardList, Is.EquivalentTo(player.UnitsInPlay));
                    That(player.UnitsInPlay, Does.Contain(Card));
                });
            }

            AssertAfterPlay();
        }

        protected static void AssertGameState(Player player, int attack, int resource)
        {
            Assert.Multiple(() =>
            {
                That(player.Resources, Is.EqualTo(resource));
                That(player.GetAvailableAttack(), Is.EqualTo(attack));
            });
        }

        protected void AssertNoForceChange() 
        {
            That(Game.ForceBalance.Position, Is.EqualTo(3));
        }

        protected void AssertForceIncreasedBy(Faction faction, int amount) 
        {
            int expectedValue = faction == Faction.empire ? 3 - amount : 3 + amount;
            if (expectedValue < 0) 
            {
                expectedValue = 0;
            }
            if (expectedValue > 6) 
            {
                expectedValue = 6;
            }
            That(Game.ForceBalance.Position, Is.EqualTo(expectedValue));
        }
    }
}