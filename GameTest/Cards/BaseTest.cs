using Game.Cards.Common.Models.Interface;
using GameTest.Cards.Interfaces;
using SWDB.Game;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards
{
    public class BaseTest : IHasMoveToInPlay, IHasGame
    {
        protected static readonly int NEUTRAL_GALAXY_CARD = 90;
        protected static readonly int EMPIRE_GALAXY_CARD = 10;
        protected static readonly int REBEL_GALAXY_CARD = 50;
        protected static readonly int LUKE_ID = 73;

        private static IReadOnlyCollection<CardLocation> BUY_LOCATION = new HashSet<CardLocation> {
            CardLocation.GalaxyRow,
            CardLocation.GalaxyDiscard,
            CardLocation.GalaxyDeck,
            CardLocation.OuterRimPilots
        };

        public SWDBGame Game { get; private set; }

        [SetUp]
        public void BaseSetup()
        {
            Game = new SWDBGame();
        }

        public IList<IPlayableCard> MoveToInPlay(Type type, Player? player) 
        {
            return MoveToInPlay(type, player, 1);
        }

        public IList<IPlayableCard> MoveToInPlay(Type type, Player? player, int amount) 
        {
            if (player == null) return new List<IPlayableCard>();

            IList<IPlayableCard> cards = new List<IPlayableCard>();
            foreach (Card card in Game.CardMap.Values) 
            {
                if (card is IPlayableCard playableCard) 
                {
                    if (playableCard.GetType() == type) 
                    {
                        if (BUY_LOCATION.Contains(playableCard.Location)) 
                        {
                            playableCard.BuyToHand(player);
                        }
                        playableCard.MoveToInPlay();
                        cards.Add(playableCard);
                        if (cards.Count == amount) {
                            return cards;
                        }
                    }
                }
            }
            return cards;
        }

        protected void EmptyGalaxyRow() 
        {
            for (int i = Game.GalaxyRow.Count - 1; i >= 0; i--)
            {
                IPlayableCard card = Game.GalaxyRow.BaseList[i];
                Game.GalaxyRow.RemoveAt(i);
                card.MoveToTopOfGalaxyDeck();
            }
        }

        protected IList<IPlayableCard> MoveToGalaxyRow(Type type) 
        {
            return MoveToGalaxyRow(type, 1);
        }

        protected IList<IPlayableCard> MoveToGalaxyRow(Type type, int amount) 
        {
            IList<IPlayableCard> cards = new List<IPlayableCard>();
            foreach (Card card in Game.CardMap.Values) 
            {
                if (card is IPlayableCard playableCard) 
                {
                    if (playableCard.GetType() == type) 
                    {
                        playableCard.MoveToGalaxyRow();
                        cards.Add(playableCard);
                        if (cards.Count == amount) {
                            return cards;
                        }
                    }
                }
            }
            return cards;
        }
    }
}