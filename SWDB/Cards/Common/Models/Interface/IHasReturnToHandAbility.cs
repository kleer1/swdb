using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWDB.Cards.Common.Models.Interface
{
    public interface IHasReturnToHandAbility : IHasAbility
    {
        bool IsValidTarget(PlayableCard card);
    }
}