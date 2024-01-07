using SWDB.Common;
using SWDB.Game;

namespace SWDB.Cards.Common.Models
{
    public class Unit : PlayableCard
    {
        protected Unit(int id, int cost, int attack, int resources, int force, Faction faction, string title, IList<Trait> traits,
                bool isUnique, Player? owner, CardLocation location, IList<Card> cardList, SWDBGame game) :
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
            CardList = (IList<Card>?) Owner.UnitsInPlay;
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