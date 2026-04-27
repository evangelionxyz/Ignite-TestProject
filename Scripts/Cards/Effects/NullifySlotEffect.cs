using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Scripts.Cards.Effects.EffectMisc;
using TestProject.Scripts.Run;

namespace TestProject.Scripts.Cards.Effects
{
    public class NullifySlotEffect : IEffect
    {
        private int _slotIndexToNullify;

        public NullifySlotEffect(int slotIndexToNullify)
        {
            _slotIndexToNullify = slotIndexToNullify;
        }

        public void Apply(Card[] dock, int index, TermContext pending)
        {
            if (pending is null) return;
            if (_slotIndexToNullify < 0 || _slotIndexToNullify >= pending.Docks.Length) return;

            pending.Docks[_slotIndexToNullify].IsNullified = true;
            pending.Docks[_slotIndexToNullify].Modifiers.Add(new Games.ModifierEntry(
                _slotIndexToNullify, EMetrics.Default, 0, Run.Misc.EModifierType.Nullify
                ));
        }
    }
}