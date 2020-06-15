using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Trollkit_Library.Models;

namespace Trollkit_Library.ViewModels.Commands
{
	public class SendServerCommand : ICommand
	{
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

		private readonly Action<object> _action;
		private readonly Func<object, bool> _canExecute;

		public SendServerCommand(Action action)
			: this((o) => action())
		{ }

		public SendServerCommand(Action<object> action)
			: this(action, (o) => true)
		{ }

		public SendServerCommand(Action<object> action, Func<object, bool> canExecute)
		{
			_action = action;
			this._canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			return _canExecute(parameter);
		}

		public void Execute(object parameter)
		{
			this._action(parameter);
		}
	}
}
