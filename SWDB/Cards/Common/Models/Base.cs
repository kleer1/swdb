using SWDB.Common;
using SWDB.Game;

namespace SWDB.Cards.Common.Models
{
    public class Base : Card
    {
        public int HitPoints { get; protected set; }
        public int CurrentDamage { get; set; }

        protected Base(int id, Faction faction, string title, CardLocation location,
                IList<Card>? cardList, SWDBGame game, Player owner, int hitPoints) :
                 base(id, faction, title, true, location, cardList, game, owner) 
        {
            HitPoints = hitPoints;
        }

        public int HetRemainingHealth() 
        {
            return HitPoints - CurrentDamage;
        }

        public virtual void AddDamage(int damage) 
        {
            CurrentDamage += damage;
            if (CurrentDamage >= HitPoints) 
            {
            DestroyBase();
            } else if (CurrentDamage < 0) 
            {
                CurrentDamage = 0;
            }
        }

        protected void DestroyBase() 
        {
            if (Owner == null)
            {
                throw new ArgumentException("Could not destroy base with no owner.");
            }
            if (Owner.Opponent == null)
            {
                throw new ArgumentException("Could not destroy base with no opponent.");
            }
            Owner.CurrentBase = null;
            Owner.Opponent.DestroyedBases.Add(this);
            Location = CardLocationHelper.GetDestroyedBases(Owner.Opponent.Faction);
            CardList = (IList<Card>?) Owner.Opponent.DestroyedBases;
            Owner = Owner.Opponent;
        }

        public void MakeCurrentBase() 
        {
            if (Owner == null) 
            {
                throw new ArgumentException("Setting new current base while there is no owner");
            }
            if (Owner.CurrentBase != null) 
            {
                throw new ArgumentException("Setting new current base while there is still a current base");
            }
            if (Location != CardLocationHelper.GetAvailableBases(Owner.Faction)) 
            {
                throw new ArgumentException("Setting new current base for an unavailable base");
            }
            Owner.CurrentBase = this;
            CardList?.Remove(this);
            Location = CardLocationHelper.GetCurrentBase(Owner.Faction);
        }

        public override bool AbilityActive() 
        {
            if (Owner == null) 
            {
                return false;
            }
            return base.AbilityActive() && Location == CardLocationHelper.GetCurrentBase(Owner.Faction);
        }

        public override string ToString() 
        {
            return "Base{" +
                    "id=" + Id +
                    ", title='" + Title + '\'' +
                    ", hitPoints=" + HitPoints +
                    ", currentDamage=" + CurrentDamage +
                    '}';
        }
    }
}