namespace HearthDb.Enums
{
    public enum PlayState
    {
        INVALID = 0,
        PLAYING = 1,
        WINNING = 2,
        LOSING = 3,
        WON = 4,
        LOST = 5,
        TIED = 6,
        DISCONNECTED = 7,
        CONCEDED = 8,

        // Renamed in 10833,
        QUIT = CONCEDED
    }
}