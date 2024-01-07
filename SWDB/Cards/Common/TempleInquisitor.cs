using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Common;
using SWDB.Game;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Cards.Common
{
    public class TempleInquisitor : Unit, IHasOnPlayAction, IHasChooseStats
    {
        public Stats? Choice { get; protected set; } = null;

        public TempleInquisitor(int id, Faction faction, string title, IList<Trait> traits, Player owner,
            CardLocation location, IList<Card> cardList, SWDBGame game) :
            base(id, 0, 0, 0, 0, faction, title, traits, false, owner, location, cardList, game) {}
        
        public IList<PendingAction> GetActions() 
        {
            return new List<PendingAction>(){ PendingAction.Of(Action.ChooseStatBoost) };
        }

        public void ApplyChoice(Stats statChoice) 
        {
            Choice = statChoice;
            if (Choice == Stats.Resources) 
            {
                Owner?.AddResources(1);

            } else if (Choice == Stats.Force) 
            {
                Owner?.AddForce(1);
            }
        }

        public override void MoveToDiscard()
        {
            base.MoveToDiscard();
            Choice = null;
        }

        public override int Attack 
        {
            get
            {
                if (Choice != null && Stats.Attack == Choice) return base.Attack + 1;
                return base.Attack;
            }
        }

        public override int Resources 
        { 
            get 
            {
                if (Choice != null && Stats.Resources == Choice) return base.Resources + 1;
                return base.Resources;
            }
        }

        public override int Force
        {
            get
            {
                if (Choice != null && Stats.Force == Choice) return base.Force + 1;
                return base.Force;
            }
        }
    }
}