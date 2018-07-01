using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;
using Logic;

namespace ConsoleUI
{
    public class ConsolePlayerCreator : PlayerCreator
    {
        public override IPlayer CreatePlayer(int id)
        {
            return new ConsolePlayer(args);
        }
    }
}
