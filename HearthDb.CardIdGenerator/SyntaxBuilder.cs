#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using HearthDb.Enums;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static HearthDb.Enums.CardType;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

#endregion

namespace HearthDb.CardIdGenerator
{
	internal class SyntaxBuilder
	{
		private static readonly Regex MemberNameRegex = new Regex(@"[^\w\d]");
		internal static List<(string, ClassDeclarationSyntax)> GetNonCollectible()
		{
			var cards = Cards.All.Values.Where(x => !x.Collectible && x.Name != null).OrderBy(x => x.Set).ThenBy(x => x.Id).ToList();
			var conflicts = new Dictionary<string, List<string>>();
			while(true)
			{
				var classDecls = new List<(string, ClassDeclarationSyntax)>();
				var newNamingConflicts = new Dictionary<string, List<string>>();
				foreach(var cardClass in CardClasses)
				{
					var className = cardClass == CardClass.DREAM ? "DreamCards" : CultureInfo.InvariantCulture.TextInfo.ToTitleCase(cardClass.ToString().ToLower());
					Console.WriteLine($"> Generating NonCollectible.{className}");
					var classDecl = ClassDeclaration("NonCollectible").AddModifiers(Token(PublicKeyword), Token(PartialKeyword));
					var cCard = ClassDeclaration(className).AddModifiers(Token(PublicKeyword));
					var members = new List<MemberDeclarationSyntax>();
					foreach(var card in cards.Where(x => x.Class == cardClass))
					{
						var name = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(card.Name.ToLower());
						name = NameOverrides(card, name);
						name = MemberNameRegex.Replace(name, "");
						name = ResolveNameFromId(card, name);
						name = ResolveNamingConflict(name, card, newNamingConflicts, className, conflicts);
						members.Add(GenerateConst(name, card.Id).WithLeadingTrivia(GetCardDescription(card)));
					}

					if (members.Any())
					{
						cCard = cCard.AddMembers(members.ToArray());
						classDecl = classDecl.AddMembers(cCard);
						classDecls.Add(($"NonCollectible.{className}", classDecl));
					}
				}
				if(!newNamingConflicts.Any(x => x.Value.Count > 1))
					return classDecls;

				foreach(var pair in newNamingConflicts.Where(x => x.Value.Count > 1).ToDictionary(pair => pair.Key, pair => pair.Value))
					conflicts.Add(pair.Key, pair.Value);

				Console.WriteLine($"NonCollectible New Conflicts: {newNamingConflicts.Sum(x => x.Value.Count)}, Total Unique: {conflicts.Sum(x => x.Value.Count)}");
			}
		}

		public static string NameOverrides(Card card, string name)
		{
			if(card.Id == "GILA_BOSS_66p")
				return "DotDotDot";
			if (card.Id == "THD_040")
				return "EliseStarseekerTavernBrawl2";
			if (card.Id == "THD_042")
				return "BrannBronzebeardTavernBrawl2";
			if(name == "???")
				return "QuestionQuestionQuestion";
			return name;
		}

		private static string ResolveNameFromId(Card card, string name)
		{
			var baseId = Helper.GetBaseId(card.Id);
			if(baseId != card.Id)
			{
				Card baseCard;
				if(Cards.All.TryGetValue(baseId, out baseCard))
				{
					var tmpName = Regex.Replace(baseCard.Name, @"[^\w\d]", "");
					if(Regex.IsMatch(card.Id, @"_[\dabet]+[hH]"))
					{
						if(tmpName.StartsWith("Heroic"))
							tmpName = tmpName.Substring(6);
						tmpName += "Heroic";
					}
					name = tmpName + "_" + name;
				}
			}
			if(card.Set == Enums.CardSet.HERO_SKINS)
				name += "HeroSkins";

			if(card.Set == Enums.CardSet.VANILLA)
				name += "Vanilla";
			if(card.Set == Enums.CardSet.CORE)
				name += "Core";
			if ((int)card.Set == 1810) // 1810 is used temporarily for CORE cards before or after they rotate around the standard year rotation
				name += "CorePlaceholder";
			if (card.Set == Enums.CardSet.LEGACY)
				name += "Legacy";

			if(Regex.IsMatch(card.Id, @"_\d+[abhHt]?[eo]"))
				name += "Enchantment";
			if(Regex.IsMatch(card.Id, @"_\d+[hH]?[t]"))
				name += "Token";
			if(Helper.SpecialPrefixes.ContainsKey(card.Id))
				name += Helper.SpecialPrefixes[card.Id];
			if(Regex.IsMatch(card.Id, @"_2_TB$"))
				name += "TavernBrawlHeroPower";
			else if(Regex.IsMatch(card.Id, @"_TB$") || card.Id.StartsWith("TB"))
				name += "TavernBrawl";
			else if(card.Id == "BRM_027h")
				name += "Hero";
			else if(card.Id == "BRM_027p")
				name += "HeroPower";
			else if(Regex.IsMatch(card.Id, @"_[\dabet]+[hH]") && !(name.Contains("_") && name.Split('_')[0].Contains("Heroic")))
			{
				if(name.StartsWith("Heroic"))
					name = name.Substring(6);
				name += "Heroic";
			}
			if(Regex.IsMatch(name, @"^\d"))
				name = "_" + name;
			return name;
		}

