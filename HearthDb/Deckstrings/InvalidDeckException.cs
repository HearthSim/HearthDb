using System;

namespace HearthDb.Deckstrings
{
	public class InvalidDeckException : Exception
	{
		public InvalidDeckException(string message) : base(message)
		{
		}
	}
}
