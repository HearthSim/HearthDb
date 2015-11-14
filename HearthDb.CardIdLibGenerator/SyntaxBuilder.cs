using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using HearthDb.Enums;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HearthDb.CardIdGenerator
{
    internal class SyntaxBuilder
    {
        internal static ClassDeclarationSyntax GetNonCollectible(ClassDeclarationSyntax classDecl)
        {
            foreach(var set in Enum.GetValues(typeof(CardSet)).Cast<CardSet>())
            {
                var anySet = false;
                var setName = Helper.GetSetName(set);
                var cSet = SyntaxFactory.ClassDeclaration(setName).AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                foreach(var c in Enum.GetNames(typeof(CardClass)))
                {
                    var className = c == "DREAM" ? "DreamCards" : CultureInfo.InvariantCulture.TextInfo.ToTitleCase(c.ToLower());
                    var cCard = SyntaxFactory.ClassDeclaration(className).AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                    var anyCards = false;
                    var existing = new Dictionary<string, int>();
                    foreach(var card in
                        Cards.All.Values.Where(
                                               x =>
                                               !x.Collectible && x.Class.ToString().Equals(c) &&
                                               x.Set.ToString().Equals(set.ToString())))
                    {
                        var name = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(card.Name.ToLower());
                        name = Regex.Replace(name, @"[^\w\d]", "");
                        if(Regex.IsMatch(card.Id, @"_\d+[abhHt]?[eo]"))
                            name += "Enchantment";
                        if(Regex.IsMatch(card.Id, @"_\d+[hH]?[t]"))
                            name += "Token";
                        if(Helper.SpecialPrefixes.ContainsKey(card.Id))
                            name += Helper.SpecialPrefixes[card.Id];
                        if(Regex.IsMatch(card.Id, @"_2_TB$"))
                            name += "TavernBrawlHeroPower";
                        else if(Regex.IsMatch(card.Id, @"_TB$"))
                            name += "TavernBrawl";
                        else if(card.Id == "BRM_027h")
                            name += "Hero";
                        else if(card.Id == "BRM_027p")
                            name += "HeroPower";
                        else if((Regex.IsMatch(card.Id, @"_[\dabet]+[hH]") || name.StartsWith("NAX1h")))
                        {
                            if(name.StartsWith("Heroic"))
                                name = name.Substring(6);
                            name += "Heroic";
                        }
                        if(Regex.IsMatch(name, @"^\d"))
                            name = "_" + name;
                        if(!existing.ContainsKey(name))
                            existing.Add(name, 0);
                        if(existing[name]++ > 0)
                            name += existing[name];
                        cCard = cCard.AddMembers(GenerateConst(name, card.Id));
                        anyCards = true;
                    }
                    if(anyCards)
                    {
                        cSet = cSet.AddMembers(cCard);
                        anySet = true;
                    }
                }
                if(anySet)
                    classDecl = classDecl.AddMembers(cSet);
            }
            return classDecl;
        }

        internal static ClassDeclarationSyntax GetCollectible(ClassDeclarationSyntax classDecl)
        {
            foreach(var c in Enum.GetNames(typeof(CardClass)))
            {
                var anyCards = false;
                var className = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(c.ToLower());
                var cCard = SyntaxFactory.ClassDeclaration(className).AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                foreach(var card in
                    Cards.All.Values.Where(x => x.Collectible && x.Class.ToString().Equals(c)))
                {
                    var name = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(card.Name.ToLower());
                    name = Regex.Replace(name, @"[^\w\d]", "");
                    cCard = cCard.AddMembers(GenerateConst(name, card.Id));
                    anyCards = true;
                }
                if(anyCards)
                    classDecl = classDecl.AddMembers(cCard);
            }
            return classDecl;
        }

        internal static FieldDeclarationSyntax GenerateConst(string identifier, string value)
        {
            var assignedValue =
                SyntaxFactory.EqualsValueClause(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression,
                                                                                SyntaxFactory.Literal(value)));
            var declaration =
                SyntaxFactory.SeparatedList(new[]
                {
                    SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(identifier), null, assignedValue)
                });
            return
                SyntaxFactory.FieldDeclaration(SyntaxFactory.VariableDeclaration(SyntaxFactory.ParseTypeName("string"), declaration))
                             .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                             .AddModifiers(SyntaxFactory.Token(SyntaxKind.ConstKeyword));
        }
    }
}