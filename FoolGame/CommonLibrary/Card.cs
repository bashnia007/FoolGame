namespace CommonLibrary
{
    public class Card
    {
        public Suit Suit { get; private set; }
        public Nominal Nominal { get; private set; }

        public Card(Suit suit, Nominal nominal)
        {
            Suit = suit;
            Nominal = nominal;
        }
    }
}
