using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		/*public static string ImageTypeToString(ImageTypes imageType)
		{
			switch (imageType)
			{
				case ImageTypes.None:
					return "None";
				case ImageTypes.WhiteSquare:
					return "White square";
				case ImageTypes.BlackSquare:
					return "Black square";
				case ImageTypes.WhiteQueen:
					return "White queen";
				case ImageTypes.BlackQueen:
					return "Black queen";
				default:
					return "Invalid image type";
			}
		}*/
	}
}
