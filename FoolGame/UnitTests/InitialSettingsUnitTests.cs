using System;
using System.Collections.Generic;
using System.Linq;
using CommonLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class InitialSettingsUnitTests
    {
        [TestMethod]
        public void ShuffleCards_ReturnsCorrectCount()
        {
            var cards = Logic.InitialSettings.ShuffleCards();

            var count = Enum.GetNames(typeof(Nominal)).Length*Enum.GetNames(typeof(Suit)).Length;
            Assert.AreEqual(cards.Count, count);
        }

        [TestMethod]
        public void ShuffleCards_NoDublicates()
        {
            var cards = Logic.InitialSettings.ShuffleCards();

            Assert.IsTrue(cards.All(x => cards.Count(y => x.Suit == y.Suit && x.Nominal == y.Nominal) == 1));
        }

        [TestMethod]
        public void ProvideCards_EachPlayerRecieveCards()
        {
            var cards = Logic.InitialSettings.ShuffleCards();
            var cardsCount = cards.Count;
            var players = new List<Player>
            {
                new Player(1),
                new Player(1),
                new Player(1),
                new Player(1)
            };
            short cardsOnHand = 6;

            Logic.InitialSettings.ProvideCards(players, cards, cardsOnHand);
            // каждый игрок получил по 6 карт
            foreach (var player in players)
            {
                Assert.AreEqual(player.Hand.Count, cardsOnHand);
            }

            // в колоде осталось 24 карты
            Assert.AreEqual(cards.Count, cardsCount - players.Count*cardsOnHand);
        }

        [TestMethod]
        public void ProvideTrump_RemoveCard()
        {
            var cards = Logic.InitialSettings.ShuffleCards();
            var cardsCount = cards.Count;
            var trump = Logic.InitialSettings.ProvideTrump(cards);
            Assert.AreEqual(cardsCount - 1, cards.Count);
        }
    }
}
