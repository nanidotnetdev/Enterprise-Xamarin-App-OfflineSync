using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EnterpriseAddLogs.Commands
{
	public sealed class AsyncActionCommand<TParameter> : ICommand
	{
		public event EventHandler CanExecuteChanged;

		private Func<TParameter, Task> AsyncExecuteMethod { get; set; }
		private Func<TParameter, bool> CanExecuteMethod { get; set; }

		public AsyncActionCommand(Func<TParameter, Task> asyncExecuteMethod)
		{
			AsyncExecuteMethod = asyncExecuteMethod;
		}

		public AsyncActionCommand(Func<TParameter, Task> asyncExecuteMethod, Func<TParameter, bool> canExecuteMethod)
			: this(asyncExecuteMethod)
		{
			CanExecuteMethod = canExecuteMethod;
		}

		public bool CanExecute(TParameter parameter)
		{
			var canExecute = true;
			if (CanExecuteMethod != null)
			{
				canExecute = CanExecuteMethod(parameter);
			}

			return canExecute;
		}

		bool ICommand.CanExecute(object parameter)
		{
			var canExecute = false;
			if (parameter is TParameter)
			{
				canExecute = CanExecute((TParameter)parameter);
			}
			else if (parameter == null)
			{
				canExecute = CanExecute(default(TParameter));
			}

			return canExecute;
		}

		public async void Execute(TParameter parameter)
		{
			await ExecuteAsync(parameter);
		}

		async void ICommand.Execute(object parameter)
		{
			await ExecuteAsync((TParameter)parameter);
		}

		public async Task ExecuteAsync(TParameter parameter)
		{
			await AsyncExecuteMethod?.Invoke(parameter);
		}

		public void NotifyCanExecuteChanged()
		{
			CanExecuteChanged?.Invoke(this, new EventArgs());
		}
	}

	public sealed class AsyncActionCommand : ICommand
    {
		public event EventHandler CanExecuteChanged;

		private Func<Task> AsyncExecuteMethod { get; set; }
		private Func<bool> CanExecuteMethod { get; set; }

		public AsyncActionCommand(Func<Task> asyncExecuteMethod)
		{
			AsyncExecuteMethod = asyncExecuteMethod;
		}

		public AsyncActionCommand(Func<Task> asyncExecuteMethod, Func<bool> canExecuteMethod)
			: this(asyncExecuteMethod)
		{
			CanExecuteMethod = canExecuteMethod;
		}

		public bool CanExecute(object parameter)
		{
			var canExecute = true;
			if (CanExecuteMethod != null)
			{
				canExecute = CanExecuteMethod();
			}

			return canExecute;
		}

		public async void Execute(object parameter)
		{
			await ExecuteAsync(parameter);
		}

		public async Task ExecuteAsync(object parameter)
		{
			await AsyncExecuteMethod?.Invoke();
		}

		public void NotifyCanExecuteChanged()
		{
			CanExecuteChanged?.Invoke(this, new EventArgs());
		}
	}
}
