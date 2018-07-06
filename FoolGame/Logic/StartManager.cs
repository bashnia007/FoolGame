using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;

namespace Logic
{
    public class StartManager
    {
        private readonly PlayerCreator _creator;
        private GameManager _gameManager;
        public List<IPlayer> Players;
        public StartManager(PlayerCreator creator)
        {
            _creator = creator;
        }

        public void Init()
        {
            var players = new List<IPlayer>();
            for (int i = 0; i < 4; i++)
            {
                players.Add(_creator.CreatePlayer(i));
            }
            _gameManager = new GameManager();
            _gameManager.Init(players);

            Players = _gameManager.Players;
        }
        public void Start()
        {
            _gameManager.GameProcess();
        }
    }
}
