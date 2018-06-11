using System.Collections.Generic;
using CommonLibrary;
using Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class LogicUnitTests
    {
        [TestMethod]
        public void TestTurn()
        {
            var gameManager = new GameManager();
            var players = new List<Player>
            {
                new Player(1),
                new Player(2),
                new Player(3)
            };

            //gameManager.Init(players);
            //gameManager.SelectRoles();
        }
    }
}