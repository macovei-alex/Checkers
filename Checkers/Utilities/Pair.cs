using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.Utilities
{
	[Serializable]
	internal class Pair
	{
		public int Item1 { get; set; }
		public int Item2 { get; set; }

		public Pair(int item1 = 0, int item2 = 0)
		{
			Item1 = item1;
			Item2 = item2;
		}

		public Pair(Pair pair)
		{
			Item1 = pair.Item1;
			Item2 = pair.Item2;
		}

		public override string ToString()
		{
			return $"({Item1}, {Item2})";
		}

		public override bool Equals(object obj)
		{
			return obj is Pair pair
				&& Item1 == pair.Item1
				&& Item2 == pair.Item2;
		}

		public override int GetHashCode()
		{
			return Item1.GetHashCode() ^ Item2.GetHashCode();
		}
	}
}
