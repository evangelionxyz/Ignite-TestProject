namespace TestProject.Scripts.Cards
{
    public class CardSlot
    {
        public int SlotIndex;
        public string? CurrentCardId;
        public bool isEnabled;

        public CardSlot(int slotIndex)
        {
            this.SlotIndex = slotIndex;
            this.CurrentCardId = null;
            this.isEnabled = true;
        }
    }
}