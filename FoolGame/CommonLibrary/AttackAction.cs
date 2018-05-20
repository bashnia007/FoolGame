using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class AttackAction : PlayerAction
    {
        public List<Card> AttackCards { get; }

        public bool AddCards(List<Card> cards)
        {
            if (cards.GroupBy(c => c.Nominal).Count() == cards.Count)
            {
                AttackCards.AddRange(cards);
                return true;
            }
            return false;
        }
        public AttackAction()
        {
            AttackCards = new List<Card>();
        }
    }
}
