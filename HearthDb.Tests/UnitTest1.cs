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
		public void TestCardText()
		{
			var lucentbark = Cards.GetFromDbfId(51796);
			System.Console.WriteLine(lucentbark.Text);
			Assert.IsTrue(lucentbark.Text.Contains("(5 left!)"));

			var janalai = Cards.GetFromDbfId(50088);
			Assert.IsFalse(janalai.Text.Contains("@"));
			Assert.IsTrue(janalai.Text.Contains("If your Hero Power"));

			var galakrond = Cards.GetFromDbfId(57419);
			Assert.IsTrue(galakrond.Text.Contains("Draw 1 card."));
			Assert.IsTrue(galakrond.Text.Contains("It costs (0)."));

            var eyeOfCthun = Cards.All[CardIds.NonCollectible.Neutral.CThuntheShattered_EyeOfCthunToken];
			Assert.IsTrue(eyeOfCthun.Text.Contains("(0/4)"));
			Assert.IsTrue(eyeOfCthun.Text.Contains("7 damage randomly"));

			var cramSession = Cards.All[CardIds.Collectible.Mage.CramSession];
			Assert.IsTrue(cramSession.Text.Contains("Draw $1 card"));
			Assert.IsTrue(cramSession.Text.Contains("improved by"));

			var flameLance = Cards.All[CardIds.Collectible.Mage.FlameLanceTGT];
			Assert.IsTrue(flameLance.GetLocText(Locale.frFR).Contains("$25"));

			var elvenArcher = Cards.All[CardIds.Collectible.Neutral.ElvenArcherVanilla];
			Assert.IsTrue(elvenArcher.Text.Contains("Deal 1 damage"));

			var demonicPortal = Cards.All[CardIds.NonCollectible.Neutral.DemonicPortal];
			Assert.IsTrue(demonicPortal.Text.Contains("30 left!"));

			var summoningRitual = Cards.All[CardIds.NonCollectible.Demonhunter.SummoningRitual2];
			Assert.IsTrue(summoningRitual.Text.Contains("Summon 3 Demons"));
			Assert.IsTrue(summoningRitual.Text.Contains("for 2 turns"));

			var hatredReactor = Cards.All[CardIds.NonCollectible.Warlock.HatredReactorToken];
			Assert.IsTrue(hatredReactor.Text.Contains("give it +1/+1"));

		}

		[TestMethod]
		public void DeflectOBot_HasDivineShield()
		{
			Assert.AreEqual(1, Cards.All[CardIds.NonCollectible.Neutral.DeflectOBot].Entity.GetTag(GameTag.DIVINE_SHIELD));
			Assert.AreEqual(1, Cards.All[CardIds.NonCollectible.Neutral.DeflectOBotTavernBrawl].Entity.GetTag(GameTag.DIVINE_SHIELD));
		}
	}
}
