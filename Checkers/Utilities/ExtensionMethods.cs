using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Checkers.Utilities
{
	internal static class ExtensionMethods
	{
		public static T DeepClone<T>(this T obj)
		{
			using (var ms = new MemoryStream())
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(ms, obj);
				ms.Position = 0;

				return (T)formatter.Deserialize(ms);
			}
		}

		public static int Rows<T>(this T[][] matrix)
		{
			return matrix != null ? matrix.Length : 0;
		}

		public static int Columns<T>(this T[][] matrix)
		{
			return matrix != null && matrix[0] != null ? matrix[0].Length : 0;
		}
	}
}
