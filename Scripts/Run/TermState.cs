using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Scripts.Cards;

namespace TestProject.Scripts.Games
{
    public class TermState
    {
        const int DISCARD_COUNT = 2;

        public Slot[] Dock;
        public List<PolicyCard> Hand;
        public List<PolicyCard> DiscardPile;
        public int DiscardCount;

        public int TempBiosphereChange;
        public int TempHighClassApprovalChange;
        public int TempLowClassApprovalChange;
        public int GdpGenerated;

        public int TargetGdp;

        public TermState(int targetGdp, int discardModifier = DISCARD_COUNT)
        {
            Dock = [new Slot(""), new Slot(""), new Slot("")];
            Hand = [];
            DiscardPile = [];
            TargetGdp = targetGdp;
            DiscardCount = discardModifier;

            TempBiosphereChange = 0;
            TempHighClassApprovalChange = 0;
            TempLowClassApprovalChange = 0;
            GdpGenerated = 0;
        }
    }
}
