using Game.Cards.Common.Models.Interface;
using SWDB.Game;
using static SWDB.Game.Utils.ListExtension;

namespace Game.Common.Interfaces
{
    public interface IPlayer : IBasePlayer
    {
        new int Resources { get; internal set; }
        CastedList<ICard, IPlayableCard> Hand { get; }
        CastedList<ICard, IPlayableCard> Deck { get; }
        CastedList<ICard, IPlayableCard> Discard { get; }
        CastedList<ICard, IBase> AvailableBases { get; }
        CastedList<ICard, IBase> DestroyedBases { get; }
        CastedList<ICard, IUnit> UnitsInPlay { get; }
        CastedList<ICard, ICapitalShip> ShipsInPlay { get; }
        IBase? CurrentBase { get; set; }
        IPlayer? Opponent { get; set; }
        SWDBGame? Game { get; internal set; }
        void AddResources(int amount);
        void DrawCards(int amount);
        void DiscardUnits();
        void DiscardHand();
        void AddForce(int amount);
    }
}