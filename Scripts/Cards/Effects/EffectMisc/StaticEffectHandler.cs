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
        public static void ArithmeticMultiplier(EMetrics metric, float multiplier, TermContext pending, int targetIdx, Func<bool>? customPermission = null)
        {
            bool isChanged = false;

            if (targetIdx < 0 || targetIdx >= pending.Docks.Length) return;
            if (metric == EMetrics.Default) return;
            if (multiplier == 0) return;

            customPermission ??= () => true;

            if ((metric == EMetrics.GdpGenerated || metric == EMetrics.All) && customPermission())
            { 
                pending.Docks[targetIdx].BaseGdpMultiplier += multiplier;
                isChanged = true;
            }

            if ((metric == EMetrics.BiosphereChange || metric == EMetrics.All) && customPermission())
            {
                pending.Docks[targetIdx].EffectToNatureMultiplier += multiplier; 
                isChanged = true; 
            }

            if ((metric == EMetrics.HighClassApprovalChange || metric == EMetrics.All) && customPermission())
            {
                pending.Docks[targetIdx].EffectToHighClassMultiplier += multiplier;
                isChanged = true;
            }

            if ((metric == EMetrics.LowClassApprovalChange || metric == EMetrics.All) && customPermission())
            {
                pending.Docks[targetIdx].EffectToLowClassMultiplier += multiplier;
                isChanged = true;
            }

            if (isChanged) 
                pending.Docks[targetIdx].Modifiers.Add(new Games.ModifierEntry(
                    targetIdx, metric, multiplier, Run.Misc.EModifierType.Multiply
                ));
        }

        public static void GeometricMultiplier(EMetrics metric, float multiplier, TermContext pending, int targetIdx, Func<bool>? customPermission = null)
        {
            var isChanged = false;

            if (targetIdx < 0 || targetIdx >= pending.Docks.Length) return;
            if (metric == EMetrics.Default) return;
            if (multiplier == 0) return;

            customPermission ??= () => true;

            if ((metric == EMetrics.GdpGenerated || metric == EMetrics.All) && customPermission())
            {
                pending.Docks[targetIdx].BaseGdpMultiplier *= multiplier;
                isChanged = true;
            }

            if ((metric == EMetrics.BiosphereChange || metric == EMetrics.All) && customPermission())
            {
                pending.Docks[targetIdx].EffectToNatureMultiplier *= multiplier;
                isChanged = true;
            }

            if ((metric == EMetrics.HighClassApprovalChange || metric == EMetrics.All) && customPermission())
            {
                pending.Docks[targetIdx].EffectToHighClassMultiplier *= multiplier;
                isChanged = true;
            }

            if ((metric == EMetrics.LowClassApprovalChange || metric == EMetrics.All) && customPermission())
            {
                pending.Docks[targetIdx].EffectToLowClassMultiplier *= multiplier;
                isChanged = true;
            }

            if (isChanged)
                pending.Docks[targetIdx].Modifiers.Add(new Games.ModifierEntry(
                    targetIdx, metric, multiplier, Run.Misc.EModifierType.Multiply
                ));
        }

        public static void ModifyFlatBonus(EMetrics metric, int flatBonus, TermContext pending, int targetIdx, Func<bool>? customPermission = null)
        {
            var isChanged = false;

            if (targetIdx < 0 || targetIdx >= pending.Docks.Length) return;
            if (metric == EMetrics.Default) return;
            if (flatBonus == 0) return;

            customPermission ??= () => true;

            if ((metric == EMetrics.GdpGenerated || metric == EMetrics.All) && customPermission())
            {
                pending.Docks[targetIdx].BaseGdpFlatBonus += flatBonus;
                isChanged = true;
            }

            if ((metric == EMetrics.BiosphereChange || metric == EMetrics.All) && customPermission())
            {
                pending.Docks[targetIdx].EffectToNatureFlatBonus += flatBonus;
                isChanged = true;
            }

            if ((metric == EMetrics.HighClassApprovalChange || metric == EMetrics.All) && customPermission())
            {
                pending.Docks[targetIdx].EffectToHighClassFlatBonus += flatBonus;
                isChanged = true;
            }

            if ((metric == EMetrics.LowClassApprovalChange || metric == EMetrics.All) && customPermission())
            {
                pending.Docks[targetIdx].EffectToLowClassFlatBonus += flatBonus;
                isChanged = true;
            }

            if (isChanged)
                pending.Docks[targetIdx].Modifiers.Add(new Games.ModifierEntry(
                    targetIdx, metric, flatBonus, Run.Misc.EModifierType.Flat
                ));
        }
    }
}
