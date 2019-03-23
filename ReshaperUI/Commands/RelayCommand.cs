using System;
using System.Windows.Input;

namespace ReshaperUI.Commands
{
	public class RelayCommand<T> : ICommand
	{
		protected Func<bool> _canExecute;
		protected Func<T, bool> _canExecuteWithParam;
		protected Action<T> _execute;

		public RelayCommand(Action<T> execute, Func<bool> canExecute = null)
		{
			this._execute = execute;
			this._canExecute = canExecute;
		}

		public RelayCommand(Action<T> execute, Func<T, bool> canExecuteWithParam)
		{
			this._execute = execute;
			this._canExecuteWithParam = canExecuteWithParam;
		}

		public event EventHandler CanExecuteChanged
		{
			add
			{
				CommandManager.RequerySuggested += value;
			}
			remove
			{
				CommandManager.RequerySuggested -= value;
			}
		}

		public virtual bool CanExecute(object parameter)
		{
			return _canExecute?.Invoke() ?? _canExecuteWithParam?.Invoke((T)parameter) ?? true;
		}

		public virtual void Execute(object parameter)
		{
			if (_execute != null)
			{
				_execute((T)parameter);
			}
		}
	}
	public class RelayCommand : ICommand
	{
		protected Func<bool> _canExecute;
		protected Action _execute;

		public RelayCommand(Action execute, Func<bool> canExecute = null)
		{
			this._execute = execute;
			this._canExecute = canExecute;
		}

		public event EventHandler CanExecuteChanged
		{
			add
			{
				CommandManager.RequerySuggested += value;
			}
			remove
			{
				CommandManager.RequerySuggested -= value;
			}
		}

		public virtual bool CanExecute(object parameter)
		{
			return _canExecute?.Invoke() ?? true;
		}

		public virtual void Execute(object parameter)
		{
			if (_execute != null)
			{
				_execute();
			}
		}
	}
}
