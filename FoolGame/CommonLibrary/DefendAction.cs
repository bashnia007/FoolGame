using System.Collections.Generic;

namespace CommonLibrary
{
    public class DefendAction : PlayerAction
    {
        public List<CardsPair> CardsPairs { get; set; }
        public bool AddPair(CardsPair cardsPair)
        {
            
            if ((cardsPair.AttackCard.Nominal < cardsPair.DefendCard.Nominal && cardsPair.AttackCard.Suit == cardsPair.DefendCard.Suit) ||
                (cardsPair.DefendCard.Suit == Table.Trump && cardsPair.DefendCard.Suit != Table.Trump))
            {
                CardsPairs.Add(cardsPair);
                return true;
            }
            return false;
        }
    }
}
