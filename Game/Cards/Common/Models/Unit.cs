using Game.Cards.Common.Models.Interface;
using Game.Common.Interfaces;
using SWDB.Game.Common;

namespace SWDB.Game.Cards.Common.Models
{
    public class Unit : PlayableCard, IUnit
    {
        protected Unit(int id, int cost, int attack, int resources, int force, Faction faction, string title, IList<Trait> traits,
            bool isUnique, IPlayer? owner, CardLocation location, IList<ICard> cardList, SWDBGame game) :
                base(id, faction, title, isUnique, location, cardList, game, owner, cost, attack, resources, force, traits){ }
        
        public override void MoveToInPlay() 
        {
            if (Owner == null || Owner.Game == null) 
            {
                throw new ArgumentException("Can not move a card into play with no owner");
            }
            base.MoveToInPlay();
            Owner.UnitsInPlay.Add(this);
            Location = Owner.Faction == Faction.empire ? CardLocation.EmpireUnitInPlay : CardLocation.RebelUnitInPlay;
            CardList = Owner.UnitsInPlay;
        }

        public override int Attack 
        {
            get
            {
                int _attack = base.Attack;
                if (Faction == Faction.empire && Game.StaticEffects.Contains(StaticEffect.EndorBonus) &&
                        (Traits.Contains(Trait.trooper) || Traits.Contains(Trait.vehicle))) 
                {
                    _attack++;
                }
                return _attack;    
            }
        }
    }
}