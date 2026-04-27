using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Scripts.Run;

namespace TestProject.Scripts.Cards.Effects
{
    public class HalveNeighborDamage : IEffect
    {
        private EMetrics _metricsApplied;
        private EAdjacentType _adjacentType;

        public HalveNeighborDamage(EMetrics metricsApplied, EAdjacentType adjacentType)
        {
            _metricsApplied = metricsApplied;
            _adjacentType = adjacentType;
        }

        public void Apply(Card[] dock, int index, TermContext pending)
        {
            if (dock.Length <= 0) return;

            if (_adjacentType == EAdjacentType.Left || _adjacentType == EAdjacentType.Both)
                HalvesDamage(_metricsApplied, pending, index - 1);

            if (_adjacentType == EAdjacentType.Right || _adjacentType == EAdjacentType.Both)
                HalvesDamage(_metricsApplied, pending, index + 1);
        }

        private void HalvesDamage(EMetrics metric, TermContext pending, int targetIdx)
        {
            int x = 0; 
            var slot = pending.Docks[targetIdx].Card;

            if (slot == null) return;

            switch (metric)
            {
                case EMetrics.BiosphereChange:
                    x = slot.EffectToNature;
                    break;
                case EMetrics.HighClassApprovalChange:
                    x = slot.EffectToHighClass;
                    break;
                case EMetrics.LowClassApprovalChange:
                    x = slot.EffectToLowClass;
                    break;
            }

            StaticEffectHandler.GeometricMultiplier(metric, 0.5f, pending, targetIdx, () => x < 0);
        }
    }
}
