using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Scripts.Player;

namespace TestProject.Scripts.Cards
{
    public abstract class PolicyCard : Card
    {
        EPolicyType type;
        public int BaseGdp;
        public int EffectToNature;
        public int EffectToHighClass;
        public int EffectToLowClass;
        public int APCost;
        private bool isUsable = false;

        PolicyCard(string name, string effectDesc, EPolicyType type, int baseGdp, int effectToNature, int effectToHighClass, int effectToLowClass, int APCost) : base(name, effectDesc)
        {
            this.type = type;
            this.BaseGdp = baseGdp;
            this.EffectToNature = effectToNature;
            this.EffectToHighClass = effectToHighClass;
            this.EffectToLowClass = effectToLowClass;
            this.APCost = APCost;
        }

        internal abstract void ApplySynergyEffect(List<PolicyCard> docks);
        
        public bool CheckRequirement()
        {
            isUsable = true;
            return isUsable;
        }
    }
}
