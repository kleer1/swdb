using SWDB.Game.Common;


namespace SWDB.Game.Cards.Common.Models
{
    public class CapitalShip : PlayableCard
    {
        public int HitPoints { get; protected set; }
        public int CurrentDamage { get; set; } = 0;

        protected CapitalShip(int id, int cost, int attack, int resources, int force, Faction faction, string title, IList<Trait> traits,
                       bool isUnique, Player? owner, CardLocation location, IList<Card> cardList, SWDBGame game, int hitPoints) :
                       base(id, faction, title, isUnique, location, cardList, game, owner, cost, attack, resources, force, traits) {
    
            HitPoints = hitPoints;
        }

        public void AddDamage(int damage) 
        {
            CurrentDamage += damage;
            if (CurrentDamage >= HitPoints) 
            {
                MoveToDiscard();
            }
        }

        public override void MoveToInPlay() 
        {
            if (Owner == null || Owner.Game == null) 
            {
                throw new ArgumentException("Can not move a card into play with no owner");
            }
            base.MoveToInPlay();
            Owner.ShipsInPlay.Add(this);
            Location = Owner.Faction == Faction.empire ? CardLocation.EmpireShipInPlay : CardLocation.RebelShipInPlay;
            CardList = (IList<Card>?) Owner.ShipsInPlay;
            CurrentDamage = 0;
        }

        public override void MoveToDiscard() 
        {
            base.MoveToDiscard();
            CurrentDamage = 0;
        }

        public int GetRemainingHealth() 
        {
            return HitPoints - CurrentDamage;
        }

        public override int Attack
        {
            get
            {
                int attack =  base.Attack;
                if (Owner != null && Owner.Faction == Faction.empire && Game.StaticEffects.Contains(StaticEffect.AdmiralPiettBonus)) 
                {
                    attack++;
                }
                return attack;
            }
        }
    }
}