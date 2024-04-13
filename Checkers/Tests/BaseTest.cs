using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.Tests
{
	public abstract class BaseTest
	{
		public abstract void RunTests();

		public void AssertEqual<T>(T expected, T actual)
		{
			if (!expected.Equals(actual))
			{
				throw new Exception($"Expected {expected} but got {actual}");
			}
		}

		public void AssertNotEqual<T>(T expected, T actual)
		{
			if (expected.Equals(actual))
			{
				throw new Exception($"Expected {expected} but got {actual}");
			}
		}

		public void AssertThrows<TException>(Action action) where TException : Exception
		{
			try
			{
				action();
			}
			catch (TException)
			{
				return;
			}

			throw new Exception($"Expected exception of type {typeof(TException)} but none was thrown");
		}
	}
}
