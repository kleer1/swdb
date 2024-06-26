using SWDB.Game;
using SWDB.Game.Actions;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Cards.Empire.Bases;
using SWDB.Game.Common;
using Action = SWDB.Game.Actions.Action;
using static SWDB.Game.Actions.Action;
using Game.Cards.Common.Models.Interface;
using Game.Common.Interfaces;
using Game.Actions;
using Game.Actions.Interfaces;

namespace Game.Utils
{
    internal static class ValidActionUtil
    {
        internal static IList<GameAction> GetValidGameActions(SWDBGame game)
        {
            IList<GameAction> validActions = new List<GameAction>();
            foreach (ICard card in game.CardMap.Values)
            {
                CardLocation location = card.Location;
                foreach (Action action in GameActions.GetDefaultActionsByLocation(location))
                {
                    var gameAction1 = new GameAction(action, card.Id);
                    if (IsValidAction(game, gameAction1, card: card))
                    {
                        validActions.Add(gameAction1);
                    }
                }
                foreach (Action action in GameActions.GetPendingActionsByLocation(location))
                {
                    GameAction gameAction2 = new GameAction(action, card.Id);
                    if (IsValidAction(game, gameAction2, card: card))
                    {
                        validActions.Add(gameAction2);
                    }
                }
            }

            foreach (Action action in GameActions.NoChoiceOrCardActions)
            {
                var gameAction3 = new GameAction(action);
                if (IsValidAction(game, gameAction3))
                {
                    validActions.Add(new GameAction(action));
                }
            }
            var gameAction = new GameAction(ChooseStatBoost, stats: Stats.Attack);
            if (IsValidAction(game, gameAction)) validActions.Add(gameAction);
            gameAction = new GameAction(ChooseStatBoost, stats: Stats.Resources);
            if (IsValidAction(game, gameAction)) validActions.Add(gameAction);
            gameAction = new GameAction(ChooseStatBoost, stats: Stats.Force);
            if (IsValidAction(game, gameAction)) validActions.Add(gameAction);
            gameAction = new GameAction(ChooseStatBoost, resourceOrRepair: ResourceOrRepair.Repair);
            if (IsValidAction(game, gameAction)) validActions.Add(gameAction);
            gameAction = new GameAction(ChooseStatBoost, resourceOrRepair: ResourceOrRepair.Resources);
            if (IsValidAction(game, gameAction)) validActions.Add(gameAction);
            
            return validActions;
        }
        internal static bool IsValidAction(SWDBGame game, IGameAction gameAction, ICard? card = null) 
        {
            if (game.PendingActions.Any()) 
            {
                Action pendingAction = game.PendingActions.First().Action;
                bool actionMatchesPending = gameAction.Action == pendingAction;
                bool pendingActionIsDeclinable = pendingAction.IsDeclinable() && gameAction.Action == Action.DeclineAction;
                bool confirmingAttackers = pendingAction == Action.SelectAttacker && gameAction.Action == Action.ConfirmAttackers;
                if (!actionMatchesPending && ! pendingActionIsDeclinable && ! confirmingAttackers) 
                {
                    return false;
                }
            }
            switch (gameAction.Action) 
            {
                case PlayCard:
                    return card != null && CanPlayCard(card, game.GetCurrentPlayer());
                case PurchaseCard:
                    return card != null && CanPurchaseCard(card, game.GetCurrentPlayer(), game.StaticEffects, game.CurrentPlayersAction);
                case UseCardAbility:
                    return card != null && CanUseCardAbility(card, game.GetCurrentPlayer());
                case AttackCenterRow:
                    // temporarily set target for cards that care about it
                    var temp = game.AttackTarget;
                    game.AttackTarget = card;
                    var validAttack = card != null && CanAttackCardInCenter(card, game.GetCurrentPlayer(), game.StaticEffects);
                    game.AttackTarget = temp;
                    return validAttack;
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
                    return card != null && CanDiscardFromHand(card, game.CurrentPlayersAction, game.PendingActions);
                case DiscardCardFromCenter:
                    return card != null && CanDiscardFromCenter(card, game.PendingActions);
                case ExileCard:
                    return card != null && CanExileCard(card, game.GetCurrentPlayer(), game.PendingActions);
                case ReturnCardToHand:
                    return card != null && CanReturnCardToHand(card, game.GetCurrentPlayer(), game.PendingActions, game.LastCardActivated);
                case ChooseNextBase:
                    return card != null && CanChooseNewBase(card, game.GetCurrentPlayer(), game.PendingActions);
                case SwapTopCardOfDeck:
                    return card != null && CanSwapTopCardOfDeck(card, game.PendingActions, game.GalaxyDeck.BaseList);
                case FireWhenReady:
                    return card != null && CanFireWhenReady(card, game.GetCurrentPlayer());
                case GalacticRule:
                    return card != null && CanGalacticRule(card, game.GetCurrentPlayer(), game.GalaxyDeck.BaseList);
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
                    return game.PendingActions.Any() && game.PendingActions.First().Action == Action.ChooseStatBoost && gameAction.Stats != null;
                case ChooseResourceOrRepair:
                    return CanChooseResourceOrRepair(gameAction.ResourceOrRepair, game.GetCurrentPlayer(), game.PendingActions);
                case AttackNeutralCard:
                    if (game.PendingActions.Any() || !game.StaticEffects.Contains(StaticEffect.CanBountyOneNeutral)) 
                    {
                        return false;
                    }
                    return game.GalaxyRow.BaseList.Where(pc => pc is Unit && pc.Faction == Faction.neutral &&
                        pc.Cost <= game.GetCurrentPlayer().GetAvailableAttack()).Any();
                case ConfirmAttackers :
                    return CanConfirmAttackers(game.PendingActions, game.AttackTarget, game.Attackers);

            }
            return false;
        }

