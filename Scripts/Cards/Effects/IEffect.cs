using TestProject.Scripts.Cards;
using TestProject.Scripts.Games;

namespace TestProject.Scripts.Cards.Effects
{
    public interface IEffect
    {
        public void Apply(CardSlot slot, RunState pending);
    }
}