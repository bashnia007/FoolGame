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
        public List<IPlayer> Players { get; set; }

        public IPlayer ActivePlayer { get; set; }
        public IPlayer PassivePlayer { get; set; }
        public IPlayer NeighbourPlayer { get; set; }

        private bool _isSuccessfullyDefended = false;
        private bool _isStart = false;
        
        public void Init(List<IPlayer> players)
        {
            Table.Deck = InitialSettings.ShuffleCards();
            Players = players;
            InitialSettings.ProvideCards(Players, Table.Deck, 6);
            Table.Trump = InitialSettings.ProvideTrump(Table.Deck);
            Table.OpenedCards = new List<Card>();
            Table.NotCoveredCards = new List<Card>();

            Players = InitialSettings.SelectOrderOfPlayers(players, Table.Trump);

            _isStart = true;
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
        public void SelectRoles()
        {
            if (_isStart)
            {
                ActivePlayer = Players[0];
                PassivePlayer = Players[1];
                NeighbourPlayer = Players[2%Players.Count];
                return;
            }
            if (_isSuccessfullyDefended)
            {
                ActivePlayer = PassivePlayer;
                PassivePlayer = NeighbourPlayer;
                NeighbourPlayer = Players[(Players.IndexOf(PassivePlayer) + 1)/Players.Count];
            }
            else
            {
                ActivePlayer = NeighbourPlayer;
                PassivePlayer = Players[(Players.IndexOf(PassivePlayer) + 1) / Players.Count];
                NeighbourPlayer = Players[(Players.IndexOf(PassivePlayer) + 2) / Players.Count];
            }
        }

        public void Turn(IPlayer attackPlayer)
        {
            var attackCards = attackPlayer.SelectCards();
            Table.NotCoveredCards.AddRange(attackCards);
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
