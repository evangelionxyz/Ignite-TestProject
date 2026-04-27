using System;
using System.Collections.Generic;
using System.Linq;
using TestProject.Scripts.Cards;

namespace TestProject.Scripts.Games
{
    public class RunState
    {
        const int FIRST_GDP_TARGET = 100;
        const float GDP_TARGET_INCREMENT = 2.5f;

        const int STARTING_TREASURY = 100;
        const int STARTING_BIOSPHERE = 100;
        const int STARTING_HIGH_CLASS_APPROVAL = 70;
        const int STARTING_LOW_CLASS_APPROVAL = 70;
        const int MAX_METRICS = 100;

        public string Seed;
        public int RoundNumber = 1;
        public int GdpTarget = FIRST_GDP_TARGET;

        public int Treasury = STARTING_TREASURY;
        public int Biosphere = STARTING_BIOSPHERE;
        public int HighClassApproval = STARTING_HIGH_CLASS_APPROVAL;
        public int LowClassApproval = STARTING_LOW_CLASS_APPROVAL;

        public int IndustryProspectsLevel;
        public int WelfareProspectsLevel;
        public int GreenProspectsLevel;

        private List<DebtContract> _contracts;
        public IReadOnlyList<DebtContract> Contracts => _contracts;

        public RunState(string seed)
        {
            Seed = seed;
        }

        public bool isRevolution => HighClassApproval <= 0 || LowClassApproval <= 0;
        public bool isEcologicalCollapse => Biosphere <= 0;

        public void AddTreasury(int amount) => Treasury += amount;
        public void SpendTreasury(int amount) => Treasury = Math.Max(0, Treasury - amount);

        public void AdvanceRound()
        {
            RoundNumber++;
            GdpTarget = CalculateNextTermGdpTarget();
        }

        public void ApplyBiosphereChange(int delta) => Biosphere = Math.Clamp(Biosphere + delta, 0, MAX_METRICS);
        public void ApplyHighClassApprovalChange(int delta) => HighClassApproval = Math.Clamp(HighClassApproval + delta, 0, MAX_METRICS);
        public void ApplyLowClassApprovalChange(int delta) => LowClassApproval = Math.Clamp(LowClassApproval + delta, 0, MAX_METRICS);

        public void AddIndustryProspectsLevel(int amount) => IndustryProspectsLevel += amount;
        public void AddWelfareProspectsLevel(int amount) => WelfareProspectsLevel += amount;
        public void AddGreenProspectsLevel(int amount) => GreenProspectsLevel += amount;

        public void AddDebtContract(DebtContract contract) => _contracts.Add(contract);
        
        public void TickDownContracts()
        {
            foreach (var contract in _contracts)
                contract.TickDown();
        }

        public List<DebtContract> RemoveExpiredDebtContract()
        {
            var expiredContracts = _contracts.Where(c => c.IsExpired).ToList();
            _contracts.RemoveAll(c => c.IsExpired);
            return expiredContracts;
        }

        public bool TryRepayDebtContract(DebtContract contract)
        {
            if (Treasury >= contract.AmountToGet)
            {
                SpendTreasury(contract.AmountToGet);
                _contracts.Remove(contract);
                return true;
            }
            return false;
        }

        private int CalculateNextTermGdpTarget()
        {
            return (int)(GdpTarget * Math.Pow(GDP_TARGET_INCREMENT, RoundNumber));
        }
    }
}