using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Scripts.Cards
{
    public abstract class DebtContract
    {
        public readonly string Name;
        public readonly int AmountToGet;
        private int _countdown;
        public readonly string PenaltyDesc;

        public int Countdown => _countdown;
        public bool IsExpired => _countdown < 0;

        public DebtContract(string name, int amountToGet, int countdown, string penaltyDesc)
        {
            Name = name;
            AmountToGet = amountToGet;
            _countdown = countdown;
            PenaltyDesc = penaltyDesc;
        }

        public void TickDown()
        {
            _countdown--;
        }
    }
}
