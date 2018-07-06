using CommonLibrary;

namespace UI_WPF.Models
{
    public class PlayerCard : Card
    {
        public PlayerCard(Suit suit, Nominal nominal, bool isValid) : base(suit, nominal)
        {
            IsValid = isValid;
        }

        public bool IsValid { get; set; }
    }
}
