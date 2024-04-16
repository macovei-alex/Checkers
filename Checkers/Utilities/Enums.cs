namespace Checkers.Utilities
{
	internal static class Enums
	{
		public enum Types
		{
			None,
			Queen,
			King
		}

		public enum Colors
		{
			None,
			White,
			Black
		}

		public enum ImageTypes
		{
			None = Types.None * 10 + Colors.None,
			WhiteSquare = Types.None * 10 + Colors.White,
			BlackSquare = Types.None * 10 + Colors.Black,
			WhiteQueen = Types.Queen * 10 + Colors.White,
			BlackQueen = Types.Queen * 10 + Colors.Black,
			WhiteKing = Types.King * 10 + Colors.White,
			BlackKing = Types.King * 10 + Colors.Black
		}
	}
}
