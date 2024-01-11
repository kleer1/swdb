using Game.Cards.Common.Models.Interface;
using SWDB.Game.Common;

namespace Game.Common.Interfaces
{
    public interface IBasePlayer
    {
        Faction Faction { get; }
        int Resources { get; }
        int GetAvailableAttack();
        bool IsForceWithPlayer();
        bool DoesPlayerHaveFullForce();
    }
}