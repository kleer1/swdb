using SWDB.Game;
using SWDB.Game.Actions;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Cards.Empire.Bases;
using SWDB.Game.Common;
using Action = SWDB.Game.Actions.Action;
using static SWDB.Game.Actions.Action;

namespace Game.Utils
{
    internal static class ValidActionUtil
    {
        internal static bool IsValidAction(Action action, Card? card, SWDBGame game, Stats? stats, ResourceOrRepair? resourceOrRepair) 
        {
            if (game.PendingActions.Any()) 
            {
                Action pendingAction = game.PendingActions.First().Action;
                bool actionMatchesPending = action == pendingAction;
                bool pendingActionIsDeclinable = pendingAction.IsDeclinable() && action == Action.DeclineAction;
                bool confirmingAttackers = pendingAction == Action.SelectAttacker && action == Action.ConfirmAttackers;
                if (!actionMatchesPending && ! pendingActionIsDeclinable && ! confirmingAttackers) 
                {
                    return false;
                }
            }
            switch (action) 
            {
                case PlayCard:
                    return card != null && CanPlayCard(card, game.GetCurrentPlayer());
                case PurchaseCard:
                    return card != null && CanPurchaseCard(card, game.GetCurrentPlayer(), game.StaticEffects, game.CurrentPlayersAction);
                case UseCardAbility:
                    return card != null && CanUseCardAbility(card, game.GetCurrentPlayer());
                case AttackCenterRow:
                    // temporarily set target for cards that care about it
                    return card != null && CanAttackCardInCenter(card, game.GetCurrentPlayer(), game.StaticEffects);
                case AttackBase:
                    return game.GetCurrentPlayer().GetAvailableAttack() > 0 &&
                        (game.GetCurrentPlayer().Opponent?.CurrentBase != null ||
                            (game.GetCurrentPlayer().Opponent?.ShipsInPlay.Any() ?? false));
                case SelectAttacker:
                    if (card is not PlayableCard playableCard)
                    {
                        return false;
                    }

                    if (!playableCard.AbleToAttack()) 
                    {
                        return false;
                    }

                    return card.Location == CardLocationHelper.GetUnitsInPlay(game.GetCurrentPlayer().Faction) ||
                        card.Location == CardLocationHelper.GetShipsInPlay(game.GetCurrentPlayer().Faction);
                case DiscardFromHand:
                case DurosDiscard:
                case BWingDiscard:
                    return card != null && CanDiscardFromHand(card, game.GetCurrentPlayer(), game.PendingActions);
                case DiscardCardFromCenter:
                    return card != null && CanDiscardFromCenter(card, game.PendingActions);
                case ExileCard:
                    return card != null && CanExileCard(card, game.GetCurrentPlayer(), game.PendingActions);
                case ReturnCardToHand:
                    return card != null && CanReturnCardToHand(card, game.GetCurrentPlayer(), game.PendingActions, game.LastCardActivated);
                case ChooseNextBase:
                    return card != null && CanChooseNewBase(card, game.GetCurrentPlayer(), game.PendingActions);
                case SwapTopCardOfDeck:
                    return card != null && CanSwapTopCardOfDeck(card, game.PendingActions, game.GalaxyDeck);
                case FireWhenReady:
                    return card != null && CanFireWhenReady(card, game.GetCurrentPlayer());
                case GalacticRule:
                    return card != null && CanGalacticRule(card, game.GetCurrentPlayer(), game.GalaxyDeck);
                case ANewHope1:
                    return card != null && game.PendingActions.Any() && game.PendingActions.First().Action == ANewHope1 &&
                        card.Location == CardLocation.GalaxyDiscard && card is PlayableCard &&
                        card.Faction == Faction.rebellion;
                case ANewHope2:
                    return card != null && game.PendingActions.Any() && game.PendingActions.First().Action == ANewHope2 &&
                        game.ANewHope1Card != null && card.Location == CardLocation.GalaxyRow && card is PlayableCard;
                case JynErsoTopDeck:
                    return card != null && game.PendingActions.Any() && game.PendingActions.First().Action == JynErsoTopDeck &&
                        game.CurrentPlayersAction == Faction.rebellion && card.Location == CardLocation.EmpireHand && card is PlayableCard;
                case LukeDestroyShip:
                    return card != null && game.PendingActions.Any() && game.PendingActions.First().Action == Action.LukeDestroyShip &&
                        game.CurrentPlayersAction == Faction.rebellion && card.Location == CardLocation.EmpireShipInPlay && card is CapitalShip;
                case HammerHeadAway:
                    return card != null && CanHammerHeadAway(card, game.GetCurrentPlayer(), game.PendingActions);
                case JabbaExile:
                    return card != null && CanExileCardFromHand(card, game.GetCurrentPlayer(), game.PendingActions);
                case PassTurn:
                    return game.GetCurrentPlayer().Faction == game.CurrentPlayersAction;
                case DeclineAction:
                    return game.PendingActions.Any() && game.PendingActions.First().Action.IsDeclinable();
                case ChooseStatBoost:
                    return game.PendingActions.Any() && game.PendingActions.First().Action == Action.ChooseStatBoost && stats != null;
                case ChooseResourceOrRepair:
                    return CanChooseResourceOrRepair(resourceOrRepair, game.GetCurrentPlayer(), game.PendingActions);
                case AttackNeutralCard:
                    if (game.PendingActions.Any() || !game.StaticEffects.Contains(StaticEffect.CanBountyOneNeutral)) 
                    {
                        return false;
                    }
                    return game.GalaxyRow.Where(pc => pc is Unit && pc.Faction == Faction.neutral &&
                        pc.Cost <= game.GetCurrentPlayer().GetAvailableAttack()).Any();
                case ConfirmAttackers :
                    return CanConfirmAttackers(game.PendingActions, game.AttackTarget, game.Attackers);

            }
            return false;
        }

