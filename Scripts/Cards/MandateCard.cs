using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Scripts.Cards.Effects.EffectMisc;

namespace TestProject.Scripts.Cards
{
    public class MandateCard : Card
    {
        public readonly ERarity Rarity;
        public readonly bool IsSellable;
        public readonly int SellPrice;

        public MandateCard(
            string id, 
            string name, 
            string effectDesc, 
            List<IEffect> effects,
            ERarity rarity, 
            bool isSellable, 
            int sellPrice
        ) : base(id, name, effectDesc, effects)
        {
            Rarity = rarity;
            IsSellable = isSellable;
            SellPrice = sellPrice;
        }
    }
}
