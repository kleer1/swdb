namespace SWDB.Game.Common
{
    public class ForceBalance
    {
        public int Position { get; private set; } = 6;

         public void DarkSideGainForce(int amount) 
         {
            Position -= amount;
            if (Position < 0) Position = 0;
        }

        public void LightSideGainForce(int amount) 
        {
            Position += amount;
            if (Position > 6) Position = 6;
        }

        public virtual bool LightSideHasTheForce() 
        {
            return Position > 3;
        }

        public virtual bool DarkSideHasTheForce() 
        {
            return Position < 3;
        }

        public bool LightSideFull() 
        {
            return Position == 6;
        }

        public bool DarkSideFull() 
        {
            return Position == 0;
        }
    }
}