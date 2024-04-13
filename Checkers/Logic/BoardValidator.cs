using Checkers.Utilities;
using Checkers.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.Logic
{
	internal static class BoardValidator
	{
		public static string CheckDataLegal(Board board, bool allowMultipleMoves, int startRow, int startCol, params int[] coordinates)
		{
			if (coordinates == null || coordinates.Length == 0 || coordinates.Length % 2 != 0)
			{
				return "The number of coordinates must be even";
			}

			Pair[] positions = new Pair[coordinates.Length / 2];
			for (int i = 0; i < positions.Length; i++)
			{
				positions[i] = new Pair(coordinates[i * 2], coordinates[i * 2 + 1]);
			}

			return CheckDataLegal(board, allowMultipleMoves, new Pair(startRow, startCol), positions);
		}

		public static string CheckDataLegal(Board board, bool allowMultipleMoves, Pair start, params Pair[] positions)
		{
			string retMessage = CheckPositionLegal(board, start, true);
			if (retMessage != null)
			{
				return retMessage;
			}

			if (positions == null || positions.Length == 0)
			{
				return "Positions cannot be null or empty";
			}

			if (!allowMultipleMoves && positions.Length > 2)
			{
				return "Multiple moves are not allowed";
			}

			return null;
		}

		public static string CheckSingleMoveLegal(Board board, Pair start, Pair end)
		{
			string retMessage = CheckPositionLegal(board, start, true);
			if (retMessage != null)
			{
				return retMessage;
			}

			retMessage = CheckPositionLegal(board, end, false);
			if (retMessage != null)
			{
				return retMessage;
			}

			if (board[start].Type == Enums.Types.Queen)
			{
				int moveDistance = board[start].Color == Enums.Colors.White ? 1 : -1;
				Pair posDistance = new Pair(end.Item1 - start.Item1, end.Item2 - start.Item2);

				if (posDistance.Item1 == 2 * moveDistance && Math.Abs(posDistance.Item2) == Math.Abs(2 * moveDistance))
				{
					Pair middlePos = new Pair((end.Item1 + start.Item1) / 2, (end.Item2 + start.Item2) / 2);

					retMessage = CheckPositionLegal(board, middlePos, true);
					if (retMessage != null)
					{
						return retMessage;
					}

					if (board[middlePos].Color == board[start].Color)
					{
						return $"The piece cannot jump over a piece of its own color: piece at {start} tried jumping over {middlePos} to get to {end}";
					}
				}
				else if (posDistance.Item1 == moveDistance && Math.Abs(posDistance.Item2) == Math.Abs(moveDistance))
				{
					;
				}
				else
				{
					return $"The piece cannot move ( {posDistance.Item1}, {posDistance.Item2} ) rows and columns at a time";
				}
			}

			return null;
		}

		public static string CheckPositionLegal(Board board, Pair pos, bool shouldHavePiece)
		{
			if (pos == null)
			{
				return $"Position ( {pos} ) cannot be null";
			}
			if (!Functions.IsBetween(pos.Item1, 0, board.Rows)
				|| !Functions.IsBetween(pos.Item2, 0, board.Columns))
			{
				return $"Position ( {pos} ) is out of bounds";
			}

			if (shouldHavePiece)
			{
				if (board[pos] == null || board[pos].Type == Enums.Types.None)
				{
					return $"There is no piece at starting position ( {pos} )";
				}
			}
			else
			{
				if (board[pos] != null && board[pos].Type != Enums.Types.None)
				{
					return $"There is already a piece at position ( {pos} )";
				}
			}

			return null;
		}
	}
}