        private static readonly ISet<Action> DiscardActions = new HashSet<Action>{ Action.BWingDiscard, Action.DiscardFromHand, Action.DurosDiscard };

        private static bool CanPlayCard(Card card, Player player) 
        {
            return card.Location == CardLocationHelper.GetHand(player.Faction);
        }

        private static bool CanPurchaseCard(Card card, Player player, IList<StaticEffect> staticEffects, Faction currentFaction) 
        {
            if (card.Faction != player.Faction && card.Faction != Faction.neutral) 
            {
                return false;
            }

            if (staticEffects.Contains(StaticEffect.NextFactionPurchaseIsFree) && card.Faction != player.Faction) 
            {
                return false;
            }

            if (staticEffects.Contains(StaticEffect.NextFactionOrNeutralPurchaseIsFree) &&
                    card.Faction != currentFaction && card.Faction != Faction.neutral) 
            {
                return false;
            }

            if (card is not PlayableCard)
            {
                return false;
            }

            if (!staticEffects.Contains(StaticEffect.NextFactionPurchaseIsFree) &&
                    !staticEffects.Contains(StaticEffect.NextFactionOrNeutralPurchaseIsFree)
                    && player.Resources < ((PlayableCard) card).Cost) 
            {
                return false;
            }

            if (staticEffects.Contains(StaticEffect.PurchaseFromDiscard)) 
            {
                return card.Location == CardLocation.GalaxyDiscard;
            }

            return card.Location == CardLocation.GalaxyRow || card.Location == CardLocation.OuterRimPilots;
        }

        private static bool CanUseCardAbility(Card card, Player player) 
        {
            if (card.Location != CardLocationHelper.GetShipsInPlay(player.Faction) &&
                    card.Location != CardLocationHelper.GetUnitsInPlay(player.Faction) &&
                    card.Location != CardLocationHelper.GetCurrentBase(player.Faction)) 
            {
                return false;
            }
            if (card is not IHasAbility)
            {
                return false;
            }
            return ((IHasAbility) card).AbilityActive();
        }

        private static bool CanAttackCardInCenter(Card card, Player player, IList<StaticEffect> staticEffects) 
        {
            if (card.Location != CardLocation.GalaxyRow) 
            {
                return false;
            }

            if (player.Faction == card.Faction) 
            {
                return false;
            }

            if (card is ITargetable targetable && targetable.GetTargetValue() > player.GetAvailableAttack()) 
            {
                return false;
            }

            if (card.Faction == Faction.neutral && ((PlayableCard) card).Cost > player.GetAvailableAttack()) 
            {
                return false;
            }
            
            return card is ITargetable || (staticEffects.Contains(StaticEffect.CanBountyOneNeutral) && card.Faction == Faction.neutral);
        }

        private static bool CanDiscardFromHand(Card card, Player player, IList<PendingAction> pendingActions) 
        {
            return pendingActions.Any() && DiscardActions.Contains(pendingActions.First().Action) &&
                card.Location == CardLocationHelper.GetHand(player.Faction);
        }

        private static bool CanDiscardFromCenter(Card card, IList<PendingAction> pendingActions) 
        {
            return pendingActions.Any() && pendingActions.First().Action == Action.DiscardCardFromCenter &&
                card.Location == CardLocation.GalaxyRow;
        }

        private static bool CanExileCard(Card card, Player player, IList<PendingAction> pendingActions) 
        {
            return CanExileCard(card, new HashSet<CardLocation> {
                CardLocationHelper.GetHand(player.Faction), 
                CardLocationHelper.GetDiscard(player.Faction)
            }, pendingActions);
        }

