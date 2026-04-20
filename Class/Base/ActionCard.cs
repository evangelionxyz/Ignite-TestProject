using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Class.Base
{
    public abstract class ActionCard: Card
    {
        int treasuryCost;

        public ActionCard(string name, string effectDesc, int treasuryCost) : base(name, effectDesc)
        {
            this.treasuryCost = treasuryCost;
        }
    }
}
