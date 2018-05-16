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
        public List<Player> Players { get; set; }

        public Player ActivePlayer { get; private set; }
        public Player PassivePlayer { get; private set; }

        public void Init(List<Player> players)
        {
            Table.Deck = InitialSettings.ShuffleCards();
            Players = players;
            InitialSettings.ProvideCards(Players, Table.Deck, 6);
            Table.Trump = InitialSettings.ProvideTrump(Table.Deck);

            Players = InitialSettings.SelectOrderOfPlayers(players, Table.Trump);
        }

        public void GameProcess()
        {
            while (true)
            {
                SelectRoles();
                Turn();
                

            }
        }

        /// <summary>
        /// Установить ходящего и бьющегося игроков
        /// </summary>
        private void SelectRoles()
        {
            
        }

        private void Turn()
        {
            var attackCards = ActivePlayer.SelectCards();
            var defenderDecision = PassivePlayer.SelectPlayerAction(attackCards);
            switch (defenderDecision.ActionType)
            {
                case ActionType.Defend:
                    Defend();
                    break;
            }
        }

        private void Defend()
        {
            
        }
    }
}
