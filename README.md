# HearthDb

[![Build & Deploy](https://github.com/HearthSim/HearthDb/actions/workflows/main.yml/badge.svg)](https://github.com/HearthSim/HearthDb/actions/workflows/main.yml)


HearthDb is a .NET Hearthstone database.

## Deckstrings
`HearthDb.Deckstrings.DeckSerializer` can serialize decks from and to Hearthstones deck string format, used for importing/exporting decks.  

See [here](https://github.com/HearthSim/HearthDb/blob/master/HearthDb.Tests/DeckSerializerTest.cs) for example usage.

Full deck strings documentation can be found here: https://hearthsim.info/docs/deckstrings/

## Cards
`HearthDb.Cards` contains all card information of the `CardDefs.xml`, imported from the [hsdata HearthSim project](https://github.com/HearthSim/hs-data).

See [here](https://github.com/HearthSim/HearthDb/blob/master/HearthDb.Tests/UnitTest1.cs#L14-L25) for example usage.

### Localized strings
By default, HearthDb only loads locale data for enUS and zhCN. Additional language 
data can be downloaded from `api.hearthstonejson.com`, e.g. `https://api.hearthstonejson.com/v1/latest/CardDefs.deDE.xml` and loaded at runtime via 
`HearthDb.Cards.LoadLocaleData(...)`.

If desired, all language data can be included by default by replacing the 
`<BaseDataUrl>` in `HearthDb.csproj` with `https://github.
com/HearthSim/hsdata/blob/master/CardDefs.xml`.

## CardIDs
`HearthDb.CardIds` contains properly named constant for all cardIds existing in Hearthstone. 

The `CardIds.cs` file is generated by `HearthDb.CardIdLibGenerator`. 

See [here](https://github.com/HearthSim/HearthDb/blob/master/HearthDb.Tests/UnitTest1.cs#L28-L34) for example usage.

## Enums
`HearthDb.Enums` contains all the relevant Hearthstone enums.

The `Enums.cs` file is generated by `HearthDb.EnumsGenerator`.
