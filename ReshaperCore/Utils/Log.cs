using System;

namespace ReshaperCore.Utils
{
	public class Log
	{

		public static event ErrorLoggedEventHandler ErrorLogged;
		public delegate void ErrorLoggedEventHandler(Exception e, String info, String extraInfo);
		public static event InfoLoggedEventHandler InfoLogged;
		public delegate void InfoLoggedEventHandler(String info, String extraInfo);

		public static string LogText
		{
			get;
			set;
		} = string.Empty;

		public static void LogError(Exception e, String info = "", String extraInfo = "")
		{
			LogText += $"Error - {info}:\n{extraInfo}\n\n";
			try
			{
				ErrorLogged?.Invoke(e, info, extraInfo);
			}
			catch
			{
			}
		}

		public static void LogInfo(String info = "", String extraInfo = "")
		{
			LogText += $"{info}:\n{extraInfo}\n\n";
			try
			{
				InfoLogged?.Invoke(info, extraInfo);
			}
			catch
			{
			}
		}
	}
}
