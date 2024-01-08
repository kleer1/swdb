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
    
    public static class SetupUtils
    {
        private delegate Card CardBuilder();
        public static IDictionary<int, Card> Setup(SWDBGame game)
        {
            IDictionary<int, Card> cardMap = new Dictionary<int, Card>();
            int id = 0;
            BuildImperialDeck(id, cardMap, game);
            BuildImperialCards(id, cardMap, game);
            BuildRebelDeck(id, cardMap, game);
            BuildRebelCards(id, cardMap, game);
            BuildOuterRimPilots(id, cardMap, game);
            BuildNeutralCards(id, cardMap, game);
            BuildImperialBases(id, cardMap, game);
            BuildRebelBases(id, cardMap, game);
            game.GalaxyDeck.Shuffle();
            return cardMap;
        }

        private static void BuildImperialDeck(int currentCardId, IDictionary<int, Card> cardMap, SWDBGame game) {
            // Add Shuttles
            BuildCards(7, cardMap, () => new ImperialShuttle(currentCardId++, game));
            // Add troopers
            BuildCards(2, cardMap, () => new Stormtrooper(currentCardId++, game));
            // Add inquisitor
            BuildCards(1, cardMap, () => new Inquisitor(currentCardId++, game));
            game.Empire.Deck.Shuffle();
        }

        private static void BuildImperialBases(int currentCardId, IDictionary<int, Card> cardMap, SWDBGame game) {
            BuildCards(1, cardMap, () => new Lothal(currentCardId++, game));
            BuildCards(1, cardMap, () => new DeathStar(currentCardId++, game));
            BuildCards(1, cardMap, () => new Mustafar(currentCardId++, game));
            BuildCards(1, cardMap, () => new Corellia(currentCardId++, game));
            BuildCards(1, cardMap, () => new Kessel(currentCardId++, game));
            BuildCards(1, cardMap, () => new Kafrene(currentCardId++, game));
            BuildCards(1, cardMap, () => new Rodia(currentCardId++, game));
            BuildCards(1, cardMap, () => new OrdMantell(currentCardId++, game));
            BuildCards(1, cardMap, () => new Endor(currentCardId++, game));
            BuildCards(1, cardMap, () => new Coruscant(currentCardId++, game));
        }

        private static void BuildRebelBases(int currentCardId, IDictionary<int, Card> cardMap, SWDBGame game) {
            BuildCards(1, cardMap, () => new Dantooine(currentCardId++, game));
            BuildCards(1, cardMap, () => new Hoth(currentCardId++, game));
            BuildCards(1, cardMap, () => new MonCala(currentCardId++, game));
            BuildCards(1, cardMap, () => new Dagobah(currentCardId++, game));
            BuildCards(1, cardMap, () => new Sullust(currentCardId++, game));
            BuildCards(1, cardMap, () => new Bespin(currentCardId++, game));
            BuildCards(1, cardMap, () => new Jedha(currentCardId++, game));
            BuildCards(1, cardMap, () => new Alderaan(currentCardId++, game));
            BuildCards(1, cardMap, () => new Tatooine(currentCardId++, game));
            BuildCards(1, cardMap, () => new Yavin4(currentCardId++, game));
        }

        private static void BuildRebelDeck(int currentCardId, IDictionary<int, Card> cardMap, SWDBGame game) {
            // Add Shuttles
            BuildCards(7, cardMap, () => new AllianceShuttle(currentCardId++, game));
            // Add troopers
            BuildCards(2, cardMap, () => new RebelTrooper(currentCardId++, game));
            // Add guardian
            BuildCards(1, cardMap, () => new TempleGuardian(currentCardId++, game));
            game.Rebel.Deck.Shuffle();
        }

        private static void BuildOuterRimPilots(int currentCardId, IDictionary<int, Card> cardMap, SWDBGame game) {
            BuildCards(10, cardMap, () => new OuterRimPilot(currentCardId++, game));
        }

        private static void BuildImperialCards(int currentCardId, IDictionary<int, Card> cardMap, SWDBGame game) {
            BuildCards(3, cardMap, () => new TieFighter(currentCardId++, game));
            BuildCards(2, cardMap, () => new DeathTrooper(currentCardId++, game));
            BuildCards(2, cardMap, () => new TieBomber(currentCardId++, game));
            BuildCards(2, cardMap, () => new ScoutTrooper(currentCardId++, game));
            BuildCards(2, cardMap, () => new TieInterceptor(currentCardId++, game));
            BuildCards(2, cardMap, () => new AtSt(currentCardId++, game));
            BuildCards(2, cardMap, () => new LandingCraft(currentCardId++, game));
            BuildCards(1, cardMap, () => new AtAt(currentCardId++, game));
            BuildCards(1, cardMap, () => new AdmiralPiett(currentCardId++, game));
            BuildCards(1, cardMap, () => new GeneralVeers(currentCardId++, game));
            BuildCards(1, cardMap, () => new MoffJerjerrod(currentCardId++, game));
            BuildCards(1, cardMap, () => new BobaFett(currentCardId++, game));
            BuildCards(1, cardMap, () => new DirectorKrennic(currentCardId++, game));
            BuildCards(1, cardMap, () => new GrandMoffTarkin(currentCardId++, game));
            BuildCards(1, cardMap, () => new DarthVader(currentCardId++, game));
            BuildCards(3, cardMap, () => new GozantiCruiser(currentCardId++, game));
            BuildCards(2, cardMap, () => new ImperialCarrier(currentCardId++, game));
            BuildCards(2, cardMap, () => new StarDestroyer(currentCardId++, game));
        }

        private static void BuildRebelCards(int currentCardId, IDictionary<int, Card> cardMap, SWDBGame game) {
            BuildCards(2, cardMap, () => new YWing(currentCardId++, game));
            BuildCards(2, cardMap, () => new Snowspeeder(currentCardId++, game));
            BuildCards(2, cardMap, () => new DurosSpy(currentCardId++, game));
            BuildCards(2, cardMap, () => new RebelCommando(currentCardId++, game));
            BuildCards(3, cardMap, () => new XWing(currentCardId++, game));
            BuildCards(2, cardMap, () => new UWing(currentCardId++, game));
            BuildCards(2, cardMap, () => new BWing(currentCardId++, game));
            BuildCards(1, cardMap, () => new BazeMalbus(currentCardId++, game));
            BuildCards(1, cardMap, () => new ChirrutImwe(currentCardId++, game));
            BuildCards(1, cardMap, () => new JynErso(currentCardId++, game));
            BuildCards(1, cardMap, () => new Chewbacca(currentCardId++, game));
            BuildCards(1, cardMap, () => new HanSolo(currentCardId++, game));
            BuildCards(1, cardMap, () => new CassianAndor(currentCardId++, game));
            BuildCards(1, cardMap, () => new PrincessLeia(currentCardId++, game));
            BuildCards(1, cardMap, () => new MillenniumFalcon(currentCardId++, game));
            BuildCards(1, cardMap, () => new LukeSkywalker(currentCardId++, game));
            BuildCards(2, cardMap, () => new RebelTransport(currentCardId++, game));
            BuildCards(2, cardMap, () => new HammerheadCorvette(currentCardId++, game));
            BuildCards(2, cardMap, () => new MonCalamariCruiser(currentCardId++, game));
        }

        private static void BuildNeutralCards(int currentCardId, IDictionary<int, Card> cardMap, SWDBGame game) {
            BuildCards(2, cardMap, () => new Z95Headhunter(currentCardId++, game));
            BuildCards(2, cardMap, () => new JawaScavenger(currentCardId++, game));
            BuildCards(2, cardMap, () => new RodianGunslinger(currentCardId++, game));
            BuildCards(2, cardMap, () => new KelDorMystic(currentCardId++, game));
            BuildCards(2, cardMap, () => new TwilekSmuggler(currentCardId++, game));
            BuildCards(2, cardMap, () => new FangFighter(currentCardId++, game));
            BuildCards(2, cardMap, () => new Hwk290(currentCardId++, game));
            BuildCards(2, cardMap, () => new QuarrenMercenary(currentCardId++, game));
            BuildCards(1, cardMap, () => new Lobot(currentCardId++, game));
            BuildCards(1, cardMap, () => new Bossk(currentCardId++, game));
            BuildCards(1, cardMap, () => new Dengar(currentCardId++, game));
            BuildCards(1, cardMap, () => new Ig88(currentCardId++, game));
            BuildCards(1, cardMap, () => new LandoCalrissian(currentCardId++, game));
            BuildCards(1, cardMap, () => new JabbasSailBarge(currentCardId++, game));
            BuildCards(1, cardMap, () => new JabbaTheHutt(currentCardId++, game));
            BuildCards(2, cardMap, () => new CrocCruiser(currentCardId++, game));
            BuildCards(3, cardMap, () => new BlockadeRunner(currentCardId++, game));
            BuildCards(2, cardMap, () => new NebulonBFrigate(currentCardId++, game));
        }

        private static void BuildCards(int number, IDictionary<int, Card> cardMap, CardBuilder cardBuilder) {
            for (int i = 0; i < number; i++) {
                Card card = cardBuilder.Invoke();
                cardMap[card.Id] = card;
            }
        }
    }
}