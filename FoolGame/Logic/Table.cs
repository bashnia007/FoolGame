using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;

namespace Logic
{
    public class Table
    {
        /// <summary>
        /// Колода карт
        /// </summary>
        public Queue<Card> Deck { get; set; }
        /// <summary>
        /// Козырь
        /// </summary>
        public Suit Trump { get; set; }
    }
}
