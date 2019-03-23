using System.ComponentModel.Composition;
using ReshaperCore;
using ReshaperUI.Display.Xaml.Windows;

namespace ReshaperUI
{
	[Export(typeof(IAssemblyLifetimeManager))]
	public class UILifetimeManager : IAssemblyLifetimeManager
	{
		public void Init()
		{
			EventViewWindow window = new EventViewWindow();
			window.Show();
		}

		public void Shutdown()
		{

		}
	}
}