        private static readonly ISet<Action> DiscardActions = new HashSet<Action>{ Action.BWingDiscard, Action.DiscardFromHand, Action.DurosDiscard };

        private static bool CanPlayCard(ICard card, IPlayer player) 
        {
            return card.Location == CardLocationHelper.GetHand(player.Faction);
        }

        private static bool CanPurchaseCard(ICard card, IPlayer player, IList<StaticEffect> staticEffects, Faction currentFaction) 
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

        private static bool CanUseCardAbility(ICard card, IPlayer player) 
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

        private static bool CanAttackCardInCenter(ICard card, IPlayer player, IList<StaticEffect> staticEffects) 
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

            if (card.Faction == Faction.neutral && ((IPlayableCard) card).Cost > player.GetAvailableAttack()) 
            {
                return false;
            }
            
            return card is ITargetable || (staticEffects.Contains(StaticEffect.CanBountyOneNeutral) && card.Faction == Faction.neutral);
        }

        private static bool CanDiscardFromHand(ICard card, Faction faction, IList<PendingAction> pendingActions) 
        {
            return pendingActions.Any() && DiscardActions.Contains(pendingActions.First().Action) &&
                card.Location == CardLocationHelper.GetHand(faction);
        }

        private static bool CanDiscardFromCenter(ICard card, IList<PendingAction> pendingActions) 
        {
            return pendingActions.Any() && pendingActions.First().Action == Action.DiscardCardFromCenter &&
                card.Location == CardLocation.GalaxyRow;
        }

        private static bool CanExileCard(ICard card, IPlayer player, IList<PendingAction> pendingActions) 
        {
            return CanExileCard(card, new HashSet<CardLocation> {
                CardLocationHelper.GetHand(player.Faction), 
                CardLocationHelper.GetDiscard(player.Faction)
            }, pendingActions);
        }

        private static bool CanExileCardFromHand(ICard card, IPlayer player, IList<PendingAction> pendingActions) 
        {
            return CanExileCard(card, new HashSet<CardLocation>{ CardLocationHelper.GetHand(player.Faction) }, pendingActions);
        }

        private static bool CanExileCard(ICard card, ISet<CardLocation> cardLocations, IList<PendingAction> pendingActions) 
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

        private static bool CanChooseNewBase(ICard card, IPlayer player, IList<PendingAction> pendingActions) 
        {
            return pendingActions.Any() && pendingActions.First().Action == Action.ChooseNextBase && card is Base &&
                player.CurrentBase == null && card.Location == CardLocationHelper.GetAvailableBases(player.Faction);
        }

        private static bool CanChooseResourceOrRepair(ResourceOrRepair? choice, IPlayer player, IList<PendingAction> pendingActions) 
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

        private static bool CanReturnCardToHand(ICard card, IPlayer player, IList<PendingAction> pendingActions, ICard? lastCardActivated) 
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

        private static bool CanSwapTopCardOfDeck(ICard card, IList<PendingAction> pendingActions, IList<IPlayableCard> galaxyDeck) 
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

        private static bool CanFireWhenReady(ICard card, IPlayer player) 
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

        private static bool CanGalacticRule(ICard card, IPlayer player, IList<IPlayableCard> galaxyDeck) 
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

        private static bool CanHammerHeadAway(ICard card, IPlayer player, IList<PendingAction> pendingActions) 
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

        private static bool CanConfirmAttackers(IList<PendingAction> pendingActions, ICard? attackTarget, IList<IPlayableCard> attackers) 
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