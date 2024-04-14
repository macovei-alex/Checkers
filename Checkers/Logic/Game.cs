using Checkers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Checkers.ViewModels;
using Checkers.Utilities;

namespace Checkers.Logic
{
	internal class Game
	{
		public const int MOVE_DISTANCE = 1;
		public const int JUMP_DISTANCE = 2;

		public Board Board { get; set; }

		private readonly bool _allowMultipleMoves;
		public bool AllowMultipleMoves => _allowMultipleMoves;

		public Enums.Colors Turn { get; private set; }

		public Game(Board board, bool allowMultipleMoves = false)
		{
			Board = board;
			_allowMultipleMoves = allowMultipleMoves;
			Turn = Enums.Colors.White;
		}

		[JsonConstructor]
		public Game(Board board, bool allowMultipleMoves, Enums.Colors turn)
		{
			Board = board;
			_allowMultipleMoves = allowMultipleMoves;
			Turn = turn;
		}

		#region higher level move methods

		public bool Move(int startingRow, int startingCol, params int[] coordinates)
		{
			if (coordinates == null || coordinates.Length == 0 || coordinates.Length % 2 != 0)
			{
				throw new GameException("The number of coordinates must be even");
			}

			Pair[] positions = new Pair[coordinates.Length / 2];
			for (int i = 0; i < positions.Length; i++)
			{
				positions[i] = new Pair(coordinates[i * 2], coordinates[i * 2 + 1]);
			}

			return Move(new Pair(startingRow, startingCol), positions);
		}

		public bool Move(Pair startingPosition, params Pair[] positions)
		{
			if (positions?.Length > 1)
			{
				Board clone = Board.DeepClone();
				MoveWithoutTurn(clone, startingPosition, positions);
				Board.CopyDataFrom(clone);
			}
			else
			{
				MoveWithoutTurn(Board, startingPosition, positions);
			}

			Turn = Functions.OppositeColor(Turn);
			return CheckWin();
		}

		public void MoveWithoutTurn(Board board, int startingRow, int startingCol, params int[] coordinates)
		{
			if (coordinates == null || coordinates.Length == 0 || coordinates.Length % 2 != 0)
			{
				throw new GameException("The number of coordinates must be even");
			}

			Pair[] positions = new Pair[coordinates.Length / 2];
			for (int i = 0; i < positions.Length; i++)
			{
				positions[i] = new Pair(coordinates[i * 2], coordinates[i * 2 + 1]);
			}

			MoveWithoutTurn(board, new Pair(startingRow, startingCol), positions);
		}

		#endregion

		public void MoveWithoutTurn(Board board, Pair start, params Pair[] positions)
		{
			string retMessage = BoardValidator.CheckDataLegal(board, AllowMultipleMoves, start, positions);
			if (retMessage != null)
			{
				throw new GameException(retMessage);
			}

			SingleMoveWithoutTurn(board, start, positions[0]);
			for (int i = 1; i < positions.Length; i++)
			{
				SingleMoveWithoutTurn(board, positions[i - 1], positions[i]);
			}

			board[positions.Last()].Type = board[start].Type;
			board[positions.Last()].Color = board[start].Color;
			board[start].RemovePiece();

			Board.CopyDataFrom(board);
		}

		public void SingleMoveWithoutTurn(Board board, Pair start, Pair end)
		{
			string retMessage = BoardValidator.CheckSingleMoveLegal(board, start, end);
			if (retMessage != null)
			{
				throw new GameException(retMessage);
			}

			if (Math.Abs((start - end).Min()) == 2)
			{
				Pair middlePos = new Pair(
					(start.Item1 + end.Item1) / 2,
					(start.Item2 + end.Item2) / 2);
				var oppositeColor = Functions.OppositeColor(Turn);

				if (board[middlePos]?.Color == oppositeColor)
				{
					board[middlePos].RemovePiece();
				}
			}

			if (end.Item1 == 0 || end.Item1 == Board.DEFAULT_ROWS - 1)
			{
				board[end].Type = Enums.Types.King;
			}
		}

		public List<Pair> GetLegalMoves(Board board, Pair start, Pair[] excludedPositions = null)
		{
			List<Pair> values = new List<Pair>();

			Pair end = null;
			Pair rowLimits = new Pair(Math.Max(0, start.Item1 - 2), Math.Min(start.Item1 + 2, board.Rows - 1));
			Pair colLimits = new Pair(Math.Max(0, start.Item2 - 2), Math.Min(start.Item2 + 2, board.Columns - 1));

			for (int i = rowLimits.Item1; i <= rowLimits.Item2; i++)
			{
				for (int j = colLimits.Item1; j <= colLimits.Item2; j++)
				{
					if (excludedPositions != null && excludedPositions.Contains(new Pair(i, j)))
					{
						continue;
					}

					if (end == null)
					{
						end = new Pair(i, j);
					}
					else
					{
						end.Item1 = i;
						end.Item2 = j;
					}

					if (BoardValidator.CheckSingleMoveLegal(board, start, end) == null)
					{
						values.Add(end);
						end = null;
					}
				}
			}
			return values;
		}

		private bool CheckWin()
		{
			Enums.Colors oppositeColor = Functions.OppositeColor(Turn);

			for (byte i = 0; i < Board.DEFAULT_ROWS; i++)
			{
				for (byte j = 0; j < Board.DEFAULT_COLUMNS; j++)
				{
					if (Board[i, j] != null && Board[i, j].Color == oppositeColor)
					{
						return false;
					}
				}
			}

			return true;
		}

		#region Utility methods

		public override bool Equals(object obj)
		{
			Game other = obj as Game;
			if (other == null)
			{
				return false;
			}
			return Board.Equals(other.Board) && Turn == other.Turn;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < Board.Rows; i++)
			{
				for (int j = 0; j < Board.Columns; j++)
				{
					string piece = Board[i, j].ToString();
					stringBuilder.Append(piece);
					if (piece.Length == 1)
					{
						stringBuilder.Append("   ");
					}
					else
					{
						stringBuilder.Append("  ");
					}
				}
				stringBuilder.Append('\n');
			}
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		public static Game FromJson(string json)
		{
			return JsonConvert.DeserializeObject<Game>(json);
		}

		#endregion
	}
}
