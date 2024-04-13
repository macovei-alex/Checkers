using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.Logic
{
	internal class GameException : ApplicationException
	{
		public GameException(string message) : base(message) { }
	}
}
