#region

using HearthDb.Enums;
using System.Linq;
using NUnit.Framework;

#endregion

namespace HearthDb.Tests
{
	[TestFixture]
	public class UnitTest1
	{
		[Test]
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
			Assert.AreEqual(2539, Cards.All["AT_001"].DbfId);
		}

		[Test]
		public void EntourageCardTest()
		{
			var animalCompanion = Cards.Collectible[CardIds.Collectible.Hunter.AnimalCompanion];
			Assert.AreEqual(3, animalCompanion.EntourageCardIds.Length);
			Assert.AreEqual(CardIds.NonCollectible.Hunter.Misha, animalCompanion.EntourageCardIds[0]);
			Assert.AreEqual(CardIds.NonCollectible.Hunter.Leokk, animalCompanion.EntourageCardIds[1]);
		}

		[Test]
		public void EntourageCardTest_AnimalCompanion()
		{
			var animalCompanion = Cards.Collectible[CardIds.Collectible.Hunter.AnimalCompanion];
			Assert.AreEqual(3, animalCompanion.EntourageCardIds.Length);
		}

		[Test]
		public void IgnoreCaseTest()
		{
			var c1 = Cards.GetFromName("Flame Lance", Locale.enUS);
			var c2 = Cards.GetFromName("FLAME LANCE", Locale.enUS);
			var c3 = Cards.GetFromName("flame lance", Locale.enUS);
			var c4 = Cards.GetFromName("FlAmE lAnCe", Locale.enUS);
			Assert.AreNotEqual(c1, c2);
			Assert.AreEqual(c2, c3);
			Assert.AreEqual(c3, c4);
		}

		[Test]
		public void FromDbfIdTest()
		{
			Assert.AreEqual("Flame Lance", Cards.GetFromDbfId(2539).Name);
		}

		[Test]
		public void HeroCardTest()
		{
			Assert.IsTrue(Cards.Collectible.ContainsKey(CardIds.Collectible.Hunter.DeathstalkerRexxar));
			Assert.AreEqual("Deathstalker Rexxar", Cards.GetFromDbfId(43398).Name);
		}

		[Test]
		public void TestMechanics()
		{
			var deadscaleKnight = Cards.Collectible[CardIds.Collectible.Neutral.DeadscaleKnight];
			Assert.IsTrue(deadscaleKnight.Mechanics.Contains("Lifesteal"));
			var giantWasp = Cards.Collectible[CardIds.Collectible.Neutral.GiantWasp];
			Assert.IsTrue(giantWasp.Mechanics.Contains("Poisonous"));
		}
	}
}
