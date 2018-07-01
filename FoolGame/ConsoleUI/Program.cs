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
            var startManager = new StartManager(new ConsolePlayerCreator());
            startManager.Start();
            WriteInfo();
        }

        static void WriteInfo()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine($"Trump is {Table.Trump.Print()} {Table.Trump}");
        }
    }
}
