using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Class.Base
{
    public abstract class PolicyCard : Card
    {
        EPolicyType type;
        int baseGdp;
        int effectToNature;
        int effectToApproval;
        int APCost;

        PolicyCard(string name, string effectDesc, EPolicyType type, int baseGdp, int effectToNature, int effectToApproval, int APCost) : base(name, effectDesc)
        {
            this.type = type;
            this.baseGdp = baseGdp;
            this.effectToNature = effectToNature;
            this.effectToApproval = effectToApproval;
            this.APCost = APCost;
        }
    }
}
