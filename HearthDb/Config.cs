namespace HearthDb
{
    public static class Config
    {
        /// <summary>
        /// Automatically and synchronously load card data included with HearthDB.
        /// By default, this will be all non-localized card metadata, as well as localized strings in enUS and zhCN.
        /// If disabled, card data needs to be explicitly loaded via <c>HearthDb.Cards.LoadBaseData</c>
        /// </summary>
        public static bool AutoLoadCardDefs { get; set; } = true;
    }
}