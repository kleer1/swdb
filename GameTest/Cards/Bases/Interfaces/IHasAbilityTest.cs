using SWDB.Game.Cards.Common.Models;
using static NUnit.Framework.Assert;

namespace GameTest.Cards.Bases.Interfaces
{
    public interface IHasAbilityTest : IAvailableBaseCard
    {
        [Test]
        void TestUseCardAbility() 
        {;
            SetupAbility();
            UseCardAbility();
            VerifyAbility();
        }

        void UseCardAbility() 
        {
            Base.MakeCurrentBase();
            Game.ApplyAction(SWDB.Game.Actions.Action.PassTurn);
            Game.ApplyAction(SWDB.Game.Actions.Action.UseCardAbility, Base.Id);
            That(Base.AbilityActive(), Is.False);
        }

        void SetupAbility() 
        {

        }

        void VerifyAbility();
    }
}