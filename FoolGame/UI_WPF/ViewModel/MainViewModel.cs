using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommonLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Logic;
using UI_WPF.Helpers;
using UI_WPF.Models;

namespace UI_WPF.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<Card> Cards { get; set; }
        public ObservableCollection<IPlayer> Players { get; set; } 
        public Player CurrentPlayer { get; set; }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            Init();
        }

        private StartManager _startManager;
        private void Init()
        {
            _startManager = new StartManager(new DesktopPlayerCreator());
            _startManager.Init();
            var list = _startManager.Players;
            Players = new ObservableCollection<IPlayer>(list);
        }

        private void Start()
        {
            var task = new TaskFactory().StartNew(StartGame);
        }

        private async void StartGame()
        {
            Thread.CurrentThread.Name = "Main";
            try
            {
                await Task.Run(() =>
                {
                    _startManager.Start();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region OpenPlayerViewCommand

        private RelayCommand _openPlayerView;

        public ICommand OpenPlayerViewCommand
            => _openPlayerView ?? (_openPlayerView = new RelayCommand(ExecuteOpenPlayerView, CanExecuteOpenPlayerView));

        private void ExecuteOpenPlayerView()
        {
            foreach (var player in Players)
            {
                var playerVm = new PlayerViewModel((DesktopPlayer) player);
                var playerWindow = new PlayerView();
                playerWindow.DataContext = playerVm;
                playerWindow.Show();
            }
            /*var playerVm = new PlayerViewModel((DesktopPlayer) Players[0]);
            var playerWindow = new PlayerView();
            playerWindow.DataContext = playerVm;
            playerWindow.Show();*/
            Start();
        }

        private bool CanExecuteOpenPlayerView()
        {
            return true;
        }

        #endregion
    }
}