using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class AttackAction : IPlayerAction
    {
        public List<Card> AttackCards { get; }

        public bool AddCards(List<Card> cards)
        {
            if (cards.GroupBy(c => c.Nominal).Count() == 1 && cards.Count <= Table.VisiblePlayers.First(p => p.Role == PlayerRole.Passive).CardsCount)
            {
                AttackCards.AddRange(cards);
                foreach (var card in cards)
                {
                    Player.Hand.Remove(card);
                }
                return true;
            }
            return false;
        }
        public AttackAction(IPlayer player)
        {
            AttackCards = new List<Card>();
            Player = player;
        }

        public IPlayer Player { get; set; }
        public ActionType ActionType => ActionType.Attack;
    }
}
