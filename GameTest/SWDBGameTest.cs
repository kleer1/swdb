using Game.Actions;
using Game.Cards;
using Game.Cards.Common.Models.Interface;
using Game.Common.Interfaces;
using Game.State.Interfaces;
using SWDB.Game;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Empire.Bases;
using SWDB.Game.Cards.Empire.Ships;
using SWDB.Game.Cards.Empire.Units;
using SWDB.Game.Cards.Empire.Units.Starter;
using SWDB.Game.Cards.Neutral.Ships;
using SWDB.Game.Cards.Neutral.Units;
using SWDB.Game.Cards.Rebellion.Bases;
using SWDB.Game.Cards.Rebellion.Ships;
using SWDB.Game.Cards.Rebellion.Units;
using SWDB.Game.Cards.Rebellion.Units.Starter;
using SWDB.Game.Common;
using SWDB.Game.Utils;
using System.Text;

namespace GameTest
{
    [TestFixture]
    public class SWDBGameTest
    {

        [Test]
        public void TestInitialize() 
        {
            SWDBGame game = new SWDBGame();
            That(game.CurrentPlayersTurn, Is.EqualTo(Faction.empire));
            That(game.ForceBalance.Position, Is.EqualTo(6));
            AssertGalaxyState(game);
            AssertOuterRim(game);
            That(game.ExiledCards, Has.Count.EqualTo(0));
            That(game.CardMap.Count, Is.EqualTo(140));
            That(game.StaticEffects, Has.Count.EqualTo(0));
            That(game.KnowsTopCardOfDeck[Faction.empire], Is.EqualTo(0));
            That(game.KnowsTopCardOfDeck[Faction.rebellion], Is.EqualTo(0));
            AssertEmpireStartState(game);
            AssertRebelStartState(game);
            That(game.Empire.Opponent, Is.EqualTo(game.Rebel));
            That(game.Rebel.Opponent, Is.EqualTo(game.Empire));
            That(game.CurrentPlayersAction, Is.EqualTo(Faction.empire));
            That(game.CurrentPlayersTurn, Is.EqualTo(Faction.empire));
            That(game.PendingActions, Has.Count.EqualTo(0));
            That(game.LastCardPlayed, Is.Null);
            That(game.LastCardActivated,Is.Null);
            That(game.Attackers, Has.Count.EqualTo(0));
            That(game.AttackTarget, Is.Null);
            That(game.CanSeeOpponentsHand, Is.False);
            That(game.ExileAtEndOfTurn, Has.Count.EqualTo(0));
            That(game.ANewHope1Card, Is.Null);
        }

