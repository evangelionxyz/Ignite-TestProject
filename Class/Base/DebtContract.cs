using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Class.Base
{
    public abstract class DebtContract
    {
        string name;
        int amountToGet;
        int countdown;
        string penaltyDesc;

        public DebtContract(string name, int amountToGet, int countdown, string penaltyDesc)
        {
            this.name = name;
            this.amountToGet = amountToGet;
            this.countdown = countdown;
            this.penaltyDesc = penaltyDesc;
        }

        public abstract void ApplyPenalty();
    }
}
