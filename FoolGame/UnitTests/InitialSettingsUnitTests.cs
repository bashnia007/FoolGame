using System;
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
    }
}
