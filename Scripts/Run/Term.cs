using Ignite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Scripts.Cards;
using TestProject.Scripts.Run;
using TestProject.Scripts.Run.Misc;

namespace TestProject.Scripts.Games
{
    public class Term
    {
        const int DEFAULT_HAND_SIZE = 5;

        [SerializeField]
        private int _handSize = DEFAULT_HAND_SIZE;

        private RunState _runState;
        private DeckManager _deckManager;

        public TermContext State;

        public Term(RunState runState, DeckManager deckManager)
        {
            State = new(_handSize);
            _runState = runState;
            _deckManager = deckManager;
        }


        public void DrawHand()
        {
            foreach (var card in _deckManager.Hand)
            {
                _deckManager.UnselectCardToHand(card);
            }
            _deckManager.DrawHand(_handSize, _runState.Seed, _runState.RoundNumber);
        }

        public bool TryPlaceCardToDock(int handIndex, int dockIndex, out PolicyCard? card)
        {
            card = null;
            if (dockIndex > 0 || dockIndex < State.Docks.Length) return false;

            var temp = _deckManager.TrySelectCardFromHand(handIndex);
            if (temp == null) return false;

            card = State.Docks[dockIndex].Card;
            State.Docks[dockIndex].Card = temp;
            return true;
        }

        public bool TryRemoveCardFromDock(int dockIndex, out PolicyCard? card)
        {
            card = null;
            if (dockIndex > 0 || dockIndex < State.Docks.Length) return false;

            card = State.Docks[dockIndex].Card;
            if (card == null) return false;

            _deckManager.UnselectCardToHand(card);
            State.Docks[dockIndex].Card = null;
            return true;
        }

        public TermContext ExecuteTerm()
        {
            var pending = new TermContext(State.Docks.Length);

            PolicyCard[] dock = new PolicyCard[State.Docks.Length];
            for (int i = 0; i < State.Docks.Length; i++)
            {
                dock[i] = State.Docks[i].Card;
                pending.Docks[i] = new Slot(dock[i]);
            }

            // Apply effects of all cards in dock
            for (int i = 0; i < dock.Length; i++)
            {
                if (dock[i] is null) continue;
                foreach (var effect in dock[i].Effects)
                    effect.Apply(dock, i, pending);
            }

            // Apply mandates effects
            for (int i = 0; i < _deckManager.Mandates.Count; i++)
            {
                var mandate = _deckManager.Mandates[i];
                foreach (var effect in mandate.Effects)
                    effect.Apply(dock, i, pending);
            }

            // Calculate final term result
            foreach (var slot in pending.Docks)
            {
                if (slot.Card is null || slot.IsDisabled) continue;

                State.TotalGdpGenerated += slot.FinalBaseGdp;
                State.TotalBiosphereChange += slot.FinalEffectToNature;
                State.TotalHighClassApprovalChange += slot.FinalEffectToHighClass;
                State.TotalLowClassApprovalChange += slot.FinalEffectToLowClass;
            }

            return pending;
        }
    }
}
