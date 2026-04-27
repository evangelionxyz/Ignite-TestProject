using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Scripts.Run;

namespace TestProject.Scripts.Cards.Effects
{
    public class ModifyByCardTypeEffect : IEffect
    {
        private EPolicyType _cardType;
        private EMetrics _metrics;
        

        public void Apply(Card[] dock, int index, TermContext pending)
        {
            throw new NotImplementedException();
        }
    }
}
