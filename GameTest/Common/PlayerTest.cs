using Moq;
using SWDB.Game;
using SWDB.Game.Common;
using SWDB.Game.Cards.Common.Models;
using static SWDB.Game.Utils.ListExtension;
using Game.Cards.Common.Models.Interface;

namespace GameTest.Common
{
    [TestFixture]
    public class PlayerTest
    {
        private Mock<SWDBGame> game;

        private Mock<ForceBalance> forceBalance;

        [SetUp]
        public void SetUp()
        {
            game = new Mock<SWDBGame>(MockBehavior.Loose);
            forceBalance = new Mock<ForceBalance>(MockBehavior.Loose);
        }

        [Test]
        public void IsForceWithPlayer()
        {
            var player = BuildImperialPlayer();
            game.Setup(g => g.ForceBalance).Returns(forceBalance.Object);
            forceBalance.SetupSequence(fb => fb.DarkSideHasTheForce()).Returns(true).Returns(false);
            forceBalance.SetupSequence(fb => fb.LightSideHasTheForce()).Returns(false).Returns(true);

            True(player.IsForceWithPlayer());
            False(player.IsForceWithPlayer());

            forceBalance.Verify(fb => fb.DarkSideHasTheForce(), Times.Exactly(2));
           forceBalance.Verify(fb => fb.LightSideHasTheForce(), Times.Never);

            player = BuildRebelPlayer();

            False(player.IsForceWithPlayer());
            True(player.IsForceWithPlayer());
            forceBalance.Verify(fb => fb.DarkSideHasTheForce(), Times.Exactly(2));
            forceBalance.Verify(fb => fb.LightSideHasTheForce(), Times.Exactly(2));
        }

        [Test]
        public void AddResources()
        {
            var player = BuildImperialPlayer();
            That(player.Resources, Is.EqualTo(0));

            player.AddResources(5);
            That(player.Resources, Is.EqualTo(5));

            player.AddResources(-6);
            That(player.Resources, Is.EqualTo(0));
        }

        [Test]
        public void DrawCards()
        {
            var player = BuildPlayer(Faction.empire, 1, 0, 0, 0, 0);
            That(player.Deck, Has.Count.EqualTo(1));

            player.DrawCards(1);
            AssertAllSizes(player, 0, 0, 1, 0, 0);
            var card = player.Hand.BaseList[0];
            Mock.Get(card).Verify(c => c.MoveToHand(), Times.Once);

            // Test deck shuffle
            player = BuildPlayer(Faction.empire, 0, 1, 0, 0, 0);
            player.DrawCards(1);
            AssertAllSizes(player, 0, 0, 1, 0, 0);
            card = player.Hand.BaseList[0];
            Mock.Get(card).Verify(c => c.MoveToHand(), Times.Once);

            // Test no exception when more than deck
            player = BuildPlayer(Faction.empire, 2, 2, 0, 0, 0);
            player.DrawCards(5);
            AssertAllSizes(player, 0, 0, 4, 0, 0);
            foreach (var c in player.Hand.BaseList)
            {
                Mock.Get(c).Verify(card => card.MoveToHand(), Times.Once);
            }
        }

        [Test]
        public void DiscardUnits()
        {
            var player = BuildPlayer(Faction.empire, 0, 0, 0, 1, 0);
            player.DiscardUnits();
            AssertAllSizes(player, 0, 1, 0, 0, 0);
            var card = player.Discard.BaseList[0];
            Mock.Get(card).Verify(c => c.MoveToDiscard(), Times.Once);

            player = BuildPlayer(Faction.empire, 2, 2, 2, 2, 0);
            player.DiscardUnits();
            AssertAllSizes(player, 2, 4, 2, 0, 0);
        }

        [Test]
        public void DiscardHand()
        {
            var player = BuildPlayer(Faction.empire, 0, 0, 2, 0, 0);
            player.DiscardHand();
            AssertAllSizes(player, 0, 2, 0, 0, 0);
            foreach (var card in player.Discard.BaseList)
            {
                Mock.Get(card).Verify(c => c.MoveToDiscard(), Times.Once);
            }

            player = BuildPlayer(Faction.empire, 2, 2, 2, 2, 2);
            player.DiscardHand();
            AssertAllSizes(player, 2, 4, 0, 2, 2);
        }

