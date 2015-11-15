#region

using System.Linq;
using HearthDb.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HearthDb.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void BasicTest()
        {
            Assert.AreEqual("Flame Lance", Cards.All["AT_001"].Name);
            Assert.AreEqual("Flammenlanze", Cards.All["AT_001"].GetLocName(Language.deDE));
            Assert.AreEqual("Nutthapon Petchthai", Cards.All["AT_001"].ArtistName);
            Assert.AreEqual(CardSet.TGT, Cards.All["AT_001"].Set);
            Assert.AreEqual(true, Cards.All["AT_001"].Collectible);
            Assert.AreEqual(Rarity.COMMON, Cards.All["AT_001"].Rarity);
            Assert.AreEqual(CardClass.MAGE, Cards.All["AT_001"].Class);
            Assert.AreEqual(5, Cards.All["AT_001"].Cost);
            Assert.AreEqual(0, Cards.All["AT_001"].Attack);
        }

        [TestMethod]
        public void EntourageCardTest()
        {
            var animalCompanion = Cards.Collectible[CardIds.Collectible.Hunter.AnimalCompanion];
            Assert.AreEqual(3, animalCompanion.EntourageCardIds.Length);
            Assert.AreEqual(CardIds.NonCollectible.Hunter.MishaBasic, animalCompanion.EntourageCardIds[0]);
            Assert.AreEqual(CardIds.NonCollectible.Hunter.LeokkBasic, animalCompanion.EntourageCardIds[1]);
        }

        [TestMethod]
        public void EntourageCardTest_BaneOfDoom()
        {
            var baneOfDoom = Cards.Collectible[CardIds.Collectible.Warlock.BaneOfDoom];
            Assert.AreEqual(6, baneOfDoom.EntourageCardIds.Length);
        }

        [TestMethod]
        public void DbfWrapper_ContainsAllCards()
        {
            foreach(var card in Cards.All.Where(x => x.Value.Set != CardSet.NONE))
                Assert.IsTrue(CardDbfWrapper.Records.Values.Any(v => v.MiniGuid == card.Key), card.Value.Name);
        }
    }
}