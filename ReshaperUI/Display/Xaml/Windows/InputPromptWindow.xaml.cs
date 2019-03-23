using System.Windows;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels;
using ReshaperUI.Display.ViewModels.EventViews;

namespace ReshaperUI.Display.Xaml.Windows
{
	/// <summary>
	/// Interaction logic for InputPromptWindow.xaml
	/// </summary>
	public partial class InputPromptWindow : Window, IModelIndependentPresenter<InputPromptViewModel>
	{
		public InputPromptWindow()
		{
			InitializeComponent();
		}

		public void SetModel(object model)
		{
			DataContext = model;
			((InputPromptViewModel)model).CloseRequested += OnCloseRequested;
		}

		private void OnCloseRequested()
		{
			Close();
		}

		void IModelIndependentPresenter.Show()
		{
			Show();
			Activate();
		}
	}
}
