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

        private bool _isSuccessfullyDefended = true;
        private bool _isStart = false;
        
        public void Init(List<IPlayer> players)
        {
            Players = players;
            Table.Deck = InitialSettings.ShuffleCards();
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
                Turn();

                RestoreCards();
            }
        }

        /// <summary>
        /// Установить ходящего и бьющегося игроков
        /// </summary>
        public void SelectRoles()
        {
            if (_isStart)
            {
                _isStart = false;
                ActivePlayer = Players[0];
                PassivePlayer = Players[1];
                NeighbourPlayer = Players[2%Players.Count];
                return;
            }
            int passiveIndex = Players.IndexOf(PassivePlayer);
            int neighbourIndex = Players.IndexOf(NeighbourPlayer);
            if (_isSuccessfullyDefended)
            {
                ActivePlayer = Players[passiveIndex % Players.Count];
                PassivePlayer = Players[neighbourIndex % Players.Count];
                NeighbourPlayer = Players[(Players.IndexOf(PassivePlayer) + 1)%Players.Count];
            }
            else
            {
                ActivePlayer = Players[neighbourIndex % Players.Count];
                PassivePlayer = Players[(neighbourIndex + 1) % Players.Count];
                NeighbourPlayer = Players[(neighbourIndex + 2) % Players.Count];
            }
        }

        public void Turn(IPlayer attackPlayer)
        {
            Table.NotCoveredCards = new List<Card>();
            var attackAction = attackPlayer.Attack();
            var attackCards = attackAction.AttackCards;
            foreach (var attackCard in attackCards)
            {
                Table.NotCoveredCards.Add(attackCard);
            }
            Table.OpenedCards.AddRange(attackCards);
            var defenderDecision = PassivePlayer.SelectPlayerAction();
            switch (defenderDecision.ActionType)
            {
                case ActionType.Defend:
                    Defend(defenderDecision as DefendAction);
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

        public void Turn()
        {
            var isAdd = true;
            var attackAction = ActivePlayer.Attack();
            AddAttackCards((attackAction).AttackCards);
            while (Table.OpenedCards.Count <= Constants.MaxCardsOnTheTable && isAdd)
            {
                var defenderDecision = PassivePlayer.SelectPlayerAction();
                switch (defenderDecision.ActionType)
                {
                    case ActionType.Defend:
                        Defend(defenderDecision as DefendAction);
                        break;
                    case ActionType.Pass:
                        _isSuccessfullyDefended = false;
                        return;
                    case ActionType.Transfer:
                        return;
                }
                var attackerAction = ActivePlayer.SelectPlayerAction(isAttack: true);
                switch (attackerAction.ActionType)
                {
                    case ActionType.Add:
                        AddAttackCards(((AddAction)attackerAction).AddedCards);
                        break;
                    case ActionType.None:
                        isAdd = AddNeighbour(NeighbourPlayer);
                        break;
                }
            }
        }

        private bool AddNeighbour(IPlayer player)
        {
            var attackerAction = player.SelectPlayerAction(isAttack: true);
            switch (attackerAction.ActionType)
            {
                case ActionType.Add:
                    AddAttackCards(((AddAction)attackerAction).AddedCards);
                    return true;
                case ActionType.None:
                    return false;
            }
            return false;
        }
        
        private void Defend(DefendAction defendActions)
        {
            Table.OpenedCards.AddRange(defendActions.CardsPairs.Select(cp => cp.DefendCard));
            Table.NotCoveredCards = new List<Card>();
        }

        private void AddAttackCards(List<Card> attackCards)
        {
            foreach (var attackCard in attackCards)
            {
                Table.NotCoveredCards.Add(attackCard);
            }
            Table.OpenedCards.AddRange(attackCards);
        }

        private void RestoreCards()
        {
            RestoreCardsToPlayer(ActivePlayer);
            RestoreCardsToPlayer(NeighbourPlayer);
            RestoreCardsToPlayer(PassivePlayer);
        }

        private void RestoreCardsToPlayer(IPlayer player)
        {
            if(player.Hand.Count >= Constants.MaxCardsInTheHand) return;
            var cardsToAdd = Math.Min(Table.Deck.Count, Constants.MaxCardsInTheHand - player.Hand.Count);
            for (int i = 0; i < cardsToAdd; i++)
            {
                player.Hand.Add(Table.Deck.Dequeue());
            }
        }
    }
}
