using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Scripts.Cards.Effects.EffectMisc;

namespace TestProject.Scripts.Cards
{
    public abstract class ActionCard: Card
    {
        public readonly int SellPrice;

        public ActionCard(
            string id, 
            string name, 
            string effectDesc, 
            List<IEffect> effects, 
            int sellPrice
        ): base(id, name, effectDesc, effects)
        {
            SellPrice = sellPrice;
        }
    }
}
