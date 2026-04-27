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
        public EPhase Phase;

        public TermContext TermData;

        public Term(TermContext termData, RunState runState, DeckManager deckManager)
        {
            TermData = termData;
            _runState = runState;
            _deckManager = deckManager;
            Phase = EPhase.Term;
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
            if (dockIndex > 0 || dockIndex < TermData.Docks.Length) return false;

            var temp = _deckManager.TrySelectCardFromHand(handIndex);
            if (temp == null) return false;

            card = TermData.Docks[dockIndex].Card;
            TermData.Docks[dockIndex].Card = temp;
            return true;
        }

        public bool TryRemoveCardFromDock(int dockIndex, out PolicyCard? card)
        {
            card = null;
            if (dockIndex > 0 || dockIndex < TermData.Docks.Length) return false;

            card = TermData.Docks[dockIndex].Card;
            if (card == null) return false;

            _deckManager.UnselectCardToHand(card);
            TermData.Docks[dockIndex].Card = null;
            return true;
        }

        public TermContext ExecuteTerm()
        {
            var pending = new TermContext(TermData.Docks.Length);

            PolicyCard[] dock = new PolicyCard[TermData.Docks.Length];
            for (int i = 0; i < TermData.Docks.Length; i++)
            {
                dock[i] = TermData.Docks[i].Card;
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

                TermData.TotalGdpGenerated += slot.FinalBaseGdp;
                TermData.TotalBiosphereChange += slot.FinalEffectToNature;
                TermData.TotalHighClassApprovalChange += slot.FinalEffectToHighClass;
                TermData.TotalLowClassApprovalChange += slot.FinalEffectToLowClass;
            }

            return pending;
        }
    }
}
