using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.IO;
using Checkers.Utilities;
using System;
using static Checkers.Utilities.Enums;

namespace Checkers.Models
{
	internal class Piece
	{
		protected Colors _color;
		public virtual Colors Color
		{
			get => _color;
			set => _color = value;
		}

		protected Types _type;
		public virtual Types Type
		{
			get => _type;
			set => _type = value;
		}

		public Piece(Types type, Colors color)
		{
			Type = type;
			Color = color;
		}

		public override string ToString()
		{
			if (Type == Types.None)
			{
				if (Color == Colors.None)
				{
					return "-";
				}
				return Color == Colors.White ? "W" : "B";
			}
			if (Type == Types.Queen)
			{
				return Color == Colors.White ? "WQ" : "BQ";
			}
			if (Type == Types.King)
			{
				return Color == Colors.White ? "WK" : "BK";
			}
			return "-";
		}

		public override bool Equals(object obj)
		{
			return obj is Piece other
				&& Type == other.Type
				&& Color == other.Color;
		}

		public override int GetHashCode()
		{
			return Type.GetHashCode() * 10 + Color.GetHashCode();
		}

		public virtual void RemovePiece()
		{
			Type = Types.None;
			Color = Colors.Black;
		}
	}
}
