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
            Table.TrumpCard = InitialSettings.ProvideTrump(Table.Deck);
            Table.OpenedCards = new List<Card>();
            Table.NotCoveredCards = new List<Card>();
            Table.IsFirstRound = true;
            Table.VisiblePlayers = InitialSettings.FillVisiblePlayers(players);

            Players = InitialSettings.SelectOrderOfPlayers(players, Table.Trump);

            _isStart = true;
        }

        public void GameProcess()
        {
            while (!CheckGameOver())
            {
                Table.AttackCardsCount = 0;
                Table.OpenedCards = new List<Card>();
                Table.NotCoveredCards = new List<Card>();

                SelectRoles();
                Turn();

                RestoreCards();

                Table.IsFirstRound = false;
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
                UpdateVisiblePlayers();
                return;
            }
            UpdateRoles(_isSuccessfullyDefended ? PassivePlayer : NeighbourPlayer);
            UpdateVisiblePlayers();
        }

        private void UpdateRoles(IPlayer startFrom)
        {
            int active = FindNextActiveUser(Players.IndexOf(startFrom));
            ActivePlayer = Players[active % Players.Count];

            active = FindNextActiveUser(Players.IndexOf(ActivePlayer) + 1);
            PassivePlayer = Players[active % Players.Count];

            active = FindNextActiveUser(Players.IndexOf(PassivePlayer) + 1);
            NeighbourPlayer = Players[active % Players.Count];
        }

        private int FindNextActiveUser(int startIndex)
        {
            int i = 0;
            while (Players[(startIndex + i) % Players.Count].Hand.Count == 0)
            {
                i++;
            }
            return startIndex+i;
        }
        
        public void Turn()
        {
            var isAdd = true;
            _isSuccessfullyDefended = true;
            var attackAction = ActivePlayer.Attack();
            AddAttackCards((attackAction).AttackCards);
            Table.TransferPossible = true;
            while (isAdd)
            {
                var defenderDecision = PassivePlayer.SelectPlayerAction();
                switch (defenderDecision.ActionType)
                {
                    case ActionType.Defend:
                        Defend(defenderDecision as DefendAction);
                        break;
                    case ActionType.Pass:
                        Pass();
                        return;
                    case ActionType.Transfer:
                        Transfer(defenderDecision as TransferAction);
                        continue;
                }
                if (IsDefenderWon()) return;
                var attackerAction = ActivePlayer.SelectPlayerAction(isAttack: true);
                switch (attackerAction.ActionType)
                {
                    case ActionType.Add:
                        AddAttackCards(((AddAction)attackerAction).AddedCards);
                        break;
                    case ActionType.None:
                        isAdd = ActivePlayer.Id != NeighbourPlayer.Id && AddNeighbour(NeighbourPlayer);
                        break;
                }
            }
        }

        private void Pass()
        {
            _isSuccessfullyDefended = false;
            Table.NotCoveredCards = new List<Card>();
            Table.OpenedCards = new List<Card>();
        }

        private void Transfer(TransferAction transferAction)
        {
            int passiveIndex = Players.IndexOf(PassivePlayer);
            int neighbourIndex = Players.IndexOf(NeighbourPlayer);
            ActivePlayer = Players[passiveIndex % Players.Count];
            PassivePlayer = Players[neighbourIndex % Players.Count];
            NeighbourPlayer = Players[(Players.IndexOf(PassivePlayer) + 1) % Players.Count];

            transferAction.Player.Hand.Remove(transferAction.TransferCard);
            Table.NotCoveredCards.Add(transferAction.TransferCard);
            Table.OpenedCards.Add(transferAction.TransferCard);
            Table.AttackCardsCount++;

            UpdateVisiblePlayers();
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
            Table.TransferPossible = false;
            defendActions.Player.Hand.RemoveAll(c => defendActions.CardsPairs.Select(def => def.DefendCard).Contains(c));
            Table.OpenedCards.AddRange(defendActions.CardsPairs.Select(cp => cp.DefendCard));
            Table.NotCoveredCards = new List<Card>();
        }

        private void AddAttackCards(List<Card> attackCards)
        {
            Table.AttackCardsCount += attackCards.Count;
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
            foreach (var player in Players.Where(p => p.Id != ActivePlayer.Id && p.Id != NeighbourPlayer.Id && p.Id != PassivePlayer.Id))
            {
                RestoreCardsToPlayer(player);
            }
        }

        private void RestoreCardsToPlayer(IPlayer player)
        {
            if(player.Hand.Count >= Constants.NessecaryCardsInTheHand) return;
            var cardsToAdd = Math.Min(Table.Deck.Count, Constants.NessecaryCardsInTheHand - player.Hand.Count);
            for (int i = 0; i < cardsToAdd; i++)
            {
                player.Hand.Add(Table.Deck.Dequeue());
            }
        }

        private bool IsDefenderWon()
        {
            if (PassivePlayer.Hand.Count == 0)
            {
                PassivePlayer.WinAction();
                return true;
            }
            return false;
        }

        private bool CheckGameOver()
        {
            if (Table.Deck.Count == 0 && Players.Where(p => p.Hand.Count > 0).ToList().Count == 1)
            {
                Players.First(p => p.Hand.Count > 0).LoseAction();
                return true;
            }
            return false;
        }

        private void UpdateVisiblePlayers()
        {
            Table.VisiblePlayers.First(p => p.Id == ActivePlayer.Id).Role = PlayerRole.Active;
            Table.VisiblePlayers.First(p => p.Id == PassivePlayer.Id).Role = PlayerRole.Passive;
            Table.VisiblePlayers.First(p => p.Id == NeighbourPlayer.Id).Role = PlayerRole.Neighbour;
            foreach (var player in Table.VisiblePlayers.Where(p => p.Id != ActivePlayer.Id && p.Id != NeighbourPlayer.Id && p.Id != PassivePlayer.Id))
            {
                player.Role = PlayerRole.None;
            }
        }
    }
}
