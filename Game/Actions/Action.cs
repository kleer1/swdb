using System.Reflection;
using SWDB.Game.Common;

namespace SWDB.Game.Actions
{
    class ActionAttr : Attribute
    {
        public CardLocation[] DefaultLocations { get; private set; }
        public CardLocation[] PendingLocations { get; private set; }
        public bool IsDeclinable { get; private set; }
        internal ActionAttr()
        {
            DefaultLocations = Array.Empty<CardLocation>();
            PendingLocations = Array.Empty<CardLocation>();
            IsDeclinable = true;
        }

        internal ActionAttr(CardLocation[] defaultLocations, CardLocation[] pendingLocations)
        {
            DefaultLocations = defaultLocations;
            PendingLocations = pendingLocations;
            IsDeclinable = true;
        }

        internal ActionAttr(CardLocation[] defaultLocations, CardLocation[] pendingLocations, bool isDeclinable)
        {
            DefaultLocations = defaultLocations;
            PendingLocations = pendingLocations;
            IsDeclinable = isDeclinable;
        }
    }

    public static class GameActions
    {
        private static readonly ISet<Action> Values = Enum.GetValues(typeof(Action)).Cast<Action>().ToHashSet();

        public static ISet<CardLocation> DefaultLocations(this Action a)
        {
            ActionAttr actionAttr = GetAttr(a);
            return new HashSet<CardLocation>(actionAttr.DefaultLocations);
        }

        public static ISet<CardLocation> PendingLocations(this Action a)
        {
            ActionAttr actionAttr = GetAttr(a);
            return new HashSet<CardLocation>(actionAttr.PendingLocations);
        }

        public static bool IsDeclinable(this Action a)
        {
            ActionAttr actionAttr = GetAttr(a);
            return actionAttr.IsDeclinable;
        }

        public static ISet<Action> GetDefaultActionsByLocation(CardLocation location)
        {
            return Values.Where(a => a.DefaultLocations().Contains(location)).ToHashSet();
        }

        public static ISet<Action> GetPendingActionsByLocation(CardLocation location)
        {
            return Values.Where(a => a.PendingLocations().Contains(location)).ToHashSet();
        }

        private static ActionAttr GetAttr(Action a)
        {

            return (ActionAttr)(Attribute.GetCustomAttribute(ForValue(a), typeof(ActionAttr)) ?? throw new Exception("Could not get Action Attribule"));
        }

        private static MemberInfo ForValue(Action a)
        {
            string name = Enum.GetName(typeof(Action), a) ?? throw new Exception("Could not get Enum name");
            return typeof(Action).GetField(name) ?? throw new Exception("Could not get Action Value");
        }
    }
    
    public enum Action
    {
        /*
        * Card Actions
        */
        [ActionAttr(
            new[]{CardLocation.EmpireHand, CardLocation.RebelHand}, 
            new CardLocation[0])] 
            PlayCard,
        [ActionAttr(
            new[]{CardLocation.GalaxyRow, CardLocation.OuterRimPilots}, 
            new[]{CardLocation.GalaxyRow, CardLocation.GalaxyDiscard})] 
            PurchaseCard,
        [ActionAttr(
            new[]{CardLocation.EmpireShipInPlay, CardLocation.EmpireUnitInPlay, 
                CardLocation.RebelUnitInPlay, CardLocation.RebelShipInPlay, CardLocation.EmpireCurrentBase,
                CardLocation.RebelCurrentBase}, 
            new CardLocation[0])]
            UseCardAbility,
        [ActionAttr(
            new[]{CardLocation.GalaxyRow}, 
            new[]{CardLocation.GalaxyRow},
            false)] 
            AttackCenterRow,
        [ActionAttr(
            new[]{CardLocation.EmpireCurrentBase, CardLocation.RebelCurrentBase}, 
            new CardLocation[0])]
            AttackBase,
        [ActionAttr(
            new CardLocation[0],
            new[]{CardLocation.EmpireShipInPlay, CardLocation.EmpireUnitInPlay,
                CardLocation.RebelUnitInPlay, CardLocation.RebelShipInPlay},
            false)]
            SelectAttacker,
        [ActionAttr(
            new CardLocation[0],
            new[]{CardLocation.EmpireHand, CardLocation.RebelHand},
            false)]
            DiscardFromHand,
        [ActionAttr(
            new CardLocation[0],
            new[]{CardLocation.GalaxyRow})]
            DiscardCardFromCenter,
        [ActionAttr(
            new CardLocation[0],
            new[]{CardLocation.EmpireHand, CardLocation.RebelHand, CardLocation.EmpireDiscard, CardLocation.RebelDiscard})]
            ExileCard,
        [ActionAttr(
            new CardLocation[0],
            new[]{CardLocation.EmpireDiscard, CardLocation.RebelDiscard})]
            ReturnCardToHand,
        [ActionAttr(
            new CardLocation[0],
            new[]{CardLocation.EmpireAvailableBases, CardLocation.RebelAvailableBases},
            false)]
            ChooseNextBase,
        [ActionAttr(
            new CardLocation[0],
            new[]{CardLocation.GalaxyRow})]
            SwapTopCardOfDeck,
        [ActionAttr(
            new CardLocation[0],
            new[]{CardLocation.GalaxyRow, CardLocation.RebelShipInPlay})]
            FireWhenReady,
        [ActionAttr(
            new CardLocation[0],
            new[]{CardLocation.GalaxyDeck},
            false)]
            GalacticRule,
        [ActionAttr(
            new CardLocation[0],
            new[]{CardLocation.GalaxyDiscard})]
            ANewHope1,
        [ActionAttr(
            new CardLocation[0],
            new[]{CardLocation.GalaxyRow},
            false)]
            ANewHope2,
        [ActionAttr(
            new CardLocation[0],
            new[]{CardLocation.EmpireHand, CardLocation.RebelHand})]
            DurosDiscard,
        [ActionAttr(
            new CardLocation[0],
            new[]{CardLocation.EmpireHand, CardLocation.RebelHand})]
            BWingDiscard,
        [ActionAttr(
            new CardLocation[0],
            new[]{CardLocation.EmpireHand},
            false)]
            JynErsoTopDeck,
        [ActionAttr(
            new CardLocation[0],
            new[]{CardLocation.EmpireShipInPlay},
            false)]
            LukeDestroyShip,
        [ActionAttr(
            new CardLocation[0],
            new[]{CardLocation.GalaxyRow, CardLocation.EmpireShipInPlay},
            false)]
            HammerHeadAway,
        [ActionAttr(
            new CardLocation[0],
            new[]{CardLocation.EmpireHand, CardLocation.RebelHand},
            false)]
            JabbaExile,
        /*
        * Non-card Actions
        */
        [ActionAttr()] PassTurn,
        [ActionAttr()] DeclineAction,
        [ActionAttr()] ChooseStatBoost,
        [ActionAttr()] ChooseResourceOrRepair,
        [ActionAttr()] AttackNeutralCard,
        [ActionAttr()] ConfirmAttackers
    }
}