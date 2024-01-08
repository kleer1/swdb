namespace SWDB.Cards.Common.Models.Interface
{
    public interface IHasChooseStats : IHasOnPlayAction
    {
        void ApplyChoice(Stats stat);
    }
}