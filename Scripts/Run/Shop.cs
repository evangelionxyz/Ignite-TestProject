using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Scripts.Cards;
using TestProject.Scripts.Games;

namespace TestProject.Scripts.Run
{
    public class Shop
    {
        private const int REROLL_STARTING_COST = 3;
        private const float REROLL_MULTIPLIER = 1.5f;
        private const int SHOP_STOCK_SIZE = 5;

        private RunState _runState;
        private DeckManager _deckManager;

        private List<PolicyCard> allPoliciesCard = CardRegistry.GetAll().OfType<PolicyCard>().ToList();
        private List<MandateCard> allMandateCard = CardRegistry.GetAll().OfType<MandateCard>().ToList();
        private List<ActionCard> allActionCard = CardRegistry.GetAll().OfType<ActionCard>().ToList();

        private int _rerollCount;
        public int RerollCost => ComputeRerollCost();

        private PolicyCard[] _availablePolicyCards = new PolicyCard[SHOP_STOCK_SIZE];
        private MandateCard[] _availableMandateCards = new MandateCard[SHOP_STOCK_SIZE];
        private ActionCard[] _availableActionCards = new ActionCard[SHOP_STOCK_SIZE];

        public IReadOnlyList<PolicyCard> AvailablePolicyCards => _availablePolicyCards;
        public IReadOnlyList<MandateCard> AvailableMandateCards => _availableMandateCards;
        public IReadOnlyList<ActionCard> AvailableActionCards => _availableActionCards;

        private int ComputeRerollCost()
        {
            return (int)(REROLL_STARTING_COST * Math.Pow(REROLL_MULTIPLIER, _rerollCount));
        }

        public Shop(RunState runState , DeckManager deckManager)
        {
            _runState = runState;
            _deckManager = deckManager;

            GenerateShopStock();
        }

        public void Reroll()
        {
            _rerollCount++;
            GenerateShopStock();
        }

        private void GenerateShopStock()
        {
            string seed = _runState.Seed + _runState.RoundNumber.ToString() + _rerollCount.ToString();
            var random = new Random(seed.GetHashCode());

            for (int i = 0; i < SHOP_STOCK_SIZE; i++)
            {
                _availableActionCards[i] = allActionCard[random.Next(0, allActionCard.Count - 1)];
                _availablePolicyCards[i] = allPoliciesCard[random.Next(0, allPoliciesCard.Count - 1)];

                // Based on rarity weight for mandates
                var sum = allMandateCard.Sum(item => GetRarityWeight(item.Rarity));
                var roll = random.Next(0, sum);
                int cumulative = 0;

                foreach (var card in allMandateCard)
                {
                    cumulative += GetRarityWeight(card.Rarity);
                    if (roll < cumulative)
                    {
                        _availableMandateCards[i] = card;
                        break;
                    }
                }
            }
        }

        private static int GetRarityWeight(ERarity rarity)
        {
            switch (rarity)
            {
                case ERarity.Common:
                    return 70;
                case ERarity.Rare:
                    return 20;
                case ERarity.Legendary:
                    return 10;
                case ERarity.Default:
                default:
                    return 0;
            }
        }
    }
}
