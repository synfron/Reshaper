using System.Windows;
using ReshaperCore;

namespace Reshaper
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private readonly Bootstrapper bootstrapper;

		public App()
		{
			bootstrapper = new Bootstrapper();
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			bootstrapper.Init();
		}

		protected override void OnExit(ExitEventArgs e)
		{
			bootstrapper.Shutdown();
			base.OnExit(e);
		}
	}
}
