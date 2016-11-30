#region

using System.Collections.Generic;

#endregion

namespace HearthDb.Enums
{
	public static class Dictionaries
	{
		public static Dictionary<GameTag, string> TagNames = new Dictionary<GameTag, string>
		{
			{GameTag.TRIGGER_VISUAL, "TriggerVisual"},
			{GameTag.HEALTH, "Health"},
			{GameTag.ATK, "Atk"},
			{GameTag.COST, "Cost"},
			{GameTag.ELITE, "Elite"},
			{GameTag.CARD_SET, "CardSet"},
			{GameTag.CARDTEXT_INHAND, "CardTextInHand"},
			{GameTag.CARDNAME, "CardName"},
			{GameTag.DURABILITY, "Durability"},
			{GameTag.WINDFURY, "Windfury"},
			{GameTag.TAUNT, "Taunt"},
			{GameTag.STEALTH, "Stealth"},
			{GameTag.SPELLPOWER, "Spellpower"},
			{GameTag.DIVINE_SHIELD, "Divine Shield"},
			{GameTag.CHARGE, "Charge"},
			{GameTag.CLASS, "Class"},
			{GameTag.CARDRACE, "Race"},
			{GameTag.FACTION, "Faction"},
			{GameTag.RARITY, "Rarity"},
			{GameTag.CARDTYPE, "CardType"},
			{GameTag.FREEZE, "Freeze"},
			{GameTag.ENRAGED, "Enrage"},
			{GameTag.RECALL, "Recall"},
			{GameTag.DEATHRATTLE, "Deathrattle"},
			{GameTag.BATTLECRY, "Battlecry"},
			{GameTag.SECRET, "Secret"},
			{GameTag.COMBO, "Combo"},
			{GameTag.CANT_BE_DAMAGED, "Cant Be Damaged"},
			{GameTag.AttackVisualType, "AttackVisualType"},
			{GameTag.CardTextInPlay, "CardTextInPlay"},
			{GameTag.DevState, "DevState"},
			{GameTag.MORPH, "Morph"},
			{GameTag.COLLECTIBLE, "Collectible"},
			{GameTag.TARGETING_ARROW_TEXT, "TargetingArrowText"},
			{GameTag.ENCHANTMENT_BIRTH_VISUAL, "EnchantmentBirthVisual"},
			{GameTag.ENCHANTMENT_IDLE_VISUAL, "EnchantmentIdleVisual"},
			{GameTag.InvisibleDeathrattle, "InvisibleDeathrattle"},
			{GameTag.TAG_ONE_TURN_EFFECT, "OneTurnEffect"},
			{GameTag.SILENCE, "Silence"},
			{GameTag.COUNTER, "Counter"},
			{GameTag.ARTISTNAME, "ArtistName"},
			{GameTag.ImmuneToSpellpower, "ImmuneToSpellpower"},
			{GameTag.ADJACENT_BUFF, "AdjacentBuff"},
			{GameTag.FLAVORTEXT, "FlavorText"},
			{GameTag.HealTarget, "HealTarget"},
			{GameTag.AURA, "Aura"},
			{GameTag.POISONOUS, "Poisonous"},
			{GameTag.HOW_TO_EARN, "HowToGetThisCard"},
			{GameTag.HOW_TO_EARN_GOLDEN, "HowToGetThisGoldCard"},
			{GameTag.AI_MUST_PLAY, "AIMustPlay"},
			{GameTag.AFFECTED_BY_SPELL_POWER, "AffectedBySpellPower"},
			{GameTag.SPARE_PART, "SparePart"}
		};

		public static Dictionary<GameTag, string> Mechanics = new Dictionary<GameTag, string>
		{
			{GameTag.WINDFURY, "Windfury"},
			{GameTag.TAUNT, "Taunt"},
			{GameTag.STEALTH, "Stealth"},
			{GameTag.SPELLPOWER, "Spellpower"},
			{GameTag.DIVINE_SHIELD, "Divine Shield"},
			{GameTag.CHARGE, "Charge"},
			{GameTag.FREEZE, "Freeze"},
			{GameTag.ENRAGED, "Enrage"},
			{GameTag.DEATHRATTLE, "Deathrattle"},
			{GameTag.BATTLECRY, "Battlecry"},
			{GameTag.SECRET, "Secret"},
			{GameTag.COMBO, "Combo"},
			{GameTag.SILENCE, "Silence"},
			{GameTag.ImmuneToSpellpower, "ImmuneToSpellpower"}
		};

		public static Dictionary<GameTag, string> ReferencedMechanics = new Dictionary<GameTag, string> {{GameTag.TREASURE, "Discover"}};

		public static Dictionary<GameTag, System.Type> TagTypes = new Dictionary<GameTag, System.Type>
		{
			{GameTag.TRIGGER_VISUAL, typeof(bool)},
			{GameTag.ELITE, typeof(bool)},
			{GameTag.CARD_SET, typeof(CardSet)},
			{GameTag.CARDTEXT_INHAND, typeof(string)},
			{GameTag.CARDNAME, typeof(string)},
			{GameTag.WINDFURY, typeof(bool)},
			{GameTag.TAUNT, typeof(bool)},
			{GameTag.STEALTH, typeof(bool)},
			{GameTag.SPELLPOWER, typeof(bool)},
			{GameTag.DIVINE_SHIELD, typeof(bool)},
			{GameTag.CHARGE, typeof(bool)},
			{GameTag.CLASS, typeof(CardClass)},
			{GameTag.CARDRACE, typeof(Race)},
			{GameTag.FACTION, typeof(Faction)},
			{GameTag.RARITY, typeof(Rarity)},
			{GameTag.CARDTYPE, typeof(CardType)},
			{GameTag.FREEZE, typeof(bool)},
			{GameTag.ENRAGED, typeof(bool)},
			{GameTag.DEATHRATTLE, typeof(bool)},
			{GameTag.BATTLECRY, typeof(bool)},
			{GameTag.SECRET, typeof(bool)},
			{GameTag.COMBO, typeof(bool)},
			{GameTag.CANT_BE_DAMAGED, typeof(bool)},
			{GameTag.CardTextInPlay, typeof(string)},
			{GameTag.MORPH, typeof(bool)},
			{GameTag.COLLECTIBLE, typeof(bool)},
			{GameTag.TARGETING_ARROW_TEXT, typeof(string)},
			{GameTag.ENCHANTMENT_BIRTH_VISUAL, typeof(EnchantmentVisual)},
			{GameTag.ENCHANTMENT_IDLE_VISUAL, typeof(EnchantmentVisual)},
			{GameTag.InvisibleDeathrattle, typeof(bool)},
			{GameTag.TAG_ONE_TURN_EFFECT, typeof(bool)},
			{GameTag.SILENCE, typeof(bool)},
			{GameTag.COUNTER, typeof(bool)},
			{GameTag.ARTISTNAME, typeof(string)},
			{GameTag.ImmuneToSpellpower, typeof(bool)},
			{GameTag.ADJACENT_BUFF, typeof(bool)},
			{GameTag.FLAVORTEXT, typeof(bool)},
			{GameTag.HealTarget, typeof(bool)},
			{GameTag.AURA, typeof(bool)},
			{GameTag.POISONOUS, typeof(bool)},
			{GameTag.HOW_TO_EARN, typeof(string)},
			{GameTag.HOW_TO_EARN_GOLDEN, typeof(string)},
			{GameTag.AI_MUST_PLAY, typeof(bool)},
			{GameTag.AFFECTED_BY_SPELL_POWER, typeof(bool)},
			{GameTag.SPARE_PART, typeof(bool)}
		};
	}
}
