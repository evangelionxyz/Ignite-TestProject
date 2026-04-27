using System.Collections.Generic;
using TestProject.Scripts.Games;

namespace TestProject.Scripts.Run
{
    public class TermContext
    {
        public Slot[] Docks;

        public int TotalGdpGenerated;
        public int TotalBiosphereChange;
        public int TotalHighClassApprovalChange;
        public int TotalLowClassApprovalChange;

        public List<int> CardIdToSpawn = [];
        public List<int> CardIdToRemove = [];

        public TermContext(int maxDockCount)
        {
            Docks = new Slot[maxDockCount];
        }
    }
}