        [Test]
        public void GetAvailableAttack()
        {
            var player = BuildPlayer(Faction.empire, 0, 0, 0, 3, 3);
            // attack == 0 + 1 + 2 + 0 + 1 + 2 = 6
            That(player.GetAvailableAttack(), Is.EqualTo(6));

            // Mark unit 2 as has attacked
            Mock.Get(player.UnitsInPlay.BaseList[2]).Setup(u => u.AbleToAttack()).Returns(false);
            // attack == 0 + 1 + 0 + 1 + 2 = 4
            That(player.GetAvailableAttack(), Is.EqualTo(4));

            // Mark ship 1 as has attacked
            Mock.Get(player.ShipsInPlay.BaseList[1]).Setup(s => s.AbleToAttack()).Returns(false);
            // attack == 0 + 1 + 0 + 2 = 3
            That(player.GetAvailableAttack(), Is.EqualTo(3));

            // Mark unit 1 as has attacked
            Mock.Get(player.UnitsInPlay.BaseList[1]).Setup(u => u.AbleToAttack()).Returns(false);
            // attack == 0 + 0 + 2 = 2
            That(player.GetAvailableAttack(), Is.EqualTo(2));

            // Mark ship 0 as has attacked
            Mock.Get(player.ShipsInPlay.BaseList[0]).Setup(s => s.AbleToAttack()).Returns(false);
            // attack == 0 + 2 = 2
            That(player.GetAvailableAttack(), Is.EqualTo(2));
        }

        private Player BuildImperialPlayer()
        {
            return BuildPlayer(Faction.empire, 0, 0, 0, 0, 0);
        }

        private Player BuildRebelPlayer()
        {
            return BuildPlayer(Faction.rebellion, 0, 0, 0, 0, 0);
        }

        private Player BuildPlayer(Faction faction, int deckSize, int discardSize, int handSize, int unitsInPlay, int shipsInPlay)
        {
            var player = new Player(faction);
            player.Game = game.Object;
            player.Deck = BuildCardList(deckSize, player);
            player.Discard = BuildCardList(discardSize, player);
            player.Hand = BuildCardList(handSize, player);
            player.UnitsInPlay = BuildUnitList(unitsInPlay, player);
            player.ShipsInPlay = BuildShipList(shipsInPlay);
            return player;
        }

        private CastedList<ICard, IUnit> BuildUnitList(int amount, Player player)
        {
            var units = new CastedList<ICard, IUnit>();
            for (int i = 0; i < amount; i++)
            {
                var unit = new Mock<IUnit>();
                unit.Setup(u => u.MoveToDiscard()).Callback(() =>
                {
                    units.Remove(unit.Object);
                    player.Discard.Add(unit.Object);
                });
                unit.Setup(u => u.Attack).Returns(i);
                unit.Setup(u => u.AbleToAttack()).Returns(true);
                units.Add(unit.Object);
            }
            return units;
        }

        private CastedList<ICard, ICapitalShip> BuildShipList(int amount)
        {
            var ships = new CastedList<ICard, ICapitalShip>();
            for (int i = 0; i < amount; i++)
            {
                var ship = new Mock<ICapitalShip>();
                ship.Setup(s => s.Attack).Returns(i);
                ship.Setup(s => s.AbleToAttack()).Returns(true);
                ships.Add(ship.Object);
            }
            return ships;
        }

        private CastedList<ICard, IPlayableCard> BuildCardList(int amount, Player player)
        {
            var cards = new CastedList<ICard, IPlayableCard>();
            for (int i = 0; i < amount; i++)
            {
                var cardMock = new Mock<IPlayableCard>();
                cardMock.Setup(c => c.MoveToHand()).Callback(() =>
                {
                    cards.Remove(cardMock.Object);
                    player.Hand.BaseList.Add(cardMock.Object);
                });
                cardMock.Setup(c => c.MoveToDiscard()).Callback(() =>
                {
                    cards.Remove(cardMock.Object);
                    player.Discard.BaseList.Add(cardMock.Object);
                });
                cards.Add(cardMock.Object);
            }
            return cards;
        }

        private void AssertAllSizes(Player player, int deckSize, int discardSize, int handSize, int unitsInPlay, int shipsInPlay)
        {
            That(player.Deck, Has.Count.EqualTo(deckSize));
            That(player.Discard, Has.Count.EqualTo(discardSize));
            That(player.Hand, Has.Count.EqualTo(handSize));
            That(player.UnitsInPlay, Has.Count.EqualTo(unitsInPlay));
            That(player.ShipsInPlay, Has.Count.EqualTo(shipsInPlay));
        }
    }
}