using TestProject.Scripts.Cards.Effects.EffectMisc;
using TestProject.Scripts.Run.Misc;

namespace TestProject.Scripts.Games
{
    public class ModifierEntry
    {
        // Untuk dibaca UI sebagai log efek
        public int TargetCardIndex;
        public EMetrics TargetStat;
        public float Value;
        public EModifierType Type;

        public ModifierEntry(int targetCardIndex, EMetrics targetStat, float value, EModifierType type)
        {
            TargetCardIndex = targetCardIndex;
            TargetStat = targetStat;
            Value = value;
            Type = type;
        }
    }
}