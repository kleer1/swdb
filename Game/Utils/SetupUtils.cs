using Game.Cards.Common.Models.Interface;
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
using SWDB.Game.Utils;

namespace SWDB.Cards.Utils
{
    public class Counter
    {
        private int _counter = 0;
        public int GetAndIncrement()
        {
            int temp = _counter;
            _counter++;
            return temp;
        }
    }
    
    public static class SetupUtils
    {
        private delegate Card CardBuilder();
        public static IDictionary<int, ICard> Setup(SWDBGame game)
        {
            Counter counter = new();
            IDictionary<int, ICard> cardMap = new Dictionary<int, ICard>();
            BuildImperialDeck(counter, cardMap, game);
            BuildImperialCards(counter, cardMap, game);
            BuildRebelDeck(counter, cardMap, game);
            BuildRebelCards(counter, cardMap, game);
            BuildOuterRimPilots(counter, cardMap, game);
            BuildNeutralCards(counter, cardMap, game);
            BuildImperialBases(counter, cardMap, game);
            BuildRebelBases(counter, cardMap, game);
            game.GalaxyDeck.Shuffle();
            return cardMap;
        }

        private static void BuildImperialDeck(Counter counter, IDictionary<int, ICard> cardMap, SWDBGame game) {
            // Add Shuttles
            BuildCards(7, cardMap, () => new ImperialShuttle(counter.GetAndIncrement(), game));
            // Add troopers
            BuildCards(2, cardMap, () => new Stormtrooper(counter.GetAndIncrement(), game));
            // Add inquisitor
            BuildCards(1, cardMap, () => new Inquisitor(counter.GetAndIncrement(), game));
            game.Empire.Deck.Shuffle();
        }

