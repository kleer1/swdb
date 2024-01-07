using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWDB.Game
{
    public class ForceBalance
    {
        private int Position { get; set; } = 6;

         public void DarkSideGainForce(int amount) {
            Position -= amount;
            if (Position < 0) Position = 0;
        }

        public void LightSideGainForce(int amount) {
            Position += amount;
            if (Position > 6) Position = 6;
        }

        public bool LightSideHasTheForce() {
            return Position > 3;
        }

        public bool DarkSideHasTheForce() {
            return Position < 3;
        }

        public bool LightSideFull() {
            return Position == 6;
        }

        public bool DarkSideFull() {
            return Position == 0;
        }
    }
}