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
        private int _id;
        private string _name;
        private string _effectDesc;
        private List<IEffect> _effects;

        public string Name => _name;
        public string EffectDesc => _effectDesc;
        public IReadOnlyList<IEffect> Effects => _effects;

        public Card(int id, string name, string effectDesc, List<IEffect> effects)
        {
            _id = id;
            _name = name;
            _effectDesc = effectDesc;
            _effects = effects;
        }
    }
}
