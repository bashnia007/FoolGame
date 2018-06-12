using System.Collections.Generic;

namespace CommonLibrary
{
    public interface IPlayer
    {
        int Id { get; set; }
        List<Card> Hand { get; set; }
        
        IPlayerAction SelectPlayerAction(bool isAttack = false);

        AttackAction Attack();

        void WinAction();

        void LoseAction();

    }
}