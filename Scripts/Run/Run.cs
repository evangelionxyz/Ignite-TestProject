using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Scripts.Games;
using TestProject.Scripts.Run.Misc;

namespace TestProject.Scripts.Run
{
    public class Run
    {
        public RunState State;
        public DeckManager DeckManager;
        public EPhase CurrentPhase = EPhase.Shop;

        public Term? CurrentTerm;
        public Shop? CurrentShop;

        public Run(string seed)
        {
            State = new(seed);
            DeckManager = new();
        }

        public Term StartTerm()
        {
            CurrentShop = null;
            CurrentTerm = new(State, DeckManager);
            CurrentPhase = EPhase.Term;
            return CurrentTerm;
        }

        public Shop StartShop()
        {
            CurrentTerm = null;
            CurrentShop = new(State, DeckManager);
            CurrentPhase = EPhase.Shop;
            return CurrentShop;
        }
    }
}
