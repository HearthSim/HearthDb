#region

using System;
using System.Reflection;

#endregion

namespace HearthDb
{
	public static class Info
	{
		public static Version HearthDbVersion => Assembly.GetExecutingAssembly().GetName().Version;
		public static Version HearthstoneVersion => new Version(5, 0, 0, 12574);
	}
}