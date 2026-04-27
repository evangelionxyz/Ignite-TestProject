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

        private static Dictionary<int, Card> cardsDictionary;

        private static void LoadCardData()
        {
            // TODO: Load card data from a file and populate the cardsDictionary
        }

        public static Card Get(int id)
        {
            if (cardsDictionary == null)
                LoadCardData();
            if (cardsDictionary!.TryGetValue(id, out var card))
                return card;
            throw new KeyNotFoundException($"Card with ID '{id}' not found.");
        }

        public static List<Card> GetAll() { 
            if (cardsDictionary == null)
                LoadCardData();
            return cardsDictionary!.Values.ToList();
        }
    }
}
