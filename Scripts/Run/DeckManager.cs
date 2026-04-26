using Ignite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Scripts.Cards;

namespace TestProject.Scripts.Games
{
    class DeckManager
    {
        [SerializeField]
        private int _maxMandates = 5;
        private int _mandateCardCount = 0;
        [SerializeField]
        private int _maxActionCards = 3;
        private int _ActionCardsCount = 0;

        private List<Card> _deck = [];
        private List<Card> _hand = [];
        private List<Card> _discardPile = [] ;
        private List<MandateCard> _mandates = [];
        private List<ActionCard> _actionCards = [];

        public IReadOnlyList<Card> Deck => _deck;
        public IReadOnlyList<Card> Hand => _hand;
        public IReadOnlyList<Card> DiscardPile => _discardPile;
        public IReadOnlyList<MandateCard> Mandates => _mandates;
        public IReadOnlyList<ActionCard> ActionCards => _actionCards;

        public void DrawHand(int handSize, string seed, int roundNumber)
        {
            // Put cards in Hands to Discard Pile
            _discardPile.Concat(_hand);
            _hand.Clear();

            // Check if deck has enough cards, if not shuffle discard pile into deck
            if (handSize < _deck.Count())
                RecycleDiscard(seed, roundNumber);

            // Shuffle deck and move cards from deck to hand
            ShuffleDeck(seed);
            _hand.Concat(_deck[..handSize]);
            _deck.RemoveRange(0, handSize);
        }

        public bool TryRemoveCardFromDeck(Card card)
        {
            if (_hand.Contains(card))
            {
                _deck.Remove(card);
                return true;
            }
            return false;
        }

        public bool TryAddMandateCard(MandateCard card, bool isNegative = false)
        {
            // Check if we have room for more mandates using separate count to anticipate "negative card"
            if (_mandateCardCount < _maxMandates)
            {
                if (!isNegative) _mandateCardCount++;
                _mandates.Add(card);
                return true;
            }
            return false;
        }

        public bool TryAddActionCard(ActionCard card, bool isNegative = false)
        {
            if (_ActionCardsCount < _maxActionCards)
            {
                if (!isNegative) _ActionCardsCount++;
                _actionCards.Add(card);
                return true;
            }
            return false;
        }

        public MandateCard? ReplaceMandate(int slotIndex, MandateCard mandate)
        {
            if (!mandate.isSellable) return null;

            if (slotIndex < 0 || slotIndex >= _mandates.Count) return null;

            var old = _mandates[slotIndex];
            _mandates[slotIndex] = mandate;
            return old;
        }

        public ActionCard? ReplaceActionCard(int slotIndex, ActionCard card)
        {
            if (slotIndex < 0 || slotIndex >= _actionCards.Count) return null;

            var old = _actionCards[slotIndex];
            _actionCards[slotIndex] = card;
            return old;
        }

        public MandateCard? SellMandate(int slotIndex)
        {
            if (!_mandates[slotIndex].isSellable) return null;

            if (slotIndex < 0 || slotIndex >= _mandates.Count) return null;

            var sold = _mandates[slotIndex];
            _mandates.RemoveAt(slotIndex);
            return sold;  
        }

        public ActionCard? SellActionCard(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= _actionCards.Count) return null;

            var sold = _actionCards[slotIndex];
            _actionCards.RemoveAt(slotIndex);
            return sold;
        }

        private void ShuffleDeck(string seed)
        {
            var random = new Random(seed.GetHashCode());

            // Swap each card with a random card from the deck
            for (int i = _deck.Count - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                (_deck[i], _deck[j]) = (_deck[j], _deck[i]);
            }

        }

        private void RecycleDiscard(string seed, int roundNumber)
        {
            // Put discard pile back into deck
            _deck.Concat(_discardPile);
            _discardPile.Clear();

            // Shuffle the deck using seed &  round number to make different shuffle each round
            ShuffleDeck(seed + roundNumber.ToString());
        }
    }
}
