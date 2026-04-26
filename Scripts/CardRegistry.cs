using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Scripts.Cards;

namespace TestProject.Scripts
{
    public class CardRegistry
    {
        const string CARD_DATA_PATH = "data/cards/";
        private static Dictionary<string, Card> cardsDictionary;

        private static void LoadCardData()
        {
            // TODO: Load card data from a file and populate the cardsDictionary
        }

        public static Card Get(string id)
        {
            if (cardsDictionary == null)
                LoadCardData();
            if (cardsDictionary!.TryGetValue(id, out var card))
                return card;
            throw new KeyNotFoundException($"Card with ID '{id}' not found.");
        }
    }
}
