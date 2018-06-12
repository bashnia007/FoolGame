namespace CommonLibrary
{
    public class VisiblePlayer
    {
        public VisiblePlayer(IPlayer player)
        {
            Player = player;
        }
        public IPlayer Player {  get; }
        public int Id => Player.Id;
        public int CardsCount => Player.Hand.Count;
        public PlayerRole Role { get; set; }
    }
}
