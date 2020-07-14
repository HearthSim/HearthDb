using HearthDb.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

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
			Assert.AreEqual(2539, Cards.All["AT_001"].DbfId);
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

		[TestMethod]
		public void FromDbfIdTest()
		{
			Assert.AreEqual("Flame Lance", Cards.GetFromDbfId(2539).Name);
		}

		[TestMethod]
		public void HeroCardTest()
		{
			Assert.IsTrue(Cards.Collectible.ContainsKey(CardIds.Collectible.Hunter.DeathstalkerRexxar));
			Assert.AreEqual("Deathstalker Rexxar", Cards.GetFromDbfId(43398).Name);
		}

		[TestMethod]
		public void TestMechanics()
		{
			var deadscaleKnight = Cards.Collectible[CardIds.Collectible.Neutral.DeadscaleKnight];
			Assert.IsTrue(deadscaleKnight.Mechanics.Contains("Lifesteal"));
			var giantWasp = Cards.Collectible[CardIds.Collectible.Neutral.GiantWasp];
			Assert.IsTrue(giantWasp.Mechanics.Contains("Poisonous"));
		}

		[TestMethod]
		public void TextCardText()
		{
			var lucentbark = Cards.GetFromDbfId(51796, false);
			System.Console.WriteLine(lucentbark.Text);
			Assert.IsTrue(lucentbark.Text.Contains("(5 left!)"));

			var janalai = Cards.GetFromDbfId(50088);
			Assert.IsFalse(janalai.Text.Contains("@"));
			Assert.IsTrue(janalai.Text.Contains("If your Hero Power"));

			var galakrond = Cards.GetFromDbfId(57419);
			Assert.IsTrue(galakrond.Text.Contains("Draw 1 card."));
			Assert.IsTrue(galakrond.Text.Contains("It costs (1)."));
		}

		[TestMethod]
		public void DeflectOBot_HasDivineShield()
		{
			Assert.AreEqual(1, Cards.All[CardIds.NonCollectible.Neutral.DeflectOBot].Entity.GetTag(GameTag.DIVINE_SHIELD));
			Assert.AreEqual(1, Cards.All[CardIds.NonCollectible.Neutral.DeflectOBotTavernBrawl].Entity.GetTag(GameTag.DIVINE_SHIELD));
		}
	}
}
