using System;
using System.Windows;
using System.Windows.Threading;
using ReshaperCore.Utils;

namespace ReshaperUI.Utils
{
	public static class ThreadUtils
	{
		public static void RunInUiAsync(Action action, Dispatcher dispatcher = null)
		{
			(dispatcher ?? Application.Current.Dispatcher).InvokeAsync(() =>
			{
				try
				{
					action.Invoke();
				}
				catch (Exception e)
				{
					Log.LogError(e, "Exception while updating the UI");
				}
			});
		}
	}
}
