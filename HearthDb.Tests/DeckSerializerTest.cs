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
		private const string DeckStringPreSideboards = "AAECAQcCrwSRvAIOHLACkQP/A44FqAXUBaQG7gbnB+8HgrACiLACub8CAA==";
		private const string DeckString = "AAECAQcCrwSRvAIOHLACkQP/A44FqAXUBaQG7gbnB+8HgrACiLACub8CAAA=";
		private const string ClassicDeckstring = "AAEDAaa4AwTTlQSvlgT6oASPowQN25UE3JUEppYEsJYEtpYEvZYE1JYE3ZYE6aEE8KEE8aEE86EE1KIEAA==";
		private const string SideboardedDeckstring = "AAECAbSKAwrbnwSboASjoAS4oASD1ASY1ASZ1ASh1ASxmAX9xAUKh58EiZ8Erp8E2Z8E2p8EuaAE3KAEgdQEo9QEjaQFAAEB8L8E/cQFAdqhBf3EBQA=";

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
AAECAQcCrwSRvAIOHLACkQP/A44FqAXUBaQG7gbnB+8HgrACiLACub8CAAA=
#
# To use this deck, copy it to your clipboard and create a new deck in Hearthstone";

		private const string FullSideboardedDeckString = @"### Quest Druid
# Class: Druid
# Format: Standard
# Year of the Hydra
#
# 2x (0) Innervate
# 2x (0) Pounce
# 1x (1) Lingering Zombie
# 2x (1) Living Roots
# 2x (2) Mark of the Wild
# 2x (2) Plated Beetle
# 2x (2) Power of the Wild
# 1x (2) Wild Pyromancer
# 1x (3) Brann Bronzebeard
# 1x (3) Feral Rage
# 2x (3) Lone Champion
# 2x (3) Mounted Raptor
# 2x (3) Wild Growth
# 1x (4) Dark Iron Dwarf
# 1x (4) E.T.C., Band Manager
#   1x (1) Dozing Kelpkeeper
#   2x (2) Earthen Scales
# 1x (4) Elise Starseeker
# 1x (5) Druid of the Claw
# 2x (5) Nourish
# 1x (6) Cairne Bloodhoof
# 1x (7) Ancient of Lore
# 
AAECAbSKAwrbnwSboASjoAS4oASD1ASY1ASZ1ASh1ASxmAX9xAUKh58EiZ8Erp8E2Z8E2p8EuaAE3KAEgdQEo9QEjaQFAAEB8L8E/cQFAdqhBf3EBQA=
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

			// Deck with Sideboard
			deck = DeckSerializer.Deserialize(SideboardedDeckstring);
			Assert.AreEqual(CardIds.Collectible.Druid.MalfurionStormrage_LunaraHeroHeroSkins, deck.GetHero().Id);
			cards = deck.GetCards();
			Assert.AreEqual(30, cards.Values.Sum());
			var innervate = cards.FirstOrDefault(c => c.Key.Id == CardIds.Collectible.Druid.InnervateCore);
			Assert.IsNotNull(innervate);
			Assert.AreEqual(2, innervate.Value);
			var sideboards = deck.GetSideboards();
			Assert.AreEqual(1, sideboards.Count);
			var sideboardOwner = sideboards.First().Key;
			Assert.AreEqual(CardIds.Collectible.Neutral.ETCBandManager, sideboardOwner.Id);
			var sideboardCards = sideboards.First().Value;
			Assert.AreEqual(3, sideboardCards.Values.Sum());
			var dozingKelpkeeper = sideboardCards.FirstOrDefault(c => c.Key.Id == CardIds.Collectible.Druid.DozingKelpkeeper);
			Assert.IsNotNull(dozingKelpkeeper);
			Assert.AreEqual(1, dozingKelpkeeper.Value);
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
			// Decks with sideboard
			var sideboardedDeck = DeckSerializer.Deserialize(SideboardedDeckstring);
			var sideboardedDeckReserialized = DeckSerializer.Serialize(sideboardedDeck, false);
			Assert.AreEqual(SideboardedDeckstring, sideboardedDeckReserialized);
		}

		[TestMethod]
		public void TestReserializeFromPreSideboardsDeckstring()
		{
			var deck = DeckSerializer.Deserialize(DeckStringPreSideboards);
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

			// Deck with Sideboard
			deck = DeckSerializer.Deserialize(SideboardedDeckstring);
			deck.Name = "Quest Druid";
			deck.ZodiacYear = ZodiacYear.HYDRA;

			commented = DeckSerializer.Serialize(deck, true);
			lines = commented.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
			Assert.AreEqual("### Quest Druid", lines[0]);
			Assert.AreEqual("# Class: Druid", lines[1]);
			Assert.AreEqual("# Format: Standard", lines[2]);
			Assert.AreEqual("# Year of the Hydra", lines[3]);
			Assert.AreEqual("# 1x (4) E.T.C., Band Manager", lines[19]);
			Assert.AreEqual("#   1x (1) Dozing Kelpkeeper", lines[20]);
			Assert.AreEqual("#   2x (2) Earthen Scales", lines[21]);
			Assert.AreEqual("# 1x (4) Elise Starseeker", lines[22]);
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

			// Deck with Sideboard
			deck = DeckSerializer.Deserialize(SideboardedDeckstring);
			commented = DeckSerializer.Serialize(deck, true);
			lines = commented.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
			Assert.AreEqual("### Druid Deck", lines[0]);
			Assert.AreEqual("# Class: Druid", lines[1]);
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

			// Deck with Sideboard
			deck = DeckSerializer.Deserialize(FullSideboardedDeckString);
			Assert.AreEqual("Quest Druid", deck.Name);
			Assert.AreEqual(30, deck.GetCards().Values.Sum());
			Assert.AreEqual(3, deck.GetSideboards().Values.SelectMany(s => s.Values).Sum());
			serialized = DeckSerializer.Serialize(deck, false);
			Assert.AreEqual(SideboardedDeckstring, serialized);
		}
	}
}
