using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Scripts.Cards.Effects.EffectHandler;
using TestProject.Scripts.Cards.Effects.EffectMisc;
using TestProject.Scripts.Cards.Misc;
using TestProject.Scripts.Run;

namespace TestProject.Scripts.Cards.Effects
{
    public class SpecificIndicesFlatEffect : IEffect
    {
        private EPolicyType _policyType;
        private EMetrics _metricsApplied;
        private int[] _specificIndex;
        private int _amount;

        public SpecificIndicesFlatEffect(EMetrics metricsApplied, int[] specificIndex, int amount, EPolicyType policyType = EPolicyType.Default)
        {
            _policyType = policyType;
            _metricsApplied = metricsApplied;
            _specificIndex = specificIndex;
            _amount = amount;
        }

        public void Apply(Card[] dock, int index, TermContext pending)
        {
            foreach (int i in _specificIndex)
            {
                if (pending.Docks.Length <= i || i < 0) return;

                var slot = pending.Docks[i];
                if (slot.Card == null || slot.IsDisabled || slot.IsNullified) return;

                StaticEffectHandler.ModifyFlatBonus(_metricsApplied, _amount, pending, i, () =>
                {
                    return (slot.Card.Type == _policyType || _policyType == EPolicyType.Default);
                });
            }
        }
    }
}
