using System.Collections.Generic;

namespace CommonLibrary
{
    public interface IPlayerAction
    {
        IPlayer Player { get; set; }
        ActionType ActionType { get; }
    }
}