        [Test]
        public void TestThatJustAttackingWillEndGame()
        {
            SWDBGame game = new SWDBGame();
            IList<int> actionCounts = new List<int>();
            int iterations = 10;
            for (int i = 1; i <= iterations; i++)
            {
                game = new SWDBGame();
                int numActions = 0;
                while (!game.IsGameOver)
                {
                    Faction faction = game.CurrentPlayersTurn;
                    IBase? _base = game.GetCurrentPlayer().Opponent?.CurrentBase;
                    // play ever card in hand
                    IList<int> cardIds = game.GetCurrentPlayer().Hand.Select(c => c.Id).ToList();
                    foreach (int id in cardIds)
                    {
                        game.ApplyAction(Action.PlayCard, id);
                        numActions++;
                        if (game.PendingActions.Any())
                        {
                            // always choose attack
                            numActions++;
                            game.ApplyAction(Action.ChooseStatBoost, stats: Stats.Attack);
                        }
                    }
                    LogCardList(game.GetCurrentPlayer().UnitsInPlay, CardLocationHelper.GetUnitsInPlay(game.CurrentPlayersTurn));

                    if (game.GetCurrentPlayer().GetAvailableAttack() > 0 && _base != null)
                    {
                        numActions++;
                        game.ApplyAction(Action.AttackBase, _base.Id);
                        // System.out.println("Attacking " + game.getAttackTarget().getTitle());

                        // select all attackers
                        cardIds = game.GetCurrentPlayer().UnitsInPlay.Select(c => c.Id).ToList();
                        cardIds.AddRange(game.GetCurrentPlayer().ShipsInPlay.Select(c => c.Id).ToList());
                        foreach (int id in cardIds)
                        {
                            numActions++;
                            game.ApplyAction(Action.SelectAttacker, id);
                        }
                        // logCardList(game.getAttackers(), CardLocation.getUnitsInPlay(game.getCurrentPlayersTurn()));
                        // confirm attack
                        numActions++;
                        game.ApplyAction(Action.ConfirmAttackers);
                        _base = game.GetCurrentPlayer().Opponent?.CurrentBase;
                        if (_base != null)
                        {
                            Console.WriteLine(game.GetCurrentPlayer().Opponent?.Faction + " base has " +
                                    _base.GetRemainingHealth() + " remaining health");
                        }
                        else
                        {
                            Console.WriteLine(game.CurrentPlayersTurn + " destroyed opponents base");
                        }
                    }

                    // try to buy cards with attack starting with highest
                    cardIds = game.GalaxyRow.BaseList
                        .Where(c => c.Attack > 0 && new HashSet<Faction> { game.CurrentPlayersTurn, Faction.neutral}.Contains(c.Faction))
                        .OrderBy(c => c.Attack).Reverse().Select(c => c.Id).ToList();

                    foreach (int id in cardIds)
                    {
                        numActions++;
                        game.ApplyAction(Action.PurchaseCard, id);
                        if (game.PendingActions.Any())
                        {
                            numActions++;
                            game.ApplyAction(Action.DeclineAction);
                        }
                    }
                    LogCardList(game.GetCurrentPlayer().Discard, CardLocationHelper.GetDiscard(game.CurrentPlayersTurn));


                    // pass turn after attack
                    Console.WriteLine(faction + " passing turn");
                    numActions++;
                    game.ApplyAction(Action.PassTurn);

                    // check if new base needs to be selected
                    if (game.PendingActions.Any())
                    {
                        IList<IBase> availableBases = game.GetCurrentPlayer().AvailableBases.BaseList;
                        IBase newBase = availableBases[Random.Shared.Next(availableBases.Count)];
                        numActions++;
                        game.ApplyAction(Action.ChooseNextBase, newBase.Id);
                       Console.WriteLine(game.CurrentPlayersTurn + " choose new base: " + newBase.Title);
                        // decline if the base has an ability
                        if (game.PendingActions.Any())
                        {
                            numActions++;
                            game.ApplyAction(Action.DeclineAction);
                        }
                        // galactic rule can't be declined
                        if (game.PendingActions.Any())
                        {
                            numActions++;
                            game.ApplyAction(Action.GalacticRule, game.GalaxyDeck.First().Id);
                        }
                    }
                }
                actionCounts.Add(numActions);
                Console.WriteLine("Game " + i + " complete");
            }
            Console.WriteLine("Average number of actions in game: " + (actionCounts.Sum() / (double)iterations));
            Console.WriteLine("Max actions: " + actionCounts.Max());
        }

        //[Test]
        //public void TestRandomActions()
        //{
        //    SWDBGame game = new();
        //    // Start by playing the first card
        //    IGameState gameState = game.ApplyAction(Action.PlayCard, game.GetCurrentPlayer().Hand.First().Id);
        //    while(!game.IsGameOver)
        //    {
        //        IList<GameAction> notPass = gameState.ValidActions.Where(ga => ga.Action != Action.PassTurn).ToList();
        //        GameAction gameAction;
        //        if (notPass.Any()) 
        //        {
        //            gameAction = notPass.ElementAt(Random.Shared.Next(notPass.Count));
        //        } else
        //        {
        //            gameAction = new(Action.PassTurn);
        //        }
        //        Console.WriteLine(gameAction);
        //        gameState = game.ApplyAction(gameAction);
        //    }
        //}

        private static IPlayableCard? GetCardAndMoveToInPlay(Type type, SWDBGame game) 
        {
            IPlayableCard? card = GetFromCardMap(type, game.CardMap);
            if (card == null) {
                throw new ArgumentException("Card not in map");
            }
            MoveCardToInPlay(card, game);
            return card;
        }

        private static void MoveCardToInPlay(IPlayableCard card, SWDBGame game) {
            card.BuyToHand(game.GetCurrentPlayer());
            card.MoveToInPlay();
        }

