using System;

namespace Checkers.Logic
{
	internal class GameException : ApplicationException
	{
		public GameException(string message) : base(message) { }
	}
}
