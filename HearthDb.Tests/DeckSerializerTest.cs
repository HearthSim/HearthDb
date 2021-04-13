using System;
using System.Linq;
using HearthDb.Deckstrings;
using HearthDb.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HearthDb.Tests
{
	[TestClass]
	public class DeckSerializerTest
	{
		private const string DeckString = "AAECAQcCrwSRvAIOHLACkQP/A44FqAXUBaQG7gbnB+8HgrACiLACub8CAA==";
		private const string ClassicDeckstring = "AAEDAaa4AwTTlQSvlgT6oASPowQN25UE3JUEppYEsJYEtpYEvZYE1JYE3ZYE6aEE8KEE8aEE86EE1KIEAA==";

		private const string FullDeckString = @"### Warrior123
# Class: Warrior
# Format: Standard
# Year of the Mammoth
#
# 2x (1) Upgrade!
# 1x (1) Patches the Pirate
# 2x (1) N'Zoth's First Mate
# 2x (1) Southsea Deckhand
# 2x (2) Heroic Strike
# 2x (2) Bloodsail Raider
# 2x (2) Fiery War Axe
# 2x (3) Frothing Berserker
# 2x (3) Southsea Captain
# 2x (3) Bloodsail Cultist
# 2x (4) Kor'kron Elite
# 2x (4) Mortal Strike
# 2x (4) Naga Corsair
# 2x (4) Dread Corsair
# 2x (5) Arcanite Reaper
# 1x (5) Leeroy Jenkins
#
AAECAQcCrwSRvAIOHLACkQP/A44FqAXUBaQG7gbnB+8HgrACiLACub8CAA==
#
# To use this deck, copy it to your clipboard and create a new deck in Hearthstone";

		[TestMethod]
		public void TestDeckStrings()
		{
			var deck = DeckSerializer.Deserialize(DeckString);
			Assert.AreEqual(CardIds.Collectible.Warrior.GarroshHellscreamHeroHeroSkins, deck.GetHero().Id);
			var cards = deck.GetCards();
			Assert.AreEqual(30, cards.Values.Sum());
			var heroicStroke = cards.FirstOrDefault(c => c.Key.Id == CardIds.Collectible.Warrior.HeroicStrikeLegacy);
			Assert.IsNotNull(heroicStroke);
			Assert.AreEqual(2, heroicStroke.Value);
		}

		[TestMethod]
		public void TestDeckStringsClassic()
		{
			var deck = DeckSerializer.Deserialize(ClassicDeckstring);
			Assert.AreEqual(CardIds.Collectible.Druid.MalfurionStormrage_EliseStarseekerHeroHeroSkins, deck.GetHero().Id);
			var cards = deck.GetCards();
			Assert.AreEqual(30, cards.Values.Sum());
			var claw = cards.FirstOrDefault(c => c.Key.Id == CardIds.Collectible.Druid.ClawVanilla);
			Assert.IsNotNull(claw);
			Assert.AreEqual(1, claw.Value);
			Assert.AreEqual(FormatType.FT_CLASSIC, deck.Format);
		}

		[TestMethod]
		public void TestReserialize()
		{
			var deck = DeckSerializer.Deserialize(DeckString);
			var reserialized = DeckSerializer.Serialize(deck, false);
			Assert.AreEqual(DeckString, reserialized);
		}

		[TestMethod]
		public void TestSerializerComments()
		{
			var deck = DeckSerializer.Deserialize(DeckString);
			deck.Name = "Warrior123";
			deck.ZodiacYear = ZodiacYear.MAMMOTH;

			var commented = DeckSerializer.Serialize(deck, true);
			var lines = commented.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
			Assert.AreEqual("### Warrior123", lines[0]);
			Assert.AreEqual("# Class: Warrior", lines[1]);
			Assert.AreEqual("# Format: Standard", lines[2]);
			Assert.AreEqual("# Year of the Mammoth", lines[3]);
		}

		[TestMethod]
		public void TestSerializerCommentsDefaults()
		{
			var deck = DeckSerializer.Deserialize(DeckString);

			var commented = DeckSerializer.Serialize(deck, true);
			var lines = commented.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
			Assert.AreEqual("### Warrior Deck", lines[0]);
			Assert.AreEqual("# Class: Warrior", lines[1]);
			Assert.AreEqual("# Format: Standard", lines[2]);
		}

		[TestMethod]
		public void DeserializeWithComments()
		{

			var deck = DeckSerializer.Deserialize(FullDeckString);
			Assert.AreEqual("Warrior123", deck.Name);
			Assert.AreEqual(30, deck.GetCards().Values.Sum());
			var serialized = DeckSerializer.Serialize(deck, false);
			Assert.AreEqual(DeckString, serialized);
		}
	}
}
