#region

using System;
using System.Reflection;

#endregion

namespace HearthDb
{
	public static class Info
	{
		public static Version HearthDbVersion => Assembly.GetExecutingAssembly().GetName().Version;
	}
}