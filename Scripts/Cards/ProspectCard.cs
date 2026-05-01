using System.Collections.Generic;
using TestProject.Scripts.Cards.Effects.EffectMisc;

namespace TestProject.Scripts.Cards
{
    public class ProspectCard : Card
    {
        public ProspectCard(string id, string name, string effectDesc, List<IEffect> effects) : base(id, name, effectDesc, effects)
        {
        }
    }
}