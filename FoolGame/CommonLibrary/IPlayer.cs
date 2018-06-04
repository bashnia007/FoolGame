using System.Collections.Generic;

namespace CommonLibrary
{
    public interface IPlayer
    {
        int Id { get; set; }
        List<Card> Hand { get; set; }

        List<Card> SelectCards();
        PlayerAction SelectPlayerAction(bool isAttack = false);
    }
}