using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommonLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using UI_WPF.Models;

namespace UI_WPF.ViewModel
{
    public class PlayerViewModel : ViewModelBase
    {
        public DesktopPlayer Player { get; private set; }

        public ObservableCollection<PlayerCard> PlayerCards { get; set; }
        public string Suit => Table.Trump.ToString();
        public string CardsCount => Table.Deck.Count.ToString();
        public string Title { get; set; }
        public IPlayerAction PlayerAction { get; set; }

        public string Message { get; set; }

        public ObservableCollection<Card> CardsOnTable { get; set; }

        private string _waitingFor;
        private Mutex _mutex = new Mutex();

        public string WaitingFor
        {
            get { return _waitingFor; }
            set
            {
                _waitingFor = value;
                RaisePropertyChanged(nameof(WaitingFor));
            }
        }
        
        private bool _isAttack;
        public PlayerViewModel(DesktopPlayer player)
        {
            Player = player;
            PlayerCards = new ObservableCollection<PlayerCard>();
            FillPlayerHand();
            Player.PlayerActionEvent += Handler;
            Title = "Player " + player.Id;
            _isAttack = false;
            CardsOnTable = new ObservableCollection<Card>();
        }

        private void FillPlayerHand()
        {
            foreach (var card in Player.Hand)
            {
                PlayerCards.Add(new PlayerCard(card.Suit, card.Nominal, true));
            }
        }

        public IPlayerAction Handler(bool isAttack)
        {
            UpdateTable();
            //Player.MutexObj.WaitOne();
            //_mutex.WaitOne();
            if (isAttack)
            {
                WaitingFor = "Атакуй";
            }
            else
            {
                WaitingFor = "Защищайся";
            }
            //MessageBox.Show("ololo");
            _isAttack = isAttack;
            var threadName = Thread.CurrentThread;
            //Player.WaitForPlayerAction.WaitOne();
            while (PlayerAction == null)
            {
                Thread.Sleep(1000);
            }
            return PlayerAction;
        }

        #region Commands
        #region AttackCommand

        private RelayCommand<object> _attackCommand;

        public ICommand AttackCommand
            => _attackCommand ?? (_attackCommand = new RelayCommand<object>(ExecuteAttackCommand, CanExecuteAttackCommand));

        private void ExecuteAttackCommand(object selectedItems)
        {
            var cards = ReadSelectedCards(selectedItems);
            var attackAction = new AttackAction(Player);
            if (attackAction.AddCards(cards.ToList()))
            {
                PlayerAction = attackAction;
            }
            else
            {
                MessageBox.Show("Вы не можете выбрать эти карты");
            }
            //Player.WaitForPlayerAction.Set();
            //Player.MutexObj.ReleaseMutex();
            //_mutex.ReleaseMutex();
        }

        private bool CanExecuteAttackCommand(object selectedItems)
        {
            return true;
        }

        #endregion

        #region AddCommand

        private RelayCommand _addCommand;

        public ICommand AddCommand
            => _addCommand ?? (_addCommand = new RelayCommand(ExecuteAddCommand, CanExecuteAddCommand));

        private void ExecuteAddCommand()
        {

        }

        private bool CanExecuteAddCommand()
        {
            return true;
        }

        #endregion

        #region DefendCommand

        private RelayCommand<object> _defendCommand;

        public ICommand DefendCommand
            =>
                _defendCommand ??
                (_defendCommand = new RelayCommand<object>(ExecuteDefendCommand, CanExecuteDefendCommand));

        private void ExecuteDefendCommand(object selectedItems)
        {
            var cards = ReadSelectedCards(selectedItems);
            var defendAction = new DefendAction(Player);
            foreach (var notCoveredCard in Table.NotCoveredCards)
            {
                Message = "Выбери карту для "+notCoveredCard;
            }
        }

        private bool CanExecuteDefendCommand(object arg)
        {
            return true;
        }

        #endregion

        #endregion

        private List<Card> ReadSelectedCards(object cards)
        {
            System.Collections.IList items = (System.Collections.IList)cards;
            return (List<Card>) items.Cast<Card>();
        }

        private void UpdateTable()
        {
            CardsOnTable = new ObservableCollection<Card>(Table.OpenedCards);
            RaisePropertyChanged(nameof(CardsOnTable));
        }
    }
}
