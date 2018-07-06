using System;
using System.Collections.Generic;
using System.Threading;
using CommonLibrary;

namespace UI_WPF.Models
{
    public class DesktopPlayer : IPlayer
    {
        public AutoResetEvent WaitForPlayerAction = new AutoResetEvent(true);
        public Mutex MutexObj = new Mutex();
        public delegate IPlayerAction PlayerActionDelegate(bool isAttack);

        public event PlayerActionDelegate PlayerActionEvent;
        public DesktopPlayer(int id)
        {
            Id = id;
            Hand = new List<Card>();
        }
        public int Id { get; set; }
        public List<Card> Hand { get; set; }
        public IPlayerAction SelectPlayerAction(bool isAttack = false)
        {
            var result = PlayerActionEvent?.Invoke(isAttack);
            //WaitForPlayerAction.WaitOne();
            //Thread.CurrentThread.Suspend();
            return result;
            //return new NoneAction();
            throw new NotImplementedException();
        }

        public AttackAction Attack()
        {
            var result = PlayerActionEvent?.Invoke(true);
            return (AttackAction) result;
            //throw new NotImplementedException();
        }

        public void WinAction()
        {
            throw new NotImplementedException();
        }

        public void LoseAction()
        {
            throw new NotImplementedException();
        }
    }
}
