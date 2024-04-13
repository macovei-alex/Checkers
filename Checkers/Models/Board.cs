using Checkers.Logic;
using Checkers.Models;
using Checkers.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;

namespace Checkers.ViewModels
{
	internal class Board
	{
		public const int DEFAULT_ROWS = 8;
		public const int DEFAULT_COLUMNS = 8;
		public const int DEFAULT_PLAYER_STARTING_ROWS = 3;

		public int Rows => Pieces != null ? Pieces.Length : 0;
		public int Columns => (Pieces != null && Pieces.Length > 0 && Pieces[0] != null) ? Pieces[0].Length : 0;

		private Piece[][] _pieces;
		public Piece[][] Pieces
		{
			get => _pieces;
			set => _pieces = value;
		}

		public Piece[] this[int index]
		{
			get { return Pieces[index]; }
			set { Pieces[index] = value; }
		}

		public Piece this[int row, int column]
		{
			get { return Pieces[row][column]; }
			set { Pieces[row][column] = value; }
		}

		public Piece this[Pair position]
		{
			get { return Pieces[position.Item1][position.Item2]; }
			set { Pieces[position.Item1][position.Item2] = value; }
		}

		static Board()
		{
			if (DEFAULT_ROWS % 2 != 0)
			{
				throw new GameException($"Board default row count ( {DEFAULT_ROWS} ) must even");
			}

			if (DEFAULT_COLUMNS < 3)
			{
				throw new GameException($"Board default column count ( {DEFAULT_COLUMNS} ) is less than 3");
			}

			if (DEFAULT_PLAYER_STARTING_ROWS * 2 > DEFAULT_ROWS)
			{
				throw new GameException($"The number of starting rows for both players ( {DEFAULT_PLAYER_STARTING_ROWS} * 2 = {DEFAULT_PLAYER_STARTING_ROWS * 2} ) cannot exceed the total row count {DEFAULT_ROWS}");
			}
		}

		public Board(int rows = DEFAULT_ROWS, int columns = DEFAULT_COLUMNS, int playerStartingRows = DEFAULT_PLAYER_STARTING_ROWS)
		{
			if (rows == -1 && columns == -1 && playerStartingRows == -1)
			{
				return;
			}

			if (rows < 1 || rows % 2 != 0)
			{
				rows = DEFAULT_ROWS;
			}
			if (columns < 3)
			{
				columns = DEFAULT_COLUMNS;
			}
			if (playerStartingRows >= rows * 2)
			{
				playerStartingRows = DEFAULT_PLAYER_STARTING_ROWS;
				rows = DEFAULT_ROWS;
			}

			Pieces = new Piece[rows][];
			for (int i = 0; i < rows; i++)
			{
				Pieces[i] = new Piece[columns];
				for (int j = 0; j < columns; j++)
				{
					if ((i + j) % 2 == 1)
					{
						if (i < playerStartingRows)
						{
							Pieces[i][j] = new Piece(Enums.Types.Queen, Enums.Colors.White);
						}
						else if ((rows - i - 1) < playerStartingRows)
						{
							Pieces[i][j] = new Piece(Enums.Types.Queen, Enums.Colors.Black);
						}
					}

					if (Pieces[i][j] == null)
					{
						Pieces[i][j] = new Piece(Enums.Types.None, (i + j) % 2 == 0 ? Enums.Colors.White : Enums.Colors.Black);
					}
				}
			}
		}

		public static Board NewEmptyBoard()
		{
			return new Board(-1, -1, -1);
		}

		public void CopyDataFrom(Board other)
		{
			if (Rows != other.Rows || Columns != other.Columns)
			{
				throw new ArrayTypeMismatchException("Cannot copy data from a board with different row or column count");
			}

			for (int i = 0; i < Rows; i++)
			{
				for (int j = 0; j < Columns; j++)
				{
					this[i, j].Type = other[i, j].Type;
					this[i, j].Color = other[i, j].Color;
				}
			}
		}

		public Board DeepClone()
		{
			Board board = NewEmptyBoard();
			board.Pieces = new Piece[Rows][];
			for (int i = 0; i < board.Rows; i++)
			{
				board.Pieces[i] = new Piece[Columns];
				for (int j = 0; j < board.Columns; j++)
				{
					board[i, j] = new Piece(this[i, j].Type, this[i, j].Color);
				}
			}

			return board;
		}

		public override bool Equals(object obj)
		{
			Board other = obj as Board;
			if (other == null)
			{
				return false;
			}

			if (Rows != other.Rows || Columns != other.Columns)
			{
				return false;
			}

			for (int row = 0; row < Rows; row++)
			{
				for (int column = 0; column < Columns; column++)
				{
					if (!Pieces[row][column].Equals(Pieces[row][column]))
					{
						return false;
					}
				}
			}

			return true;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