        private static bool CanExileCardFromHand(Card card, Player player, IList<PendingAction> pendingActions) 
        {
            return CanExileCard(card, new HashSet<CardLocation>{ CardLocationHelper.GetHand(player.Faction) }, pendingActions);
        }

        private static bool CanExileCard(Card card, ISet<CardLocation> cardLocations, IList<PendingAction> pendingActions) 
        {
            if (!pendingActions.Any() || (pendingActions.First().Action != Action.ExileCard) &&
                pendingActions.First().Action != Action.JabbaExile) 
            {
                return false;
            }

            if (card is not PlayableCard)
            {
                return false;
            }

            return cardLocations.Contains(card.Location);
        }

        private static bool CanChooseNewBase(Card card, Player player, IList<PendingAction> pendingActions) 
        {
            return pendingActions.Any() && pendingActions.First().Action == Action.ChooseNextBase && card is Base &&
                player.CurrentBase == null && card.Location == CardLocationHelper.GetAvailableBases(player.Faction);
        }

        private static bool CanChooseResourceOrRepair(ResourceOrRepair? choice, Player player, IList<PendingAction> pendingActions) 
        {
            if (!pendingActions.Any() || pendingActions.First().Action != Action.ChooseResourceOrRepair) 
            {
                return false;
            }

            if (choice == null) {
                return false;
            }

            return choice != ResourceOrRepair.Repair || player.CurrentBase?.CurrentDamage != 0;
        }

        private static bool CanReturnCardToHand(Card card, Player player, IList<PendingAction> pendingActions, Card? lastCardActivated) 
        {
            if (!pendingActions.Any() || pendingActions.First().Action != Action.ReturnCardToHand) 
            {
                return false;
            }

            if (card.Location != CardLocationHelper.GetDiscard(player.Faction)) 
            {
                return false;
            }

            if (lastCardActivated  is not IHasReturnToHandAbility)
            {
                return false;
            }

            if (card is not PlayableCard)
            {
                return false;
            }

            return ((IHasReturnToHandAbility) lastCardActivated).IsValidTarget((PlayableCard) card);
        }

        private static bool CanSwapTopCardOfDeck(Card card, IList<PendingAction> pendingActions, IList<PlayableCard> galaxyDeck) 
        {
            if (!pendingActions.Any() || pendingActions.First().Action != Action.SwapTopCardOfDeck) 
            {
                return false;
            }

            if (card.Location != CardLocation.GalaxyRow) 
            {
                return false;
            }

            return galaxyDeck.Any();
        }

        private static bool CanFireWhenReady(Card card, Player player) 
        {
            if (player.Faction != Faction.empire || player.CurrentBase is not DeathStar || player.Resources < 4) 
            {
                return false;
            }

            if (card is not CapitalShip) 
            {
                return false;
            }

            return card.Location == CardLocation.GalaxyRow || card.Location == CardLocationHelper.GetShipsInPlay(Faction.rebellion);
        }

        private static bool CanGalacticRule(Card card, Player player, IList<PlayableCard> galaxyDeck) 
        {
            if (player.Faction != Faction.empire || player.CurrentBase is not Coruscant)
            {
                return false;
            }

            if (galaxyDeck.Count < 2) 
            {
                return false;
            }

            if (card is not PlayableCard)
            {
                return false;
            }

            PlayableCard playableCard = (PlayableCard) card;
            return  playableCard.Location == CardLocation.GalaxyDeck &&
                (playableCard.Equals(galaxyDeck.First()) || playableCard.Equals(galaxyDeck.ElementAt(1)));
        }

        private static bool CanHammerHeadAway(Card card, Player player, IList<PendingAction> pendingActions) 
        {
            if (!pendingActions.Any() || pendingActions.First().Action != Action.HammerHeadAway) 
            {
                return false;
            }

            if (player.Faction != Faction.rebellion || card.Faction != Faction.empire || card is not CapitalShip)
            {
            
                return false;
            }

            return card.Location == CardLocation.GalaxyRow || card.Location == CardLocation.EmpireShipInPlay;
        }

        private static bool CanConfirmAttackers(IList<PendingAction> pendingActions, Card? attackTarget, IList<PlayableCard> attackers) 
        {
            if (!pendingActions.Any() || pendingActions.First().Action != Action.SelectAttacker) 
            {
                return false;
            }

            if (attackTarget == null || !attackers.Any()) 
            {
                return false;
            }

            int totalAttack = attackers.Sum(pc => pc.Attack);
            if (attackTarget is PlayableCard playableCard) 
            {

                if (attackTarget is ITargetable  targetable) 
                {
                    return totalAttack >= targetable.GetTargetValue();
                }

                if (attackTarget.Faction == Faction.neutral) 
                {
                    return totalAttack >= playableCard.Cost;
                }
            }

            return true;
        }
    }
}