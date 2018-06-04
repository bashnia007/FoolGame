using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public static class Table
    {
        /// <summary>
        /// Колода карт
        /// </summary>
        public static Queue<Card> Deck { get; set; }
        /// <summary>
        /// Козырь
        /// </summary>
        public static Suit Trump { get; set; }

        /// <summary>
        /// Открытые карты на столе
        /// </summary>
        public static List<Card> OpenedCards { get; set; }

        /// <summary>
        /// Не побитые карты на столе
        /// </summary>
        public static List<Card> NotCoveredCards { get; set; }
    }
}
