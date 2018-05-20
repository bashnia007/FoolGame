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
        public Player NeighbourPlayer { get; private set; }

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
                Turn(ActivePlayer);
                

            }
        }

        /// <summary>
        /// Установить ходящего и бьющегося игроков
        /// </summary>
        private void SelectRoles()
        {
            
        }

        private void Turn(Player attackPlayer)
        {
            var attackCards = attackPlayer.SelectCards();
            Table.OpenedCards.AddRange(attackCards);
            var defenderDecision = PassivePlayer.SelectPlayerAction();
            switch (defenderDecision.ActionType)
            {
                case ActionType.Defend:
                    Defend((DefendAction)defenderDecision);
                    break;
                case ActionType.Pass:
                    return;
                case ActionType.Transfer:
                    return;
            }
            if(Table.OpenedCards.Count >= Constants.MaxCardsOnTheTable) return;

            var attackerDecision = ActivePlayer.SelectPlayerAction(isAttack: true);
            if (attackerDecision.ActionType == ActionType.Add)
            {
                Turn(ActivePlayer);
            }
            else
            {
                var neighbourDecision = NeighbourPlayer.SelectPlayerAction(isAttack: true);
                if (neighbourDecision.ActionType == ActionType.Add)
                {
                    Turn(NeighbourPlayer);
                }
            }
        }

        private void Defend(DefendAction defendActions)
        {
            Table.OpenedCards.AddRange(defendActions.CardsPairs.Select(cp => cp.DefendCard));
        }
    }
}
