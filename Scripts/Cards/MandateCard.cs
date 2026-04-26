using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Scripts.Class.Base
{
    public abstract class MandateCard : Card
    {
        ERarity rarity;
        public readonly bool isSellable;

        public MandateCard(string name, string effectDesc, ERarity rarity, bool isSellable) : base(name, effectDesc)
        {
            this.rarity = rarity;
            this.isSellable = isSellable;
        }

        public abstract void ApplyMandate();
    }
}
