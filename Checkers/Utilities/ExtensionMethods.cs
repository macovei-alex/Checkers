using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

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
			return matrix.Length;
		}

		public static int Columns<T>(this T[][] matrix)
		{
			return matrix != null ? matrix[0].Length : 0;
		}
	}
}