        private static IPlayableCard? GetFromCardMap(Type type, IDictionary<int, ICard> cardMap) 
        {
            ICard? card = cardMap.Values.Where(c => c.GetType() == type).First();
            if (card == null) {
                return null;
            }
            if (card is IPlayableCard playableCard) {
                return playableCard;
            }
            return null;
        }

        private int CountByType(IList<ICard> cardList, Type type) 
        {
            return cardList.Where(c => c.GetType() == type).Count();
        }

        private IList<T> JoinLists<T>(IList<T> list1, IList<T> list2) 
        {
            IList<T> joinedList = new List<T>();

            foreach (T t in list1)
            {
                joinedList.Add(t);
            }

            foreach (T t in list2)
            {
                joinedList.Add(t);
            }

            return joinedList;
        }

        private void AssertStartingDeckSizes(IPlayer player) 
        {
            That(player.Hand, Has.Count.EqualTo(5));
            That(player.Deck, Has.Count.EqualTo(5));
            That(player.Discard, Has.Count.EqualTo(0));
            That(player.ShipsInPlay, Has.Count.EqualTo(0));
            That(player.UnitsInPlay, Has.Count.EqualTo(0));
        }

        private void AssertEmpireStartState(SWDBGame game) 
        {
            IPlayer player = game.Empire;
            That(player.Faction, Is.EqualTo(Faction.empire));
            AssertStartingDeckSizes(player);
            foreach (IPlayableCard card in player.Hand) 
            {
                That(card.Id, Is.InRange(CardMapping.EmpireStartingCards.MinRange(), CardMapping.EmpireStartingCards.MaxRange()));
                That(card.Location, Is.EqualTo(CardLocation.EmpireHand));
                That(card.Owner, Is.EqualTo(player));
                That(card.CardList, Is.EqualTo(player.Hand));
                That(card.Id, Is.InRange(CardMapping.EmpirePlayableCards.MinRange(), CardMapping.EmpirePlayableCards.MaxRange()));
            }

            foreach (IPlayableCard card in player.Deck) 
            {
                That(card.Id, Is.InRange(CardMapping.EmpireStartingCards.MinRange(), CardMapping.EmpireStartingCards.MaxRange()));
                That(card.Location, Is.EqualTo(CardLocation.EmpireDeck));
                That(card.Owner, Is.EqualTo(player));
                That(card.CardList, Is.EqualTo(player.Deck));
                That(card.Id, Is.InRange(CardMapping.EmpirePlayableCards.MinRange(), CardMapping.EmpirePlayableCards.MaxRange()));
            }
            int numShuttle = CountByType(JoinLists(player.Hand, player.Deck), typeof(ImperialShuttle));
            That(numShuttle, Is.EqualTo(7));
            int numTrooper = CountByType(JoinLists(player.Hand, player.Deck), typeof(Stormtrooper));
            That(numTrooper, Is.EqualTo(2));
            int numInq = CountByType(JoinLists(player.Hand, player.Deck), typeof(Inquisitor));
            That(numInq, Is.EqualTo(1));

            IBase? currentBase = player.CurrentBase;
            That(currentBase, Is.Not.Null);
            That(currentBase, Is.TypeOf(typeof(Lothal)));
            That(currentBase?.Location, Is.EqualTo(CardLocation.EmpireCurrentBase));
            That(currentBase?.Owner, Is.EqualTo(player));
            That(currentBase?.CardList, Is.Null);
            That(currentBase?.Id, Is.InRange(CardMapping.EmpireBases.MinRange(), CardMapping.EmpireBases.MaxRange()));

            That(player.AvailableBases, Has.Count.EqualTo(9));
            foreach (Base _base in player.AvailableBases) 
            {
                That(_base.Location, Is.EqualTo(CardLocation.EmpireAvailableBases));
                That(_base.Owner, Is.EqualTo(player));
                That(_base.CardList, Is.EqualTo(player.AvailableBases));
                That(_base.Id, Is.InRange(CardMapping.EmpireBases.MinRange(), CardMapping.EmpireBases.MaxRange()));
            }

            That(CountByType(player.AvailableBases, typeof(Corellia)), Is.EqualTo(1));
            That(CountByType(player.AvailableBases, typeof(Coruscant)), Is.EqualTo(1));
            That(CountByType(player.AvailableBases, typeof(DeathStar)), Is.EqualTo(1));
            That(CountByType(player.AvailableBases, typeof(Endor)), Is.EqualTo(1));
            That(CountByType(player.AvailableBases, typeof(Kafrene)), Is.EqualTo(1));
            That(CountByType(player.AvailableBases, typeof(Kessel)), Is.EqualTo(1));
            That(CountByType(player.AvailableBases, typeof(Mustafar)), Is.EqualTo(1));
            That(CountByType(player.AvailableBases, typeof(OrdMantell)), Is.EqualTo(1));
            That(CountByType(player.AvailableBases, typeof(Rodia)), Is.EqualTo(1));

            That(player.Resources, Is.EqualTo(0));
        }

