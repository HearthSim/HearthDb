using System;
using System.Linq;
using HearthDb.Deckstrings;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HearthDb.Tests
{
	[TestClass]
	public class DeckSerializerTestIssue4267
	{
		private const string DeckString = "AAECAf0GBs/SA/zeA87hA/LtA7CKBIWgBAysywPM0gPN0gOO1APG3gPm4QP14wP44wOS5AOCoASDoATnoAQA";

		private const string FullDeckString = @"### Control Warlock
# Class: Warlock
# Format: Standard
# Year of the Gryphon
#
# 2x (1) Armor Vendor
# 2x (1) Spirit Jailer
# 2x (2) Drain Soul
# 2x (2) Soul Shear
# 2x (3) Hysteria
# 2x (3) Luckysoul Hoarder
# 2x (3) School Spirits
# 1x (3) Tamsin Roame
# 2x (4) Cascading Disaster
# 2x (5) Ogremancer
# 2x (5) Siphon Soul
# 1x (6) Tickatus
# 1x (7) Soulciologist Malicia
# 2x (7) Strongman
# 2x (8) Twisting Nether
# 1x (9) Alexstrasza the Life-Binder
# 1x (9) Lord Jaraxxus
# 1x (10) Y'Shaarj, the Defiler
#
AAECAf0GBs/SA/zeA87hA/LtA7CKBIWgBAysywPM0gPN0gOO1APG3gPm4QP14wP44wOS5AOCoASDoATnoAQA


# To use this deck, copy it to your clipboard and create a new deck in Hearthstone";

		[TestMethod]
		public void DeserializeWithComments()
		{
			var deck = DeckSerializer.Deserialize(FullDeckString);
			Assert.AreEqual("Control Warlock", deck.Name);
			Assert.AreEqual(30, deck.GetCards().Values.Sum());
			var serialized = DeckSerializer.Serialize(deck, false);
			Assert.AreEqual(DeckString, serialized);
		}
	}
}
