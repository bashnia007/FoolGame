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
        public static Suit Trump => TrumpCard.Suit;
        /// <summary>
        /// Карта-козырь, последняя в колоде
        /// </summary>
        public static Card TrumpCard { get; set; }

        /// <summary>
        /// Открытые карты на столе
        /// </summary>
        public static List<Card> OpenedCards { get; set; }

        /// <summary>
        /// Не побитые карты на столе
        /// </summary>
        public static List<Card> NotCoveredCards { get; set; }

        /// <summary>
        /// Видимая всем информация о игроках
        /// </summary>
        public static List<VisiblePlayer> VisiblePlayers { get; set; }

        public static int AttackCardsCount { get; set; }

        public static bool IsFirstRound { get; set; }

        public static bool TransferPossible { get; set; }
    }
}
