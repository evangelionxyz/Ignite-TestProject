namespace TestProject.Scripts.Games
{
    public class ModifierEntry
    {
        // Untuk dibaca UI sebagai log efek
        public int TargetCardIndex;
        public string TargetStat;
        public float Value;
        public EModifierType Type;

        public ModifierEntry(int targetCardIndex, string targetStat, float value, EModifierType type)
        {
            TargetCardIndex = targetCardIndex;
            TargetStat = targetStat;
            Value = value;
            Type = type;
        }
    }
}