namespace HearthDb.Enums
{
    public enum Type
    {
        UNKNOWN = 0,
        BOOL = 1,
        NUMBER = 2,
        COUNTER = 3,
        ENTITY = 4,
        PLAYER = 5,
        TEAM = 6,
        ENTITY_DEFINITION = 7,
        STRING = 8,

        // Not present at the time
        LOCSTRING = -2
    }
}