        private void AssertRebelStartState(SWDBGame game) 
        {
            IPlayer player = game.Rebel;
            That(player.Faction, Is.EqualTo(Faction.rebellion));
            AssertStartingDeckSizes(player);
            foreach (PlayableCard card in player.Hand) 
            {
                That(card.Id, Is.InRange(CardMapping.RebelStartingCards.MinRange(), CardMapping.RebelStartingCards.MaxRange()));
                That(card.Location, Is.EqualTo(CardLocation.RebelHand));
                That(card.Owner, Is.EqualTo(player));
                That(card.CardList, Is.EqualTo(player.Hand));
                That(card.Id, Is.InRange(CardMapping.RebelPlayableCards.MinRange(), CardMapping.RebelPlayableCards.MaxRange()));
            }

            foreach (PlayableCard card in player.Deck) 
            {
                That(card.Id, Is.InRange(CardMapping.RebelStartingCards.MinRange(), CardMapping.RebelStartingCards.MaxRange()));
                That(card.Location, Is.EqualTo(CardLocation.RebelDeck));
                That(card.Owner, Is.EqualTo(player));
                That(card.CardList, Is.EqualTo(player.Deck));
                That(card.Id, Is.InRange(CardMapping.RebelPlayableCards.MinRange(), CardMapping.RebelPlayableCards.MaxRange()));
            }
            int numShuttle = CountByType(JoinLists(player.Hand, player.Deck), typeof(AllianceShuttle));
            That(numShuttle, Is.EqualTo(7));
            int numTrooper = CountByType(JoinLists(player.Hand, player.Deck), typeof(RebelTrooper));
            That(numTrooper, Is.EqualTo(2));
            int numGuard = CountByType(JoinLists(player.Hand, player.Deck), typeof(TempleGuardian));
            That(numGuard, Is.EqualTo(1));

            IBase? currentBase = player.CurrentBase;
            That(currentBase, Is.Not.Null);
            That(currentBase, Is.TypeOf(typeof(Dantooine)));
            That(currentBase?.Location, Is.EqualTo(CardLocation.RebelCurrentBase));
            That(currentBase?.Owner, Is.EqualTo(player));
            That(currentBase?.CardList, Is.Null);
            That(currentBase?.Id, Is.InRange(CardMapping.RebelBases.MinRange(), CardMapping.RebelBases.MaxRange()));

            That(player.AvailableBases, Has.Count.EqualTo(9));
            foreach (Base _base in player.AvailableBases) 
            {
                That(_base.Location, Is.EqualTo(CardLocation.RebelAvailableBases));
                That(_base.Owner, Is.EqualTo(player));
                That(_base.CardList, Is.EqualTo(player.AvailableBases));
                That(_base.Id, Is.InRange(CardMapping.RebelBases.MinRange(), CardMapping.RebelBases.MaxRange()));
            }
            That(CountByType(player.AvailableBases, typeof(Alderaan)), Is.EqualTo(1));
            That(CountByType(player.AvailableBases, typeof(Bespin)), Is.EqualTo(1));
            That(CountByType(player.AvailableBases, typeof(Dagobah)), Is.EqualTo(1));
            That(CountByType(player.AvailableBases, typeof(Hoth)), Is.EqualTo(1));
            That(CountByType(player.AvailableBases, typeof(Jedha)), Is.EqualTo(1));
            That(CountByType(player.AvailableBases, typeof(MonCala)), Is.EqualTo(1));
            That(CountByType(player.AvailableBases, typeof(Sullust)), Is.EqualTo(1));
            That(CountByType(player.AvailableBases, typeof(Tatooine)), Is.EqualTo(1));
            That(CountByType(player.AvailableBases, typeof(Yavin4)), Is.EqualTo(1));

            That(player.Resources, Is.EqualTo(0));
        }

