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
	}
}
