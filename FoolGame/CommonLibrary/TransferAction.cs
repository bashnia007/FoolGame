using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class TransferAction : IPlayerAction
    {
        public IPlayer Player { get; set; }
        public ActionType ActionType { get; }
    }
}
