using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class AddAction : IPlayerAction
    {
        public List<Card> AddedCards { get; }

        public bool AddCards(List<Card> cards)
        {
            if (cards.TrueForAll(c => Table.OpenedCards.Select(o => o.Nominal).Contains(c.Nominal)) &&
                Table.VisiblePlayers.First(p => p.Role == PlayerRole.Passive).CardsCount >= cards.Count &&
                ((!Table.IsFirstRound && Table.AttackCardsCount + cards.Count <= Constants.MaxCardsOnTheTable) ||
                (Table.IsFirstRound && Table.AttackCardsCount + cards.Count <= Constants.MaxCardOnTheTableForFirstRound)))
            {
                AddedCards.AddRange(cards);

                foreach (var card in cards)
                {
                    Player.Hand.Remove(card);
                }
                return true;
            }
            return false;
        }
        public AddAction(IPlayer player)
        {
            AddedCards = new List<Card>();
            Player = player;
        }

        public IPlayer Player { get; set; }
        public ActionType ActionType => ActionType.Add;
    }
}
