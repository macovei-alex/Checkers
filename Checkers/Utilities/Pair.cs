using System;

namespace Checkers.Utilities
{
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

		public int Max()
		{
			return Math.Max(Item1, Item2);
		}

		public int Min()
		{
			return Math.Min(Item1, Item2);
		}

		public static Pair operator -(Pair first, Pair second)
		{
			return new Pair(first.Item1 - second.Item1, first.Item2 - second.Item2);
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
