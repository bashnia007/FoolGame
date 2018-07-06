using CommonLibrary;
using Logic;
using UI_WPF.Models;

namespace UI_WPF.Helpers
{
    public class DesktopPlayerCreator : PlayerCreator
    {
        public override IPlayer CreatePlayer(int id)
        {
            return new DesktopPlayer(id);
        }
    }
}
