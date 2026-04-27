using TestProject.Scripts.Games;
using TestProject.Scripts.Run;

namespace TestProject.Scripts.Cards.Effects.EffectMisc
{
    public interface IEffect
    {
        public void Apply(Card[] dock, int index, TermContext pending);
    }
}