using HearthDb.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HearthDb.Tests
{
	[TestClass]
	public class CardsTest
	{
		[TestMethod]
		public void Jaraxxus_AllVersionsAreCollectible()
		{
			Assert.IsTrue(Cards.Collectible.ContainsKey(CardIds.Collectible.Warlock.LordJaraxxus));
			Assert.IsTrue(Cards.Collectible.ContainsKey(CardIds.Collectible.Warlock.LordJaraxxusCore));
		}

		[TestMethod]
		public void ArgentSquire_HasNoRace()
		{
			var squire = Cards.Collectible[CardIds.Collectible.Neutral.ArgentSquire];
			Assert.AreEqual(Race.INVALID, squire.Race);
			Assert.AreEqual(Race.INVALID, squire.SecondaryRace);
		}

		[TestMethod]
		public void MurlocWarleader_HasOneRace()
		{
			var murloc = Cards.Collectible[CardIds.Collectible.Neutral.MurlocWarleader];
			Assert.AreEqual(Race.MURLOC, murloc.Race);
			Assert.AreEqual(Race.INVALID, murloc.SecondaryRace);
		}

		[TestMethod]
		public void UnearthedRaptor_HasTwoRaces()
		{
			var raptor = Cards.All[CardIds.Collectible.Rogue.UnearthedRaptor];
			Assert.AreEqual(Race.UNDEAD, raptor.Race);
			Assert.AreEqual(Race.BEAST, raptor.SecondaryRace);
		}
	}
}
