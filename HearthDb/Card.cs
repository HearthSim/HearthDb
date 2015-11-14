#region

using System;
using System.Linq;
using HearthDb.CardDefs;
using HearthDb.Enums;

#endregion

namespace HearthDb
{
    public class Card
    {
        private const int GuidLength = 36;
        public readonly Entity Entity;

        internal Card(Entity entity)
        {
            Entity = entity;
        }

        public string Id => Entity.CardId;

        public string Name => GetLocName(DefaultLanguage);

        public string Text => GetLocText(DefaultLanguage);

        public string FlavorText => GetLocFlavorText(DefaultLanguage);

        public CardClass Class => (CardClass)Entity.GetTag(GameTag.CLASS);

        public Rarity Rarity => (Rarity)Entity.GetTag(GameTag.RARITY);

        public CardType Type => (CardType)Entity.GetTag(GameTag.CARDTYPE);

        public Race Race => (Race)Entity.GetTag(GameTag.CARDRACE);

        public CardSet Set => (CardSet)Entity.GetTag(GameTag.CARD_SET);

        public Faction Faction => (Faction)Entity.GetTag(GameTag.FACTION);

        public int Cost => Entity.GetTag(GameTag.COST);

        public int Attack => Entity.GetTag(GameTag.ATK);

        public int Health => Entity.GetTag(GameTag.HEALTH);

        public int Durability => Entity.GetTag(GameTag.DURABILITY);

        public string[] Mechanics
        {
            get
            {
                var mechanics =
                    Dictionaries.Mechanics.Keys.Where(mechanic => Entity.GetTag(mechanic) > 0).Select(x => Dictionaries.Mechanics[x]);
                var refMechanics =
                    Dictionaries.ReferencedMechanics.Keys.Where(mechanic => Entity.GetTag(mechanic) > 0)
                                .Select(x => Dictionaries.ReferencedMechanics[x]);
                return mechanics.Concat(refMechanics).ToArray();
            }
        }

        public string ArtistName => Entity.GetInnerValue(GameTag.ARTISTNAME);

        public string[] EntourageCardIds
        {
            get
            {
                return
                    Entity.EntourageCards.Select(
                                                 x =>
                                                 x.CardId.Length == GuidLength ? CardDbfWrapper.Records[x.CardId].MiniGuid : x.CardId)
                          .ToArray();
            }
        }

        public Language DefaultLanguage { get; set; } = Language.enUS;

        public bool Collectible => Convert.ToBoolean(Entity.GetTag(GameTag.Collectible));

        public string GetLocName(Language lang)
        {
            return Entity.GetLocString(GameTag.CARDNAME, lang);
        }

        public string GetLocText(Language lang)
        {
            return Entity.GetLocString(GameTag.CARDTEXT_INHAND, lang)?.Trim();
        }

        public string GetLocFlavorText(Language lang)
        {
            return Entity.GetLocString(GameTag.FLAVORTEXT, lang);
        }
    }
}