        private static void BuildImperialBases(Counter counter, IDictionary<int, ICard> cardMap, SWDBGame game) {
            BuildCards(1, cardMap, () => new Lothal(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new DeathStar(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new Mustafar(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new Corellia(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new Kessel(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new Kafrene(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new Rodia(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new OrdMantell(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new Endor(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new Coruscant(counter.GetAndIncrement(), game));
        }

        private static void BuildRebelBases(Counter counter, IDictionary<int, ICard> cardMap, SWDBGame game) {
            BuildCards(1, cardMap, () => new Dantooine(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new Hoth(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new MonCala(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new Dagobah(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new Sullust(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new Bespin(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new Jedha(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new Alderaan(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new Tatooine(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new Yavin4(counter.GetAndIncrement(), game));
        }

        private static void BuildRebelDeck(Counter counter, IDictionary<int, ICard> cardMap, SWDBGame game) {
            // Add Shuttles
            BuildCards(7, cardMap, () => new AllianceShuttle(counter.GetAndIncrement(), game));
            // Add troopers
            BuildCards(2, cardMap, () => new RebelTrooper(counter.GetAndIncrement(), game));
            // Add guardian
            BuildCards(1, cardMap, () => new TempleGuardian(counter.GetAndIncrement(), game));
            game.Rebel.Deck.Shuffle();
        }

        private static void BuildOuterRimPilots(Counter counter, IDictionary<int, ICard> cardMap, SWDBGame game) {
            BuildCards(10, cardMap, () => new OuterRimPilot(counter.GetAndIncrement(), game));
        }

        private static void BuildImperialCards(Counter counter, IDictionary<int, ICard> cardMap, SWDBGame game) {
            BuildCards(3, cardMap, () => new TieFighter(counter.GetAndIncrement(), game));
            BuildCards(2, cardMap, () => new DeathTrooper(counter.GetAndIncrement(), game));
            BuildCards(2, cardMap, () => new TieBomber(counter.GetAndIncrement(), game));
            BuildCards(2, cardMap, () => new ScoutTrooper(counter.GetAndIncrement(), game));
            BuildCards(2, cardMap, () => new TieInterceptor(counter.GetAndIncrement(), game));
            BuildCards(2, cardMap, () => new AtSt(counter.GetAndIncrement(), game));
            BuildCards(2, cardMap, () => new LandingCraft(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new AtAt(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new AdmiralPiett(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new GeneralVeers(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new MoffJerjerrod(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new BobaFett(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new DirectorKrennic(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new GrandMoffTarkin(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new DarthVader(counter.GetAndIncrement(), game));
            BuildCards(3, cardMap, () => new GozantiCruiser(counter.GetAndIncrement(), game));
            BuildCards(2, cardMap, () => new ImperialCarrier(counter.GetAndIncrement(), game));
            BuildCards(2, cardMap, () => new StarDestroyer(counter.GetAndIncrement(), game));
        }

        private static void BuildRebelCards(Counter counter, IDictionary<int, ICard> cardMap, SWDBGame game) {
            BuildCards(2, cardMap, () => new YWing(counter.GetAndIncrement(), game));
            BuildCards(2, cardMap, () => new Snowspeeder(counter.GetAndIncrement(), game));
            BuildCards(2, cardMap, () => new DurosSpy(counter.GetAndIncrement(), game));
            BuildCards(2, cardMap, () => new RebelCommando(counter.GetAndIncrement(), game));
            BuildCards(3, cardMap, () => new XWing(counter.GetAndIncrement(), game));
            BuildCards(2, cardMap, () => new UWing(counter.GetAndIncrement(), game));
            BuildCards(2, cardMap, () => new BWing(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new BazeMalbus(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new ChirrutImwe(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new JynErso(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new Chewbacca(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new HanSolo(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new CassianAndor(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new PrincessLeia(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new MillenniumFalcon(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new LukeSkywalker(counter.GetAndIncrement(), game));
            BuildCards(2, cardMap, () => new RebelTransport(counter.GetAndIncrement(), game));
            BuildCards(2, cardMap, () => new HammerheadCorvette(counter.GetAndIncrement(), game));
            BuildCards(2, cardMap, () => new MonCalamariCruiser(counter.GetAndIncrement(), game));
        }

        private static void BuildNeutralCards(Counter counter, IDictionary<int, ICard> cardMap, SWDBGame game) {
            BuildCards(2, cardMap, () => new Z95Headhunter(counter.GetAndIncrement(), game));
            BuildCards(2, cardMap, () => new JawaScavenger(counter.GetAndIncrement(), game));
            BuildCards(2, cardMap, () => new RodianGunslinger(counter.GetAndIncrement(), game));
            BuildCards(2, cardMap, () => new KelDorMystic(counter.GetAndIncrement(), game));
            BuildCards(2, cardMap, () => new TwilekSmuggler(counter.GetAndIncrement(), game));
            BuildCards(2, cardMap, () => new FangFighter(counter.GetAndIncrement(), game));
            BuildCards(2, cardMap, () => new Hwk290(counter.GetAndIncrement(), game));
            BuildCards(2, cardMap, () => new QuarrenMercenary(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new Lobot(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new Bossk(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new Dengar(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new Ig88(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new LandoCalrissian(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new JabbasSailBarge(counter.GetAndIncrement(), game));
            BuildCards(1, cardMap, () => new JabbaTheHutt(counter.GetAndIncrement(), game));
            BuildCards(2, cardMap, () => new CrocCruiser(counter.GetAndIncrement(), game));
            BuildCards(3, cardMap, () => new BlockadeRunner(counter.GetAndIncrement(), game));
            BuildCards(2, cardMap, () => new NebulonBFrigate(counter.GetAndIncrement(), game));
        }

        private static void BuildCards(int number, IDictionary<int, ICard> cardMap, CardBuilder cardBuilder) {
            for (int i = 0; i < number; i++) {
                Card card = cardBuilder.Invoke();
                cardMap[card.Id] = card;
            }
        }
    }
}