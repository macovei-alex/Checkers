using Checkers.Models;
using Checkers.Utilities;
using Checkers.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Checkers.Utilities.Enums;

namespace Checkers.Logic
{
	internal class Game
	{
		public const int MOVE_DISTANCE = 1;
		public const int JUMP_DISTANCE = 2;

		public Board Board { get; set; }

		private readonly bool _allowMultipleMoves;
		public bool AllowMultipleMoves => _allowMultipleMoves;

		public Colors Turn { get; set; }

		public Game(Board board, bool allowMultipleMoves = false)
		{
			Board = board;
			_allowMultipleMoves = allowMultipleMoves;
			Turn = Colors.White;
		}

		[JsonConstructor]
		public Game(Board board, bool allowMultipleMoves, Colors turn)
		{
			Board = board;
			_allowMultipleMoves = allowMultipleMoves;
			Turn = turn;
		}

		#region higher level move methods

		public bool Move(int startingRow, int startingCol, params int[] coordinates)
		{
			return Move(Board, startingRow, startingCol, coordinates);
		}

		public bool Move(Board board, int startingRow, int startingCol, params int[] coordinates)
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

			return Move(board, new Pair(startingRow, startingCol), positions);
		}

		public bool Move(Pair startingPosition, params Pair[] positions)
		{
			return Move(Board, startingPosition, positions);
		}

		public bool Move(Board board, Pair startingPosition, params Pair[] positions)
		{
			if (positions?.Length > 1)
			{
				Board clone = board.DeepClone();
				MoveWithoutTurn(clone, startingPosition, positions);
				board.CopyDataFrom(clone);
			}
			else
			{
				MoveWithoutTurn(board, startingPosition, positions);
			}

			Turn = Functions.OppositeColor(Turn);

			bool win = CheckWin(board);
			if (win == true)
			{
				Statistics.UpdateStatistics(this);
			}

			return win;
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

			if (board[positions.Last()].Type == Types.None)
			{
				board[positions.Last()].Type = board[start].Type;
			}
			board[positions.Last()].Color = board[start].Color;
			board[start].RemovePiece();
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

			if (end.Item1 == 0 || end.Item1 == board.Rows - 1)
			{
				board[end].Type = Types.King;
			}
		}

		public List<Pair> GetLegalMoves(Board board, bool mustCapture, Pair start)
		{
			List<Pair> legalPositoons = new List<Pair>();
			List<Pair> possiblePositions;
			if (mustCapture)
			{
				possiblePositions = new List<Pair>()
				{
					new Pair(start.Item1 - 2, start.Item2 - 2),
					new Pair(start.Item1 - 2, start.Item2 + 2),
					new Pair(start.Item1 + 2, start.Item2 - 2),
					new Pair(start.Item1 + 2, start.Item2 + 2),
				};
			}
			else
			{
				possiblePositions = new List<Pair>()
				{
					new Pair(start.Item1 - 2, start.Item2 - 2),
					new Pair(start.Item1 - 2, start.Item2 + 2),
					new Pair(start.Item1 - 1, start.Item2 - 1),
					new Pair(start.Item1 - 1, start.Item2 + 1),
					new Pair(start.Item1 + 1, start.Item2 - 1),
					new Pair(start.Item1 + 1, start.Item2 + 1),
					new Pair(start.Item1 + 2, start.Item2 - 2),
					new Pair(start.Item1 + 2, start.Item2 + 2),
				};
			}

			foreach (Pair pos in possiblePositions)
			{
				if (BoardValidator.CheckSingleMoveLegal(board, start, pos) == null)
				{
					legalPositoons.Add(pos);
				}
			}

			return legalPositoons;
		}

		private bool CheckWin(Board board)
		{
			for (byte i = 0; i < board.Rows; i++)
			{
				for (byte j = 0; j < board.Columns; j++)
				{
					if (board[i, j].Type != Types.None && board[i, j].Color == Turn)
					{
						return false;
					}
				}
			}

			return true;
		}

		#region utility methods

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