        private void AssertGalaxyState(SWDBGame game) 
        {
            That(game.GalaxyDeck, Has.Count.EqualTo(84));
            That(game.GalaxyRow, Has.Count.EqualTo(6));
            That(game.GalaxyDiscard, Has.Count.EqualTo(0));
            foreach (PlayableCard card in game.GalaxyRow) {
                CardMapping mapping = CardMappings.GetGalaxyCards(card.Faction);
                That(card.Id, Is.InRange(mapping.MinRange(), mapping.MaxRange()));
                mapping = CardMappings.GetPlayableCards(card.Faction);
                That(card.Id, Is.InRange(mapping.MinRange(), mapping.MaxRange()));
                if (card is CapitalShip) 
                {
                    mapping = CardMappings.GetShipCards(card.Faction);
                    That(card.Id, Is.InRange(mapping.MinRange(), mapping.MaxRange()));
                }
                That(card.Location, Is.EqualTo(CardLocation.GalaxyRow));
                That(card.Owner, Is.Null);
                That(card.CardList, Is.EqualTo(game.GalaxyRow));
            }
            foreach (PlayableCard card in game.GalaxyDeck) {
                CardMapping mapping = CardMappings.GetGalaxyCards(card.Faction);
                That(card.Id, Is.InRange(mapping.MinRange(), mapping.MaxRange()));
                mapping = CardMappings.GetPlayableCards(card.Faction);
                That(card.Id, Is.InRange(mapping.MinRange(), mapping.MaxRange()));
                if (card is CapitalShip) {
                    mapping = CardMappings.GetShipCards(card.Faction);
                    That(card.Id, Is.InRange(mapping.MinRange(), mapping.MaxRange()));
                }
                That(card.Location, Is.EqualTo(CardLocation.GalaxyDeck));
                That(card.Owner, Is.Null);
                That(card.CardList, Is.EqualTo(game.GalaxyDeck));
            }
            IList<ICard> joinedList = JoinLists(game.GalaxyRow, game.GalaxyDeck);
            That(CountByType(joinedList, typeof(NebulonBFrigate)), Is.EqualTo(2));
            That(CountByType(joinedList, typeof(BlockadeRunner)), Is.EqualTo(3));
            That(CountByType(joinedList, typeof(CrocCruiser)), Is.EqualTo(2));
            That(CountByType(joinedList, typeof(JabbaTheHutt)), Is.EqualTo(1));
            That(CountByType(joinedList, typeof(JabbasSailBarge)), Is.EqualTo(1));
            That(CountByType(joinedList, typeof(LandoCalrissian)), Is.EqualTo(1));
            That(CountByType(joinedList, typeof(Ig88)), Is.EqualTo(1));
            That(CountByType(joinedList, typeof(Dengar)), Is.EqualTo(1));
            That(CountByType(joinedList, typeof(Bossk)), Is.EqualTo(1));
            That(CountByType(joinedList, typeof(Lobot)), Is.EqualTo(1));
            That(CountByType(joinedList, typeof(QuarrenMercenary)), Is.EqualTo(2));
            That(CountByType(joinedList, typeof(Hwk290)), Is.EqualTo(2));
            That(CountByType(joinedList, typeof(FangFighter)), Is.EqualTo(2));
            That(CountByType(joinedList, typeof(TwilekSmuggler)), Is.EqualTo(2));
            That(CountByType(joinedList, typeof(KelDorMystic)), Is.EqualTo(2));
            That(CountByType(joinedList, typeof(RodianGunslinger)), Is.EqualTo(2));
            That(CountByType(joinedList, typeof(JawaScavenger)), Is.EqualTo(2));
            That(CountByType(joinedList, typeof(Z95Headhunter)), Is.EqualTo(2));
            That(CountByType(joinedList, typeof(MonCalamariCruiser)), Is.EqualTo(2));
            That(CountByType(joinedList, typeof(HammerheadCorvette)), Is.EqualTo(2));
            That(CountByType(joinedList, typeof(RebelTransport)), Is.EqualTo(2));
            That(CountByType(joinedList, typeof(LukeSkywalker)), Is.EqualTo(1));
            That(CountByType(joinedList, typeof(PrincessLeia)), Is.EqualTo(1));
            That(CountByType(joinedList, typeof(CassianAndor)), Is.EqualTo(1));
            That(CountByType(joinedList, typeof(HanSolo)), Is.EqualTo(1));
            That(CountByType(joinedList, typeof(MillenniumFalcon)), Is.EqualTo(1));
            That(CountByType(joinedList, typeof(Chewbacca)), Is.EqualTo(1));
            That(CountByType(joinedList, typeof(JynErso)), Is.EqualTo(1));
            That(CountByType(joinedList, typeof(ChirrutImwe)), Is.EqualTo(1));
            That(CountByType(joinedList, typeof(BazeMalbus)), Is.EqualTo(1));
            That(CountByType(joinedList, typeof(BWing)), Is.EqualTo(2));
            That(CountByType(joinedList, typeof(UWing)), Is.EqualTo(2));
            That(CountByType(joinedList, typeof(XWing)), Is.EqualTo(3));
            That(CountByType(joinedList, typeof(RebelCommando)), Is.EqualTo(2));
            That(CountByType(joinedList, typeof(DurosSpy)), Is.EqualTo(2));
            That(CountByType(joinedList, typeof(Snowspeeder)), Is.EqualTo(2));
            That(CountByType(joinedList, typeof(YWing)), Is.EqualTo(2));
            That(CountByType(joinedList, typeof(StarDestroyer)), Is.EqualTo(2));
            That(CountByType(joinedList, typeof(ImperialCarrier)), Is.EqualTo(2));
            That(CountByType(joinedList, typeof(GozantiCruiser)), Is.EqualTo(3));
            That(CountByType(joinedList, typeof(DarthVader)), Is.EqualTo(1));
            That(CountByType(joinedList, typeof(GrandMoffTarkin)), Is.EqualTo(1));
            That(CountByType(joinedList, typeof(DirectorKrennic)), Is.EqualTo(1));
            That(CountByType(joinedList, typeof(BobaFett)), Is.EqualTo(1));
            That(CountByType(joinedList, typeof(MoffJerjerrod)), Is.EqualTo(1));
            That(CountByType(joinedList, typeof(ScoutTrooper)), Is.EqualTo(2));
            That(CountByType(joinedList, typeof(TieInterceptor)), Is.EqualTo(2));
            That(CountByType(joinedList, typeof(GeneralVeers)), Is.EqualTo(1));
            That(CountByType(joinedList, typeof(AdmiralPiett)), Is.EqualTo(1));
            That(CountByType(joinedList, typeof(AtAt)), Is.EqualTo(1));
            That(CountByType(joinedList, typeof(LandingCraft)), Is.EqualTo(2));
            That(CountByType(joinedList, typeof(AtSt)), Is.EqualTo(2));
            That(CountByType(joinedList, typeof(TieBomber)), Is.EqualTo(2));
            That(CountByType(joinedList, typeof(TieFighter)), Is.EqualTo(3));
            That(CountByType(joinedList, typeof(DeathTrooper)), Is.EqualTo(2));
        }

