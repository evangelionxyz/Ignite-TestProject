using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Class.Base
{
    public abstract class Card
    {
        string name;
        string effectDesc;

        public Card(string name, string effectDesc)
        {
            this.name = name;
            this.effectDesc = effectDesc;
        }

        public abstract void ApplyEffect();
    }
}
