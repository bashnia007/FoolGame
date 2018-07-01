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
        public StartManager(PlayerCreator creator)
        {
            _creator = creator;
        }
        public void Start()
        {
            var players = new List<IPlayer>();
            for (int i = 0; i < 4; i++)
            {
                players.Add(_creator.CreatePlayer(i));
            }
            var gameManager = new GameManager();
            gameManager.Init(players);
            gameManager.GameProcess();
        }
    }
}
