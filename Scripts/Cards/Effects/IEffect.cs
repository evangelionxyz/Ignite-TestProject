using TestProject.Scripts.Games;
using TestProject.Scripts.Run;

namespace TestProject.Scripts.Cards.Effects
{
    public interface IEffect
    {
        public void Apply(CardSlot slot, TermContext pending);
    }
}