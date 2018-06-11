namespace CommonLibrary
{
    public class NoneAction : IPlayerAction
    {
        public IPlayer Player { get; set; }
        public ActionType ActionType => ActionType.None;
    }
}
