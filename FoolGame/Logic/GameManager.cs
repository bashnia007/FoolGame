using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;

namespace Logic
{
    public class GameManager
    {
        public Table Table { get; set; }
        public List<Player> Players { get; set; }

        public Player ActivePlayer { get; set; }
        public Player PassivePlayer { get; set; }

        public void Init(List<Player> players)
        {
            var table = new Table();
            table.Deck = InitialSettings.ShuffleCards();
            Players = players;
            InitialSettings.ProvideCards(Players, table.Deck, 6);
            table.Trump = InitialSettings.ProvideTrump(table.Deck);

            Players = InitialSettings.SelectOrderOfPlayers(players, table.Trump);
        }

        public void GameProcess()
        {
        }
    }
}
