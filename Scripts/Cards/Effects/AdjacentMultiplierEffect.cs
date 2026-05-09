using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Scripts.Cards.Effects.EffectHandler;
using TestProject.Scripts.Cards.Effects.EffectMisc;
using TestProject.Scripts.Games;
using TestProject.Scripts.Run;
using TestProject.Scripts.Run.Misc;

namespace TestProject.Scripts.Cards.Effects
{
    public class AdjacentMultiplierEffect : IEffect
    {
        private readonly float _multiplier;
        private readonly EMetrics _metricsApplied = EMetrics.Default;
        private readonly EAdjacentType _adjacentType = EAdjacentType.Both;

        public AdjacentMultiplierEffect(float multiplier, EAdjacentType adjacentType)
        {
            _multiplier = multiplier;
            _adjacentType = adjacentType;
        }

        public void Apply(Card[] dock, int index, TermContext pending)
        {
            if (dock.Length <= 0) return;

            if (_adjacentType == EAdjacentType.Left || _adjacentType == EAdjacentType.Both)
            { 
                AddMultiplier(_metricsApplied, _multiplier, pending, index - 1);
            }

            if (_adjacentType == EAdjacentType.Right || _adjacentType == EAdjacentType.Both)
            {
                AddMultiplier(_metricsApplied, _multiplier, pending, index + 1);
            }
        }

        private void AddMultiplier(EMetrics metric, float multiplier, TermContext pending, int targetIdx)
        {
            StaticEffectHandler.ArithmeticMultiplier(metric, multiplier, pending, targetIdx);
        }
    }
}
