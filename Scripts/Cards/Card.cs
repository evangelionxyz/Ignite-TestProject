using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Scripts.Cards.Effects.EffectMisc;

namespace TestProject.Scripts.Cards
{
    public abstract class Card
    {
        string _id; 
        string _name;
        string _effectDesc;
        List<IEffect> _effects;

        public string Id => _id;
        public string Name => _name;
        public string EffectDesc => _effectDesc;
        public IReadOnlyList<IEffect> Effects => _effects;

        public Card(string id, string name, string effectDesc, List<IEffect> effects)
        {
            _id = id;
            _name = name;
            _effectDesc = effectDesc;
            _effects = effects;
        }
    }
}
