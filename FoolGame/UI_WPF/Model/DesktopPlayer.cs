using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;

namespace UI_WPF.Model
{
    public class DesktopPlayer : IPlayer
    {
        public DesktopPlayer(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
        public List<Card> Hand { get; set; }
        public IPlayerAction SelectPlayerAction(bool isAttack = false)
        {
            throw new NotImplementedException();
        }

        public AttackAction Attack()
        {
            throw new NotImplementedException();
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
