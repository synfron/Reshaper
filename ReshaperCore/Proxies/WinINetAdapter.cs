

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ReshaperCore.Proxies
{
	public static class WinINetAdapter
	{
		/****************************** Module Header ******************************\
		 Module Name:  INTERNET_OPEN_TYPE.cs
		 Project:      CSWebBrowserWithProxy
		 Copyright (c) Microsoft Corporation.

		 This enum contains 4 WinINet constants used in InternetOpen function.
		 Visit http://msdn.microsoft.com/en-us/library/aa385096(VS.85).aspx to get the 
		 whole constants list.

		 This source is subject to the Microsoft Public License.
		 See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
		 All other rights reserved.

		 THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
		 EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
		 WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
		\***************************************************************************/
		public enum INTERNET_OPEN_TYPE
		{
			/// <summary>
			/// Retrieves the proxy or direct configuration from the registry.
			/// </summary>
			INTERNET_OPEN_TYPE_PRECONFIG = 0,

			/// <summary>
			/// Resolves all host names locally.
			/// </summary>
			INTERNET_OPEN_TYPE_DIRECT = 1,

			/// <summary>
			/// Passes requests to the proxy unless a proxy bypass list is supplied and the name to be resolved bypasses the proxy.
			/// </summary>
			INTERNET_OPEN_TYPE_PROXY = 3,

			/// <summary>
			/// Retrieves the proxy or direct configuration from the registry and prevents
			/// the use of a startup Microsoft JScript or Internet Setup (INS) file.
			/// </summary>
			INTERNET_OPEN_TYPE_PRECONFIG_WITH_NO_AUTOPROXY = 4
		}

		/****************************** Module Header ******************************\
		 Module Name:  INTERNET_OPTION.cs
		 Project:      CSWebBrowserWithProxy
		 Copyright (c) Microsoft Corporation.
 
		 This enum contains 4 WinINet constants used in method InternetQueryOption and 
		 InternetSetOption functions. 
		 Visit http://msdn.microsoft.com/en-us/library/aa385328(VS.85).aspx to get the 
		 whole constants list.
 
		 This source is subject to the Microsoft Public License.
		 See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
		 All other rights reserved.
 
		 THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
		 EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
		 WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
		\***************************************************************************/
		public enum INTERNET_OPTION
		{
			// Sets or retrieves an INTERNET_PER_CONN_OPTION_LIST structure that specifies
			// a list of options for a particular connection.
			INTERNET_OPTION_PER_CONNECTION_OPTION = 75,

			// Notify the system that the registry settings have been changed so that
			// it verifies the settings on the next call to InternetConnect.
			INTERNET_OPTION_SETTINGS_CHANGED = 39,

			// Causes the proxy data to be reread from the registry for a handle.
			INTERNET_OPTION_REFRESH = 37

		}

		/****************************** Module Header ******************************\
		 Module Name:  INTERNET_PER_CONN_OPTION.cs
		 Project:      CSWebBrowserWithProxy
		 Copyright (c) Microsoft Corporation.
 
		 This file defines the struct INTERNET_PER_CONN_OPTION and constants used by it.
		 The struct INTERNET_PER_CONN_OPTION contains the value of an option that to be 
		 set to internet settings.
		 Visit http://msdn.microsoft.com/en-us/library/aa385145(VS.85).aspx to get the 
		 detailed description.
 
		 This source is subject to the Microsoft Public License.
		 See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
		 All other rights reserved.
 
		 THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
		 EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
		 WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
		\***************************************************************************/

		/// <summary>
		/// Constants used in INTERNET_PER_CONN_OPTION_OptionUnion struct.
		/// </summary>
		public enum INTERNET_PER_CONN_OptionEnum
		{
			INTERNET_PER_CONN_FLAGS = 1,
			INTERNET_PER_CONN_PROXY_SERVER = 2,
			INTERNET_PER_CONN_PROXY_BYPASS = 3,
			INTERNET_PER_CONN_AUTOCONFIG_URL = 4,
			INTERNET_PER_CONN_AUTODISCOVERY_FLAGS = 5,
			INTERNET_PER_CONN_AUTOCONFIG_SECONDARY_URL = 6,
			INTERNET_PER_CONN_AUTOCONFIG_RELOAD_DELAY_MINS = 7,
			INTERNET_PER_CONN_AUTOCONFIG_LAST_DETECT_TIME = 8,
			INTERNET_PER_CONN_AUTOCONFIG_LAST_DETECT_URL = 9,
			INTERNET_PER_CONN_FLAGS_UI = 10
		}

		/// <summary>
		/// Constants used in INTERNET_PER_CONN_OPTON struct.
		/// </summary>
		public enum INTERNET_OPTION_PER_CONN_FLAGS
		{
			PROXY_TYPE_DIRECT = 0x00000001,   // direct to net
			PROXY_TYPE_PROXY = 0x00000002,   // via named proxy
			PROXY_TYPE_AUTO_PROXY_URL = 0x00000004,   // autoproxy URL
			PROXY_TYPE_AUTO_DETECT = 0x00000008   // use autoproxy detection
		}

		/// <summary>
		/// Used in INTERNET_PER_CONN_OPTION.
		/// When create a instance of OptionUnion, only one filed will be used.
		/// The StructLayout and FieldOffset attributes could help to decrease the struct size.
		/// </summary>
		[StructLayout(LayoutKind.Explicit)]
		public struct INTERNET_PER_CONN_OPTION_OptionUnion
		{
			// A value in INTERNET_OPTION_PER_CONN_FLAGS.
			[FieldOffset(0)]
			public int dwValue;
			[FieldOffset(0)]
			public System.IntPtr pszValue;
			[FieldOffset(0)]
			public System.Runtime.InteropServices.ComTypes.FILETIME ftValue;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct INTERNET_PER_CONN_OPTION
		{
			// A value in INTERNET_PER_CONN_OptionEnum.
			public int dwOption;
			public INTERNET_PER_CONN_OPTION_OptionUnion Value;
		}

		/****************************** Module Header ******************************\
		 Module Name:  INTERNET_PER_CONN_OPTION_LIST.cs
		 Project:      CSWebBrowserWithProxy
		 Copyright (c) Microsoft Corporation.
 
		 The struct INTERNET_PER_CONN_OPTION contains a list of options that to be 
		 set to internet connection.
		 Visit http://msdn.microsoft.com/en-us/library/aa385146(VS.85).aspx to get the 
		 detailed description.
 
		 This source is subject to the Microsoft Public License.
		 See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
		 All other rights reserved.
 
		 THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
		 EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
		 WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
		\***************************************************************************/
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		public struct INTERNET_PER_CONN_OPTION_LIST
		{
			public int Size;

			// The connection to be set. NULL means LAN.
			public System.IntPtr Connection;

			public int OptionCount;
			public int OptionError;

			// List of INTERNET_PER_CONN_OPTIONs.
			public System.IntPtr pOptions;
		}

		/****************************** Module Header ******************************\
		 Module Name:  NativeMethods.cs
		 Project:      CSWebBrowserWithProxy
		 Copyright (c) Microsoft Corporation.
 
		 This class is a simple .NET wrapper of wininet.dll. It contains 4 extern
		 methods in wininet.dll. They are InternetOpen, InternetCloseHandle, 
		 InternetSetOption and InternetQueryOption.
 
		 This source is subject to the Microsoft Public License.
		 See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
		 All other rights reserved.
 
		 THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
		 EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
		 WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
		\***************************************************************************/
		/// <summary>
		/// Initialize an application's use of the WinINet functions.
		/// See 
		/// </summary>
		[DllImport("wininet.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern IntPtr InternetOpen(
			string lpszAgent,
			int dwAccessType,
			string lpszProxyName,
			string lpszProxyBypass,
			int dwFlags);

		/// <summary>
		/// Close a single Internet handle.
		/// </summary>
		[DllImport("wininet.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool InternetCloseHandle(IntPtr hInternet);

		/// <summary>
		/// Sets an Internet option.
		/// </summary>
		[DllImport("wininet.dll", CharSet = CharSet.Ansi, SetLastError = true)]
		private static extern bool InternetSetOption(
			IntPtr hInternet,
			INTERNET_OPTION dwOption,
			IntPtr lpBuffer,
			int lpdwBufferLength);

		/// <summary>
		/// Queries an Internet option on the specified handle. The Handle will be always 0.
		/// </summary>
		[DllImport("wininet.dll", CharSet = CharSet.Ansi, SetLastError = true)]
		private extern static bool InternetQueryOption(
			IntPtr hInternet,
			INTERNET_OPTION dwOption,
			ref INTERNET_PER_CONN_OPTION_LIST OptionList,
			ref int lpdwBufferLength);

		/****************************** Module Header ******************************\
		 Module Name:  WinINet.cs
		 Project:      CSWebBrowserWithProxy
		 Copyright (c) Microsoft Corporation.
 
		 This class is used to set the proxy. or restore to the system proxy for the
		 current application
 
		 This source is subject to the Microsoft Public License.
		 See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
		 All other rights reserved.
 
		 THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
		 EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
		 WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
		\***************************************************************************/
		private static string agent = Process.GetCurrentProcess().ProcessName;

		/// <summary>
		/// Set the LAN connection proxy server for current process.
		/// </summary>
		/// <param name="proxyServer">
		/// The Proxy Server.
		/// </param>
		/// <returns></returns>
		public static bool SetConnectionProxy(bool isMachineSetting, string proxyServer)
		{
			if (isMachineSetting)
			{
				return SetConnectionProxy(null, proxyServer);
			}
			else
			{
				return SetConnectionProxy(agent, proxyServer);
			}
		}

		public static bool SetConnectionProxy(INTERNET_PER_CONN_OPTION_LIST optionList)
		{
			IntPtr hInternet = IntPtr.Zero;
			int size = Marshal.SizeOf(optionList);

			// Allocate memory for the INTERNET_PER_CONN_OPTION_LIST instance.
			IntPtr intptrStruct = Marshal.AllocCoTaskMem(size);

			// Marshal data from a managed object to an unmanaged block of memory.
			Marshal.StructureToPtr(optionList, intptrStruct, true);

			// Set internet settings.
			bool bReturn = InternetSetOption(
				hInternet,
				INTERNET_OPTION.INTERNET_OPTION_PER_CONNECTION_OPTION,
				intptrStruct, size);

			// Free the allocated memory.
			Marshal.FreeCoTaskMem(intptrStruct);

			return bReturn;
		}

		/// <summary>
		/// Set the LAN connection proxy server.
		/// </summary>
		/// <param name="agentName">
		/// If agentName is null or empty, this function will set the Lan proxy for
		/// the machine, else for the current process.
		/// </param>
		/// <param name="proxyServer">The Proxy Server.</param>
		/// <returns></returns>
		public static bool SetConnectionProxy(string agentName, string proxyServer)
		{
			IntPtr hInternet = IntPtr.Zero;
			try
			{
				if (!string.IsNullOrEmpty(agentName))
				{
					hInternet = InternetOpen(
						agentName,
						(int)INTERNET_OPEN_TYPE.INTERNET_OPEN_TYPE_DIRECT,
						null,
						null,
						0);
				}

				return SetConnectionProxyInternal(hInternet, proxyServer);
			}
			finally
			{
				if (hInternet != IntPtr.Zero)
				{
					InternetCloseHandle(hInternet);
				}
			}
		}

		/// <summary>
		/// Set the proxy server for LAN connection.
		/// </summary>
		static bool SetConnectionProxyInternal(IntPtr hInternet, string proxyServer)
		{

			// Create 3 options.
			INTERNET_PER_CONN_OPTION[] Options = new INTERNET_PER_CONN_OPTION[3];

			// Set PROXY flags.
			Options[0] = new INTERNET_PER_CONN_OPTION();
			Options[0].dwOption = (int)INTERNET_PER_CONN_OptionEnum.INTERNET_PER_CONN_FLAGS;
			Options[0].Value.dwValue = (int)INTERNET_OPTION_PER_CONN_FLAGS.PROXY_TYPE_PROXY;

			// Set proxy name.
			Options[1] = new INTERNET_PER_CONN_OPTION();
			Options[1].dwOption =
				(int)INTERNET_PER_CONN_OptionEnum.INTERNET_PER_CONN_PROXY_SERVER;
			Options[1].Value.pszValue = Marshal.StringToHGlobalAnsi(proxyServer);

			// Set proxy bypass.
			Options[2] = new INTERNET_PER_CONN_OPTION();
			Options[2].dwOption =
				(int)INTERNET_PER_CONN_OptionEnum.INTERNET_PER_CONN_PROXY_BYPASS;
			Options[2].Value.pszValue = Marshal.StringToHGlobalAnsi("local");

			// Allocate a block of memory of the options.
			System.IntPtr buffer = Marshal.AllocCoTaskMem(Marshal.SizeOf(Options[0])
				+ Marshal.SizeOf(Options[1]) + Marshal.SizeOf(Options[2]));

			System.IntPtr current = buffer;

			// Marshal data from a managed object to an unmanaged block of memory.
			for (int i = 0; i < Options.Length; i++)
			{
				Marshal.StructureToPtr(Options[i], current, false);
				current = (System.IntPtr)((int)current + Marshal.SizeOf(Options[i]));
			}

			// Initialize a INTERNET_PER_CONN_OPTION_LIST instance.
			INTERNET_PER_CONN_OPTION_LIST option_list = new INTERNET_PER_CONN_OPTION_LIST();

			// Point to the allocated memory.
			option_list.pOptions = buffer;

			// Return the unmanaged size of an object in bytes.
			option_list.Size = Marshal.SizeOf(option_list);

			// IntPtr.Zero means LAN connection.
			option_list.Connection = IntPtr.Zero;

			option_list.OptionCount = Options.Length;
			option_list.OptionError = 0;
			int size = Marshal.SizeOf(option_list);

			// Allocate memory for the INTERNET_PER_CONN_OPTION_LIST instance.
			IntPtr intptrStruct = Marshal.AllocCoTaskMem(size);

			// Marshal data from a managed object to an unmanaged block of memory.
			Marshal.StructureToPtr(option_list, intptrStruct, true);

			// Set internet settings.
			bool bReturn = InternetSetOption(
				hInternet,
				INTERNET_OPTION.INTERNET_OPTION_PER_CONNECTION_OPTION,
				intptrStruct, size);

			// Free the allocated memory.
			Marshal.FreeCoTaskMem(buffer);
			Marshal.FreeCoTaskMem(intptrStruct);

			// Throw an exception if this operation failed.
			if (!bReturn)
			{
				throw new ApplicationException(" Set Internet Option Failed!");
			}

			// Notify the system that the registry settings have been changed and cause
			// the proxy data to be reread from the registry for a handle.
			InternetSetOption(
				hInternet,
				INTERNET_OPTION.INTERNET_OPTION_SETTINGS_CHANGED,
				IntPtr.Zero, 0);

			InternetSetOption(
				hInternet,
				INTERNET_OPTION.INTERNET_OPTION_REFRESH,
				IntPtr.Zero, 0);

			return bReturn;
		}

		/// <summary>
		/// Get the current system options for LAN connection.
		/// Make sure free the memory after restoration. 
		/// </summary>
		public static INTERNET_PER_CONN_OPTION_LIST GetSystemProxy()
		{

			// Query following options. 
			INTERNET_PER_CONN_OPTION[] Options = new INTERNET_PER_CONN_OPTION[3];

			Options[0] = new INTERNET_PER_CONN_OPTION();
			Options[0].dwOption = (int)INTERNET_PER_CONN_OptionEnum.INTERNET_PER_CONN_FLAGS;
			Options[1] = new INTERNET_PER_CONN_OPTION();
			Options[1].dwOption = (int)INTERNET_PER_CONN_OptionEnum.INTERNET_PER_CONN_PROXY_SERVER;
			Options[2] = new INTERNET_PER_CONN_OPTION();
			Options[2].dwOption = (int)INTERNET_PER_CONN_OptionEnum.INTERNET_PER_CONN_PROXY_BYPASS;

			// Allocate a block of memory of the options.
			System.IntPtr buffer = Marshal.AllocCoTaskMem(Marshal.SizeOf(Options[0])
				+ Marshal.SizeOf(Options[1]) + Marshal.SizeOf(Options[2]));

			System.IntPtr current = (System.IntPtr)buffer;

			// Marshal data from a managed object to an unmanaged block of memory.
			for (int i = 0; i < Options.Length; i++)
			{
				Marshal.StructureToPtr(Options[i], current, false);
				current = (System.IntPtr)((int)current + Marshal.SizeOf(Options[i]));
			}

			// Initialize a INTERNET_PER_CONN_OPTION_LIST instance.
			INTERNET_PER_CONN_OPTION_LIST Request = new INTERNET_PER_CONN_OPTION_LIST();

			// Point to the allocated memory.
			Request.pOptions = buffer;

			Request.Size = Marshal.SizeOf(Request);

			// IntPtr.Zero means LAN connection.
			Request.Connection = IntPtr.Zero;

			Request.OptionCount = Options.Length;
			Request.OptionError = 0;
			int size = Marshal.SizeOf(Request);

			// Query system internet options. 
			bool result = InternetQueryOption(
				IntPtr.Zero,
				INTERNET_OPTION.INTERNET_OPTION_PER_CONNECTION_OPTION,
				ref Request,
				ref size);

			if (!result)
			{
				throw new ApplicationException("Get System Internet Option Failed! ");
			}

			return Request;
		}

		/// <summary>
		/// Restore to the system proxy settings.
		/// </summary>
		public static bool RestoreSystemProxy()
		{
			return RestoreSystemProxy(agent);
		}

		/// <summary>
		/// Restore to the system proxy settings.
		/// </summary>
		public static bool RestoreSystemProxy(string agentName)
		{
			if (string.IsNullOrEmpty(agentName))
			{
				throw new ArgumentNullException("Agent name cannot be null or empty!");
			}

			IntPtr hInternet = IntPtr.Zero;
			try
			{
				if (!string.IsNullOrEmpty(agentName))
				{
					hInternet = InternetOpen(
						agentName,
						(int)INTERNET_OPEN_TYPE.INTERNET_OPEN_TYPE_DIRECT,
						null,
						null,
						0);
				}

				return RestoreSystemProxyInternal(hInternet);
			}
			finally
			{
				if (hInternet != IntPtr.Zero)
				{
					InternetCloseHandle(hInternet);
				}
			}
		}

		/// <summary>
		/// Restore to the system proxy settings.
		/// </summary>
		static bool RestoreSystemProxyInternal(IntPtr hInternet)
		{
			var request = GetSystemProxy();

			int size = Marshal.SizeOf(request);

			// Allocate memory. 
			IntPtr intptrStruct = Marshal.AllocCoTaskMem(size);

			// Convert structure to IntPtr 
			Marshal.StructureToPtr(request, intptrStruct, true);

			// Set internet options.
			bool bReturn = InternetSetOption(
				hInternet,
				INTERNET_OPTION.INTERNET_OPTION_PER_CONNECTION_OPTION,
				intptrStruct,
				size);

			// Free the allocated memory.
			Marshal.FreeCoTaskMem(request.pOptions);
			Marshal.FreeCoTaskMem(intptrStruct);

			if (!bReturn)
			{
				throw new ApplicationException(" Set Internet Option Failed! ");
			}

			// Notify the system that the registry settings have been changed and cause
			// the proxy data to be reread from the registry for a handle.
			InternetSetOption(
				hInternet,
				INTERNET_OPTION.INTERNET_OPTION_SETTINGS_CHANGED,
				IntPtr.Zero,
				0);

			InternetSetOption(
				hInternet,
				INTERNET_OPTION.INTERNET_OPTION_REFRESH,
				IntPtr.Zero,
				0);
			return bReturn;
		}
	}
}
