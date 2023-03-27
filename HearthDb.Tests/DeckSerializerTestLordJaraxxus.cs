using System;
using System.Linq;
using HearthDb.Deckstrings;
using HearthDb.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HearthDb.Tests
{
	[TestClass]
	public class DeckSerializerTestLordJaraxxus
	{
		private const string DeckString = "AAECAfqUAwSR6AOFoATmvQSY1AQNwPkD0PkDg/sDsZ8EhKAEoaAE56AEuLQE3L0E4r0E9c4Em9QEo9QEAAA=";
		private const string WildDeckstring = "AAEBAfqUAwSR6AOFoATmvQSY1AQNwPkD0PkDg/sDsZ8EhKAEoaAE56AEuLQE3L0E4r0E9c4Em9QEo9QEAA==";
		private const string ClassicDeckstring = "AAEDAfqUAwKlogTUogQO+5UEs5YEtJYEu5YE1ZYE2ZYE65YE7ZYE9JYEgaEEw6EE06EEo6IEw6MEAA==";

		private const string FullDeckString = @"### Core Warlock
# Class: Warlock
# Format: Standard
# Year of the Hydra
#
# 2x (1) Emerald Skytalon
# 2x (1) Flame Imp
# 2x (1) Mistress of Mixtures
# 2x (1) Mortal Coil
# 2x (1) Touch of the Nathrezim
# 2x (2) Drain Soul
# 2x (2) Loot Hoarder
# 2x (2) Plated Beetle
# 1x (3) Brann Bronzebeard
# 2x (3) Dragged Below
# 1x (3) Hog Rancher
# 2x (3) Rustrot Viper
# 2x (3) Sira'kess Cultist
# 2x (4) Immolate
# 2x (4) Spice Bread Baker
# 1x (5) Za'qul
# 1x (9) Lord Jaraxxus
# 
AAECAfqUAwSR6AOFoATmvQSY1AQNwPkD0PkDg/sDsZ8EhKAEoaAE56AEuLQE3L0E4r0E9c4Em9QEo9QEAAA=
# 
# To use this deck, copy it to your clipboard and create a new deck in Hearthstone
";

		[TestMethod]
		public void TestDeckStrings()
		{
			var deck = DeckSerializer.Deserialize(DeckString);
			Assert.AreEqual(CardIds.Collectible.Warlock.Guldan_MechaJaraxxusHeroHeroSkins, deck.GetHero().Id);
			var cards = deck.GetCards();
			Assert.AreEqual(30, cards.Values.Sum());
			var lordJaraxxus = cards.FirstOrDefault(c => c.Key.Id == CardIds.Collectible.Warlock.LordJaraxxusCore);
			Assert.IsNotNull(lordJaraxxus);
			Assert.AreEqual(1, lordJaraxxus.Value);
		}

		[TestMethod]
		public void TestDeckStringsWild()
		{
			var deck = DeckSerializer.Deserialize(WildDeckstring);
			Assert.AreEqual(CardIds.Collectible.Warlock.Guldan_MechaJaraxxusHeroHeroSkins, deck.GetHero().Id);
			var cards = deck.GetCards();
			Assert.AreEqual(30, cards.Values.Sum());
			var lordJaraxxus = cards.FirstOrDefault(c => c.Key.Id == CardIds.Collectible.Warlock.LordJaraxxusCore);
			Assert.IsNotNull(lordJaraxxus);
			Assert.AreEqual(1, lordJaraxxus.Value);
			Assert.AreEqual(FormatType.FT_WILD, deck.Format);
		}

		[TestMethod]
		public void TestDeckStringsClassic()
		{
			var deck = DeckSerializer.Deserialize(ClassicDeckstring);
			Assert.AreEqual(CardIds.Collectible.Warlock.Guldan_MechaJaraxxusHeroHeroSkins, deck.GetHero().Id);
			var cards = deck.GetCards();
			Assert.AreEqual(30, cards.Values.Sum());
			var lordJaraxxus = cards.FirstOrDefault(c => c.Key.Id == CardIds.Collectible.Warlock.LordJaraxxusVanilla);
			Assert.IsNotNull(lordJaraxxus);
			Assert.AreEqual(1, lordJaraxxus.Value);
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
			deck.Name = "Warlock123";
			deck.ZodiacYear = ZodiacYear.MAMMOTH;

			var commented = DeckSerializer.Serialize(deck, true);
			var lines = commented.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
			Assert.AreEqual("### Warlock123", lines[0]);
			Assert.AreEqual("# Class: Warlock", lines[1]);
			Assert.AreEqual("# Format: Standard", lines[2]);
			Assert.AreEqual("# Year of the Mammoth", lines[3]);
		}

		[TestMethod]
		public void TestSerializerCommentsDefaults()
		{
			var deck = DeckSerializer.Deserialize(DeckString);

			var commented = DeckSerializer.Serialize(deck, true);
			var lines = commented.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
			Assert.AreEqual("### Warlock Deck", lines[0]);
			Assert.AreEqual("# Class: Warlock", lines[1]);
			Assert.AreEqual("# Format: Standard", lines[2]);
		}

		[TestMethod]
		public void DeserializeWithComments()
		{

			var deck = DeckSerializer.Deserialize(FullDeckString);
			Assert.AreEqual("Core Warlock", deck.Name);
			Assert.AreEqual(30, deck.GetCards().Values.Sum());
			var serialized = DeckSerializer.Serialize(deck, false);
			Assert.AreEqual(DeckString, serialized);
		}
	}
}
