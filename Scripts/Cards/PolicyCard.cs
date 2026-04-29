using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Scripts.Cards.Effects.EffectMisc;
using TestProject.Scripts.Cards.Misc;

namespace TestProject.Scripts.Cards
{
    public class PolicyCard : Card
    {
        public readonly EPolicyType Type;
        public int BaseGdp;
        public int EffectToNature;
        public int EffectToHighClass;
        public int EffectToLowClass;

        public PolicyCard(
            string id,
            string name, 
            string effectDesc, 
            List<IEffect> effects,
            EPolicyType type, 
            int baseGdp, int 
            effectToNature, 
            int effectToHighClass, 
            int effectToLowClass
        ): base(id, name, effectDesc, effects)
        {
            Type = type;
            BaseGdp = baseGdp;
            EffectToNature = effectToNature;
            EffectToHighClass = effectToHighClass;
            EffectToLowClass = effectToLowClass;
        }
    }
}
