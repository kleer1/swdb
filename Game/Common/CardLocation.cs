namespace SWDB.Game.Common
{
    public enum CardLocation
    {
        EmpireHand,
        EmpireDeck,
        EmpireDiscard,
        EmpireUnitInPlay,
        EmpireShipInPlay,
        EmpireCurrentBase,
        EmpireAvailableBases,
        EmpireDestroyedBases,
        RebelHand,
        RebelDeck,
        RebelDiscard,
        RebelUnitInPlay,
        RebelShipInPlay,
        RebelCurrentBase,
        RebelAvailableBases,
        RebelDestroyedBases,
        GalaxyDeck,
        GalaxyDiscard,
        GalaxyRow,
        Exile,
        OuterRimPilots
    }

    public static class CardLocationHelper 
    {
        public static CardLocation GetDeck(Faction faction)
        {
            return faction == Faction.empire ? CardLocation.EmpireDeck : CardLocation.RebelDeck;
        }

        public static CardLocation GetDiscard(Faction faction) 
        {
            return faction == Faction.empire ? CardLocation.EmpireDiscard : CardLocation.RebelDiscard;
        }

        public static CardLocation GetHand(Faction faction) 
        {
            return faction == Faction.empire ? CardLocation.EmpireHand : CardLocation.RebelHand;
        }

        public static CardLocation GetUnitsInPlay(Faction faction) 
        {
            return faction == Faction.empire ? CardLocation.EmpireUnitInPlay : CardLocation.RebelUnitInPlay;
        }

        public static CardLocation GetShipsInPlay(Faction faction) 
        {
            return faction == Faction.empire ? CardLocation.EmpireShipInPlay : CardLocation.RebelShipInPlay;
        }

        public static CardLocation GetCurrentBase(Faction faction) 
        {
            return faction == Faction.empire ? CardLocation.EmpireCurrentBase : CardLocation.RebelCurrentBase;
        }

        public static CardLocation GetAvailableBases(Faction faction) 
        {
            return faction == Faction.empire ? CardLocation.EmpireAvailableBases : CardLocation.RebelAvailableBases;
        }

        public static CardLocation GetDestroyedBases(Faction faction) 
        {
            return faction == Faction.empire ? CardLocation.EmpireDestroyedBases : CardLocation.RebelDestroyedBases;
        }
    }
}