using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Scripts.Cards.Effects.EffectMisc;
using TestProject.Scripts.Run;

namespace TestProject.Scripts.Cards.Effects
{
    public class PollutionEffect : IEffect
    {
        public void Apply(Card[] dock, int index, TermContext pending)
        {
            for (int i = index + 1; i < dock.Length; i++)
            {
                var slot = pending.Docks[i];
                pending.Docks[i].EffectToNatureFlatBonus = Math.Min(slot.EffectToNatureFlatBonus, 0);
            }
        }
    }
}
