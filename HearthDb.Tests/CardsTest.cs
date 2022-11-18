using HearthDb.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

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
		public void CoreSet_HeroesAreCollectible()
		{
			// 9 class heroes + jaraxxus
			Assert.AreEqual(10, Cards.Collectible.Values.Count(x => x.Type == CardType.HERO && x.Set == CardSet.CORE));

			Assert.IsTrue(Cards.Collectible.ContainsKey(CardIds.Collectible.Druid.MalfurionThePestilentCore));
			Assert.IsTrue(Cards.Collectible.ContainsKey(CardIds.Collectible.Mage.FrostLichJainaCore));
		}
	}
}
