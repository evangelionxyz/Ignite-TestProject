using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Class.Base
{
    public abstract class JunkCard : Card
    {
        int APCost;

        public JunkCard(string name, string effectDesc, int APCost) : base(name, effectDesc)
        {
            this.APCost = APCost;
        }
    }
}
