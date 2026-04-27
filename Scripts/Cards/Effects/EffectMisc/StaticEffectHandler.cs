using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Scripts.Cards.Effects.EffectMisc;
using TestProject.Scripts.Run;

namespace TestProject.Scripts.Cards.Effects.EffectHandler
{
    public class StaticEffectHandler
    {
        public static void ArithmeticMultiplier(EMetrics metric, float multiplier, TermContext pending, int targetIdx, Func<bool>? customPermission)
        {
            if (targetIdx < 0 || targetIdx >= pending.Docks.Length) return;
            if (metric == EMetrics.Default) return;
            if (multiplier == 0) return;

            customPermission ??= () => true;

            if ((metric == EMetrics.GdpGenerated || metric == EMetrics.All) && customPermission())
                pending.Docks[targetIdx].BaseGdpMultiplier += multiplier;

            if ((metric == EMetrics.BiosphereChange || metric == EMetrics.All) && customPermission())
                pending.Docks[targetIdx].EffectToNatureMultiplier += multiplier;

            if ((metric == EMetrics.HighClassApprovalChange || metric == EMetrics.All) && customPermission())
                pending.Docks[targetIdx].EffectToHighClassMultiplier += multiplier;

            if ((metric == EMetrics.LowClassApprovalChange || metric == EMetrics.All) && customPermission())
                pending.Docks[targetIdx].EffectToLowClassMultiplier += multiplier;
        }

        public static void GeometricMultiplier(EMetrics metric, float multiplier, TermContext pending, int targetIdx, Func<bool>? customPermission = null)
        {
            if (targetIdx < 0 || targetIdx >= pending.Docks.Length) return;
            if (metric == EMetrics.Default) return;
            if (multiplier == 0) return;

            customPermission ??= () => true;

            if ((metric == EMetrics.GdpGenerated || metric == EMetrics.All) && customPermission())
                pending.Docks[targetIdx].BaseGdpMultiplier *= multiplier;

            if ((metric == EMetrics.BiosphereChange || metric == EMetrics.All) && customPermission())
                pending.Docks[targetIdx].EffectToNatureMultiplier *= multiplier;

            if ((metric == EMetrics.HighClassApprovalChange || metric == EMetrics.All) && customPermission())
                pending.Docks[targetIdx].EffectToHighClassMultiplier *= multiplier;

            if ((metric == EMetrics.LowClassApprovalChange || metric == EMetrics.All) && customPermission())
                pending.Docks[targetIdx].EffectToLowClassMultiplier *= multiplier;
        }

        public static void ModifyFlatBonus(EMetrics metric, int flatBonus, TermContext pending, int targetIdx, Func<bool>? customPermission = null)
        {
            if (targetIdx < 0 || targetIdx >= pending.Docks.Length) return;
            if (metric == EMetrics.Default) return;
            if (flatBonus == 0) return;

            customPermission ??= () => true;

            if ((metric == EMetrics.GdpGenerated || metric == EMetrics.All) && customPermission())
                pending.Docks[targetIdx].BaseGdpFlatBonus += flatBonus;

            if ((metric == EMetrics.BiosphereChange || metric == EMetrics.All) && customPermission())
                pending.Docks[targetIdx].EffectToNatureFlatBonus += flatBonus;

            if ((metric == EMetrics.HighClassApprovalChange || metric == EMetrics.All) && customPermission())
                pending.Docks[targetIdx].EffectToHighClassFlatBonus += flatBonus;

            if ((metric == EMetrics.LowClassApprovalChange || metric == EMetrics.All) && customPermission())
                pending.Docks[targetIdx].EffectToLowClassFlatBonus += flatBonus;
        }
    }
}
