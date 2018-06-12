using System.Collections.Generic;

namespace CommonLibrary
{
    public class DefendAction : IPlayerAction
    {
        public DefendAction(IPlayer player)
        {
            CardsPairs = new List<CardsPair>();
            Player = player;
        }
        public List<CardsPair> CardsPairs { get; set; }
        public IPlayer Player { get; set; }
        public ActionType ActionType => ActionType.Defend;
        public bool AddPair(CardsPair cardsPair)
        {
            if ((cardsPair.AttackCard.Nominal < cardsPair.DefendCard.Nominal && cardsPair.AttackCard.Suit == cardsPair.DefendCard.Suit) ||
                (cardsPair.DefendCard.Suit == Table.Trump && cardsPair.AttackCard.Suit != Table.Trump))
            {
                CardsPairs.Add(cardsPair);
                return true;
            }
            return false;
        }
    }
}