		private static string ResolveNamingConflict(string name, Card card, Dictionary<string, List<string>> newNamingConflicts, string className, Dictionary<string, List<string>> conflicts)
		{
			List<string> conflictingIds;
			if(conflicts.TryGetValue(name + Helper.GetSetAbbreviation(card.Set), out conflictingIds) && conflictingIds.Contains(card.Id))
				name += Helper.GetSetAbbreviation(card.Set) + (conflictingIds.IndexOf(card.Id) + 1);
			else if(conflicts.TryGetValue(name, out conflictingIds))
			{
				if(conflictingIds.Any(x => x.Substring(0, 3) != card.Id.Substring(0, 3)))
					name += Helper.GetSetAbbreviation(card.Set);
				else
					name += (conflictingIds.IndexOf(card.Id) + 1).ToString();
			}
			else if(className == name)
				name += Helper.GetSetAbbreviation(card.Set);
			List<string> ids;
			if(!newNamingConflicts.TryGetValue(name, out ids))
			{
				ids = new List<string>();
				newNamingConflicts.Add(name, ids);
			}
			ids.Add(card.Id);
			return name;
		}

		internal static List<(string, ClassDeclarationSyntax)> GetCollectible()
		{
			var conflicts = new Dictionary<string, List<string>>();
			var cards = Cards.All.Values.Where(x => x.Collectible && x.Name != null).ToList();
			while(true)
			{
				var classDecls = new List<(string, ClassDeclarationSyntax)>();
				var newNamingConflicts = new Dictionary<string, List<string>>();
				foreach(var cardClass in CardClasses)
				{
					var className = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(cardClass.ToString().ToLower());
					Console.WriteLine($"> Generating Collectible.{className}");
					var classDecl = ClassDeclaration("Collectible").AddModifiers(Token(PublicKeyword), Token(PartialKeyword));
					var cCard = ClassDeclaration(className).AddModifiers(Token(PublicKeyword));
					var members = new List<MemberDeclarationSyntax>();
					foreach(var card in cards.Where(x => x.Class == cardClass))
					{
						var name = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(card.Name.ToLower());
						name = NameOverrides(card, name);
						name = MemberNameRegex.Replace(name, "");
						if (card.Id.StartsWith("HERO"))
							name += "Hero";
						name = ResolveNameFromId(card, name);
						name = ResolveNamingConflict(name, card, newNamingConflicts, className, conflicts);
						members.Add(GenerateConst(name, card.Id).WithLeadingTrivia(GetCardDescription(card)));
					}
					if (members.Any())
					{
						cCard = cCard.AddMembers(members.ToArray());
						classDecl = classDecl.AddMembers(cCard);
						classDecls.Add(($"Collectible.{className}", classDecl));
					}
				}
				if(!newNamingConflicts.Any(x => x.Value.Count > 1))
					return classDecls;
				foreach(var pair in newNamingConflicts.Where(x => x.Value.Count > 1).ToDictionary(pair => pair.Key, pair => pair.Value))
					conflicts.Add(pair.Key, pair.Value);
				Console.WriteLine($"Collectible New Conflicts: {newNamingConflicts.Sum(x => x.Value.Count)}, Total Unique: {conflicts.Sum(x => x.Value.Count)}");
			}
		}

