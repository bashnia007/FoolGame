using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;

namespace Logic
{
    public static class InitialSettings
    {
        private static List<Card> CreateCards()
        {
            var cards = new List<Card>();
            foreach (Nominal nominal in Enum.GetValues(typeof(Nominal)))
            {
                foreach (Suit suit in Enum.GetValues(typeof(Suit)))
                {
                    cards.Add(new Card(suit, nominal));
                }
            }
            return cards;
        }

        public static Queue<Card> ShuffleCards()
        {
            var queue = new Queue<Card>();
            var rnd = new Random();
            var cards = CreateCards();
            var count = cards.Count;

            for (int i = 0; i < count; i++)
            {
                var card = cards[rnd.Next(cards.Count)];
                cards.Remove(card);
                queue.Enqueue(card);
            }
            return queue;
        }

        public static void ProvideCards(List<Player> players, Queue<Card> deck, short cardsToHand)
        {
            foreach (var player in players)
            {
                for (int i = 0; i < cardsToHand; i++)
                {
                    player.Hand.Add(deck.Dequeue());
                }
            }
        }

        public static Card ProvideTrump(Queue<Card> deck)
        {
            return deck.Dequeue();
        }
    }
}
