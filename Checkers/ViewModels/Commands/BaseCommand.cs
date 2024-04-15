using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Checkers.ViewModels.Commands
{
	internal abstract class BaseCommand : ICommand
	{
		public event EventHandler CanExecuteChanged;

		public BaseCommand()
		{
			CanExecuteChanged += (s, e) => CanExecute(null);
		}

		public virtual bool CanExecute(object parameter)
		{
			return true;
		}

		public abstract void Execute(object parameter);

		public void NotifyCanExecute()
		{
			if (CanExecuteChanged != null)
			{
				CanExecuteChanged(this, EventArgs.Empty);
			}
		}
	}
}