		private static IEnumerable<CardClass> CardClasses
			=> new[] { CardClass.NEUTRAL }.Concat(Enum.GetValues(typeof(CardClass)).Cast<CardClass>()).Distinct();
		
		internal static FieldDeclarationSyntax GenerateConst(string identifier, string value)
		{
			var assignedValue = EqualsValueClause(LiteralExpression(StringLiteralExpression, Literal(value)));
			var declaration = SeparatedList(new[] {VariableDeclarator(Identifier(identifier), null, assignedValue)});
			return
				FieldDeclaration(VariableDeclaration(ParseTypeName("string"), declaration))
					.AddModifiers(Token(PublicKeyword))
					.AddModifiers(Token(ConstKeyword));
		}

		private static readonly Regex CardTextTags = new Regex(@"</?[iIbB]/?>");
		private static readonly Regex CardTextWhitespace = new Regex(@"\s+");

		private static SyntaxTriviaList GetCardDescription(Card card)
		{
			var lines = new List<string> { GetText(card), GetInfo(card) };

			if (Cards.NormalToTripleCardIds.TryGetValue(card.Id, out var tripleId))
			{
				lines.Insert(0, "Normal (this):");
				var triple = Cards.All[tripleId];
				lines.Add("--------------------");
				lines.Add("Triple:");
				lines.Add(GetText(triple));
				lines.Add(GetInfo(triple));
			}
			else if (Cards.TripleToNormalCardIds.TryGetValue(card.Id, out var normalId))
			{
				lines.Insert(0, "Triple (this):");
				var normal = Cards.All[normalId];
				lines.Add("--------------------");
				lines.Add("Normal:");
				lines.Add(GetText(normal));
				lines.Add(GetInfo(normal));
			}

			var summary = string.Join("<br/>\n/// ", lines);
			return ParseLeadingTrivia($"/// <summary>\n/// {summary}\n/// </summary>\n");

			string PrettyEnum(object value) => string.Join("", value.ToString().Split("_").Select(TitleCase));
			string TitleCase(string value) => CultureInfo.InvariantCulture.TextInfo.ToTitleCase(value.ToLower());

			bool HasCost(Card c)
			{
				if(c.Entity.GetTag(GameTag.HIDE_COST) > 0)
					return false;
				return c.Type switch
				{
					INVALID => false,
					GAME => false,
					PLAYER => false,
					ENCHANTMENT => false,
					ITEM => false,
					BLANK => false,
					GAME_MODE_BUTTON => false,
					MOVE_MINION_HOVER_TARGET => false,
					BATTLEGROUND_QUEST_REWARD => false,
					BATTLEGROUND_ANOMALY => false,
					BATTLEGROUND_CLICKABLE_BUTTON => false,
					_ => true,
				};
			}

			string GetText(Card c)
			{
				if(c.Text == null)
					return "(No Text)";
				var text = c.Text.Replace('\n', ' ').Replace("[x]", "").Replace(" ", " ").Replace("&", "&amp;");
				text = CardTextTags.Replace(text, "");
				return CardTextWhitespace.Replace(text, " ").Trim();
			}

			string GetInfo(Card c)
			{
				var info = new List<string>();

				if (c.TechLevel > 0)
					info.Add($"Tier-{c.TechLevel}");

				if (!(c.Type == MINION && c.TechLevel > 0) && HasCost(c))
					info.Add($"{c.Cost}-Cost");

				if (c.Type == MINION)
					info.Add($"{c.Attack}/{c.Health}");

				if (c.Race != Race.INVALID)
				{
					var raceStr = $"{PrettyEnum(c.Race)}";
					if (c.SecondaryRace != Race.INVALID)
						raceStr += $"/{PrettyEnum(c.SecondaryRace)}";
					info.Add(raceStr);
				}

				if (c.Type != INVALID)
					info.Add(PrettyEnum(c.Type));
				
				return string.Join(" ", info);
			}
		}
	}
}
