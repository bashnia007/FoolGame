using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class TransferAction : IPlayerAction
    {
        public IPlayer Player { get; set; }
        public Card TransferCard { get; set; }
        public ActionType ActionType => ActionType.Transfer;

        public TransferAction(IPlayer player)
        {
            Player = player;
        }

        public bool SelectTransferCard(Card card)
        {
            var passivePlayer = Table.VisiblePlayers.First(p => p.Role == PlayerRole.Passive);
            if (Table.OpenedCards.Count != Table.NotCoveredCards.Count ||
                Table.NotCoveredCards[0].Nominal != card.Nominal ||
                passivePlayer.CardsCount < Table.NotCoveredCards.Count + 1) return false;
            TransferCard = card;
            return true;
        }
    }
}
