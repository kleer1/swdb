using System.Reflection;
using SWDB.Game.Common;

namespace Game.Cards
{
    class CardMappingAttr : Attribute
    {
        public int MinRange { get; private set; } // inclusive
        public int MaxRange { get; private set; } // inclusive

        internal CardMappingAttr(int minRange, int maxRange)
        {
            MinRange = minRange;
            MaxRange = maxRange;
        }
    }

    public static class CardMappings
    {
        public static CardMapping EmpirePlayableCards { get; private set; }

        public static int MinRange(this CardMapping cm)
        {
            CardMappingAttr attr = GetAttr(cm);
            return attr.MinRange;
        }

        public static int MaxRange(this CardMapping cm)
        {
            CardMappingAttr attr = GetAttr(cm);
            return attr.MaxRange;
        }

        public static CardMapping GetPlayableCards(Faction faction) 
        {
            switch (faction) 
            {
                case Faction.rebellion:
                    return CardMapping.RebelPlayableCards;
                case Faction.empire:
                    return CardMapping.EmpirePlayableCards;
                case Faction.neutral:
                    return CardMapping.NeutralPlayableCards;
                default:
                    throw new ArgumentException("Invalid faction");
            }
        }

        public static CardMapping GetShipCards(Faction faction) 
        {
            switch (faction) 
            {
                case Faction.rebellion:
                    return CardMapping.RebelShips;
                case Faction.empire:
                    return CardMapping.EmpireShips;
                case Faction.neutral:
                    return CardMapping.NeutralShips;
                default:
                    throw new ArgumentException("Invalid faction");
            }
        }

        public static CardMapping GetBases(Faction faction) 
        {
            switch (faction) 
            {
                case Faction.rebellion:
                    return CardMapping.RebelBases;
                case Faction.empire:
                    return CardMapping.EmpireBases;
                default:
                    throw new ArgumentException("Invalid faction");
            }
        }

        public static CardMapping GetStartingCards(Faction faction) 
        {
            switch (faction) 
            {
                case Faction.rebellion:
                    return CardMapping.RebelStartingCards;
                case Faction.empire:
                    return CardMapping.EmpireStartingCards;
                default:
                    throw new ArgumentException("Invalid faction");
            }
        }

        public static CardMapping GetGalaxyCards(Faction faction) 
        {
            switch (faction) 
            {
                case Faction.rebellion:
                    return CardMapping.RebelGalaxyCards;
                case Faction.empire:
                    return CardMapping.EmpireGalaxyCards;
                case Faction.neutral:
                    return CardMapping.NeutralGalaxyCards;
                default:
                    throw new ArgumentException("Invalid faction");
            }
        }

        private static CardMappingAttr GetAttr(CardMapping a)
        {

            return (CardMappingAttr)(Attribute.GetCustomAttribute(ForValue(a), typeof(CardMappingAttr)) ?? throw new Exception("Could not get CardMapping Attribule"));
        }

        private static MemberInfo ForValue(CardMapping a)
        {
            string name = Enum.GetName(typeof(CardMapping), a) ?? throw new Exception("Could not get Enum name");
            return typeof(CardMapping).GetField(name) ?? throw new Exception("Could not get CardMapping Value");
        }
    }

    public enum CardMapping
    {
        [CardMappingAttr(0, 9)]EmpireStartingCards,
        [CardMappingAttr(10, 39)]EmpireGalaxyCards,
        [CardMappingAttr(0, 39)]EmpirePlayableCards,
        [CardMappingAttr(0, 32)]EmpireUnits,
        [CardMappingAttr(33, 39)]EmpireShips,
        [CardMappingAttr(40, 49)]RebelStartingCards,
        [CardMappingAttr(50, 79)]RebelGalaxyCards,
        [CardMappingAttr(40, 79)]RebelPlayableCards,
        [CardMappingAttr(40, 73)]RebelUnits,
        [CardMappingAttr(74, 79)]RebelShips,
        [CardMappingAttr(80, 89)]NeutralOuterRimCards,
        [CardMappingAttr(90, 119)]NeutralGalaxyCards,
        [CardMappingAttr(80, 119)]NeutralPlayableCards,
        [CardMappingAttr(80, 112)]NeutralUnits,
        [CardMappingAttr(113, 119)]NeutralShips,
        [CardMappingAttr(120, 129)]EmpireBases,
        [CardMappingAttr(130, 139)]RebelBases
    }
}