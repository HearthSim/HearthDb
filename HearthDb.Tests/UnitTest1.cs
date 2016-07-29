#region

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
			Assert.AreEqual("Flammenlanze", Cards.All["AT_001"].GetLocName(Locale.deDE));
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
			Assert.AreEqual(CardIds.NonCollectible.Hunter.Misha, animalCompanion.EntourageCardIds[0]);
			Assert.AreEqual(CardIds.NonCollectible.Hunter.Leokk, animalCompanion.EntourageCardIds[1]);
		}

		[TestMethod]
		public void EntourageCardTest_AnimalCompanion()
		{
			var animalCompanion = Cards.Collectible[CardIds.Collectible.Hunter.AnimalCompanion];
			Assert.AreEqual(3, animalCompanion.EntourageCardIds.Length);
		}

		[TestMethod]
		public void IgnoreCaseTest()
		{
			var c1 = Cards.GetFromName("Flame Lance", Locale.enUS);
			var c2 = Cards.GetFromName("FLAME LANCE", Locale.enUS);
			var c3 = Cards.GetFromName("flame lance", Locale.enUS);
			var c4 = Cards.GetFromName("FlAmE lAnCe", Locale.enUS);
			Assert.AreEqual(c1, c2);
			Assert.AreEqual(c2, c3);
			Assert.AreEqual(c3, c4);
		}
	}
}