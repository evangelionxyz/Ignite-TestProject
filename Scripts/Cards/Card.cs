using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Scripts.Cards.Effects;

namespace TestProject.Scripts.Cards
{
    public abstract class Card
    {
        string name;
        string effectDesc;
        List<IEffect> effects;

        public Card(string name, string effectDesc)
        {
            this.name = name;
            this.effectDesc = effectDesc;
        }
    }
}
