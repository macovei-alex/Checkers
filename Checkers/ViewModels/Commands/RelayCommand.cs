using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Checkers.ViewModels.Commands
{
	internal class RelayCommand : BaseCommand
	{
		private readonly Action<object> _execute;
		private Func<object, bool> _canExecute;

		public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
			: base()
		{
			if (execute == null)
			{
				throw new ArgumentNullException(nameof(execute));
			}
			_execute = execute;
			_canExecute = canExecute;
		}

		public override bool CanExecute(object parameter)
		{
			return _canExecute == null || _canExecute(parameter);
		}

		public override void Execute(object parameter)
		{
			_execute(parameter);
		}
	}
}
