using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Scripts.Cards
{
    public abstract class DebtContract
    {
        public readonly string Id;
        public readonly string Name;
        public readonly int AmountToGet;
        public readonly string PenaltyDesc;
        public readonly string MandatesIdToChange;
        private int _countdown;

        public int Countdown => _countdown;
        public bool IsExpired => _countdown < 0;

        public DebtContract(string id, string name, int amountToGet, int countdown, string penaltyDesc, string mandatesToChange)
        {
            Id = id;
            Name = name;
            AmountToGet = amountToGet;
            _countdown = countdown;
            PenaltyDesc = penaltyDesc;
            MandatesIdToChange = mandatesToChange;
        }

        public void TickDown() => _countdown--;
    }
}
