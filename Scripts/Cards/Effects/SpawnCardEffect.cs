using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Scripts.Run;

namespace TestProject.Scripts.Cards.Effects
{
    public class SpawnCardEffect : IEffect
    {
        private string _cardId;

        public SpawnCardEffect(string cardId)
        {
            _cardId = cardId;
        }

        public void Apply(Card[] dock, int index, TermContext pending)
        {
            pending.CardIdToSpawn.Add(_cardId);
        }
    }
}
