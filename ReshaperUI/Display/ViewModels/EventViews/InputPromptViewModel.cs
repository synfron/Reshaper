using System;
using System.Windows.Input;
using ReshaperCore.Providers;
using ReshaperUI.Commands;
using ReshaperUI.Display.ViewModels.Base;
using ReshaperUI.Utils;

namespace ReshaperUI.Display.ViewModels.EventViews
{
	public class InputPromptViewModel : BaseViewModel
	{
		private readonly SelfProvider _selfProvider;
		private ICommand _saveCommand;
		private Action<string> _callback;

		public virtual event CloseRequestedHandler CloseRequested;
		public delegate void CloseRequestedHandler();

		public ICommand SaveCommand
		{
			get
			{
				if (_saveCommand == null)
				{
					_saveCommand = new RelayCommand<string>((text) =>
					{
						if (CloseRequested != null)
						{
							CloseRequested();
						}
						_selfProvider.GetInstance().PromptCanceled -= OnInputPromptModelPromptCanceled;
						_callback(text);
					}, () =>
				{
					return true;
				});
				}
				return _saveCommand;
			}
		}

		public object Id
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string Text
		{
			get;
			set;
		}

		public InputPromptViewModel(Action<string> callback)
		{
			this._callback = callback;
			_selfProvider = new SelfProvider();
			_selfProvider.GetInstance().PromptCanceled += OnInputPromptModelPromptCanceled;
		}

		private void OnInputPromptModelPromptCanceled(object id)
		{
			if (id == Id)
			{
				_selfProvider.GetInstance().PromptCanceled -= OnInputPromptModelPromptCanceled;
				ThreadUtils.RunInUiAsync(() => CloseRequested?.Invoke());
			}
		}

	}
}
