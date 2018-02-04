using System;


namespace HearthStone.Database.Deckstrings
{
	public class InvalidDeckException : Exception
	{
		public InvalidDeckException(string message) : base(message)
		{
		}
	}
}