        private void AssertOuterRim(SWDBGame game) {
            That(game.OuterRimPilots, Has.Count.EqualTo(10));
            foreach (PlayableCard card in game.OuterRimPilots) {
                That(card.Id, Is.InRange(CardMapping.NeutralOuterRimCards.MinRange(), CardMapping.NeutralOuterRimCards.MaxRange()));
                That(card.Id, Is.InRange(CardMapping.NeutralPlayableCards.MinRange(), CardMapping.NeutralPlayableCards.MaxRange()));
                That(card.Location, Is.EqualTo(CardLocation.OuterRimPilots));
                That(card.Owner, Is.Null);
                That(card.CardList, Is.EqualTo(game.OuterRimPilots));
            }
            That(CountByType(game.OuterRimPilots, typeof(OuterRimPilot)), Is.EqualTo(10));
        }

        private static void LogCardList<T>(IList<T> cards, CardLocation cardLocation) where T : ICard
        {
            StringBuilder builder = new();
            builder.Append("Location: ")
                    .Append(cardLocation)
                    .Append(" has [");
            foreach (T card in cards)
            {
                builder.Append("{")
                        .Append(card.Id)
                        .Append(", ")
                        .Append(card.Title)
                        .Append("},");
            }
            builder.Append("]");
            Console.WriteLine(builder);
        }

    }
}