using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;
using Logic;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            var players = new List<IPlayer>();
            for (int i = 0; i < 4; i++)
            {
                players.Add(new ConsolePlayer(i+1));
            }
            var gameManager = new GameManager();
            gameManager.Init(players);
            WriteInfo();
            gameManager.GameProcess();
        }

        static void WriteInfo()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine($"Trump is {Table.Trump.Print()} {Table.Trump}");
        }
    }
}
