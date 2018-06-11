using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class PassAction : IPlayerAction
    {
        public IPlayer Player { get; set; }
        public ActionType ActionType => ActionType.Pass;

        public PassAction(IPlayer player)
        {
            Player = player;
            Player.Hand.AddRange(Table.OpenedCards);
        }
    }
}
