namespace HearthDb.Enums
{
	public enum MetaDataType
	{
		// From HistoryMeta.Type
		TARGET = 0,
		DAMAGE = 1,
		HEALING = 2,
		JOUST = 3,

		// Renamed in 9786 from PowerHistoryMetaData.Type
		META_TARGET = TARGET,
		META_DAMAGE = DAMAGE,
		META_HEALING = HEALING
	}
}