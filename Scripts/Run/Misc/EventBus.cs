using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Scripts.Games;
using TestProject.Scripts.Run;

namespace TestProject.Scripts.Run.Misc
{
    public static class EventBus
    {
        public static event Action<Term>? OnTermStarted;
        public static event Action<TermContext>? OnTermResolved;
        public static event Action<Shop>? OnShopStarted;
        public static event Action<EGameOverReason>? OnGameOver;
        public static event Action<RunState>? OnStateChanged;
        public static event Action<int>? OnBiologicalThresholdReached;

        internal static void EmitTermStarted(Term t) => OnTermStarted?.Invoke(t);
        internal static void EmitTermResolved(TermContext r) => OnTermResolved?.Invoke(r);
        internal static void EmitShopStarted(Shop s) => OnShopStarted?.Invoke(s);
        internal static void EmitGameOver(EGameOverReason r) => OnGameOver?.Invoke(r);
        internal static void EmitStateChanged(RunState s) => OnStateChanged?.Invoke(s);
        internal static void EmitBiologicalThresholdReached(int b) => OnBiologicalThresholdReached?.Invoke(b);
    }
}
