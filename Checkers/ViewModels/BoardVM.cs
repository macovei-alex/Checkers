using Checkers.Logic;
using Checkers.Utilities;
using System.Collections.ObjectModel;
using static Checkers.Utilities.Enums;

namespace Checkers.ViewModels
{
	internal class BoardVM : BaseViewModel
	{
		private ObservableCollection<ObservableCollection<PieceVM>> _piecesVM;
		public ObservableCollection<ObservableCollection<PieceVM>> PiecesVM
		{
			get => _piecesVM;
			set
			{
				_piecesVM = value;
				OnPropertyChanged(nameof(PiecesVM));
			}
		}

		public BoardVM(GameVM gameVM)
		{
			PiecesVM = new ObservableCollection<ObservableCollection<PieceVM>>();
			for (int i = 0; i < Board.DEFAULT_ROWS; i++)
			{
				PiecesVM.Add(new ObservableCollection<PieceVM>());
				for (int j = 0; j < Board.DEFAULT_COLUMNS; j++)
				{
					bool added = false;
					if ((i + j) % 2 == 1)
					{
						if (i < Board.DEFAULT_PLAYER_STARTING_ROWS)
						{
							PiecesVM[i].Add(new PieceVM(gameVM,
								Functions.MakeImageType(Types.Queen, Colors.White),
								new Pair(i, j)));
							added = true;
						}
						else if ((Board.DEFAULT_ROWS - i - 1) < Board.DEFAULT_PLAYER_STARTING_ROWS)
						{
							PiecesVM[i].Add(new PieceVM(gameVM,
								Functions.MakeImageType(Types.Queen, Colors.Black),
								new Pair(i, j)));
							added = true;
						}
					}

					if (!added)
					{
						PiecesVM[i].Add(new PieceVM(gameVM,
							Functions.MakeImageType(Types.None, (i + j) % 2 == 0 ? Colors.White : Colors.Black),
							new Pair(i, j)));
					}
				}
			}
		}

		public BoardVM(GameVM gameVM, Board board)
		{
			PiecesVM = new ObservableCollection<ObservableCollection<PieceVM>>();
			for (int i = 0; i < board.Rows; i++)
			{
				PiecesVM.Add(new ObservableCollection<PieceVM>());
				for (int j = 0; j < board.Columns; j++)
				{
					PiecesVM[i].Add(new PieceVM(gameVM,
						Functions.MakeImageType(board[i, j].Type, board[i, j].Color),
						new Pair(i, j)));
				}
			}
		}

		public ImageTypes GetImageType(Board board, int row, int column)
		{
			if (PiecesVM[row][column] == null)
			{
				throw new GameException("The piece is null");
			}

			if (board[row][column].Type == Types.None)
			{
				return (row + column) % 2 == 0 ? ImageTypes.WhiteSquare : ImageTypes.BlackSquare;
			}
			else if (board[row][column].Type == Types.Queen)
			{
				return board[row][column].Color == Colors.White ? ImageTypes.WhiteQueen : ImageTypes.BlackQueen;
			}
			else
			{
				throw new GameException($"Invalid piece type");
			}
		}
	}
}
