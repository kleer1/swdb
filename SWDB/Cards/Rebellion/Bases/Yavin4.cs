using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Common;
using SWDB.Game;

namespace SWDB.Cards.Rebellion.Bases
{
    public class Yavin4 : Base, IHasAtStartOfTurn, IHasOnReveal
    {
        public Yavin4(int id, SWDBGame game) :
            base(id, Faction.rebellion, "Yavin 4", CardLocation.RebelAvailableBases, (IList<Card>) game.Rebel.AvailableBases,
                game, game.Rebel, 16) {}

        public void ApplyAtStartOfTurn() 
        {
            Game.StaticEffects.Add(StaticEffect.Yavin4Effect);
        }

        public void ApplyOnReveal() 
        {
            Game.StaticEffects.Add(StaticEffect.Yavin4Effect);
        }
    }
}