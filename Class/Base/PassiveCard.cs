using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Class.Base
{
    public abstract class PassiveCard : Card
    {
        ERarity rarity;

        public PassiveCard(string name, string effectDesc, ERarity rarity) : base(name, effectDesc)
        {
            this.rarity = rarity;
        }
    }
}
