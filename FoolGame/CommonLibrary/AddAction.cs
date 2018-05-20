using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class AddAction : PlayerAction
    {
        public List<Card> AddedCards { get; }

        public bool AddCards(List<Card> cards)
        {
            if (cards.TrueForAll(c => Table.OpenedCards.Select(o => o.Nominal).Contains(c.Nominal)))
            {
                AddedCards.AddRange(cards);
                return true;
            }
            return false;
        }
        public AddAction()
        {
            AddedCards = new List<Card>();
        }
    }
}
