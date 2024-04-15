using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using Checkers.Logic;
using Checkers.Models;
using Checkers.Utilities;
using Checkers.ViewModels.Commands;
using static Checkers.Utilities.Enums;

namespace Checkers.ViewModels
{
	internal class GameVM : BaseViewModel
	{
		#region data

		public FileManagerVM FileManagerVM { get; }

		private Game _game;
		public Game Game
		{
			get => _game;
			set
			{
				_game = value;
				BoardVM = new BoardVM(this, _game.Board);
				MultipleMovesAllow = null;
				OnPropertyChanged(nameof(Game));
			}
		}

		private BoardVM _boardVM;
		public BoardVM BoardVM
		{
			get => _boardVM;
			set
			{
				_boardVM = value;
				OnPropertyChanged(nameof(BoardVM));
			}
		}

		public Board TemporaryBoard { get; set; }

		private PieceVM _selectedPiece;
		public PieceVM SelectedPiece
		{
			get => _selectedPiece;
			set
			{
				if (_selectedPiece != null)
				{
					_selectedPiece.IsSelected = false;
				}
				_selectedPiece = value;
				if (_selectedPiece != null)
				{
					_selectedPiece.IsSelected = true;
				}
				OnPropertyChanged(nameof(SelectedPiece));
			}
		}

		private ObservableCollection<Pair> _multipleMoves;
		public ObservableCollection<Pair> MultipleMoves
		{
			get => _multipleMoves;
			set
			{
				_multipleMoves = value;
				OnPropertyChanged(nameof(MultipleMoves));
			}
		}

		private ObservableCollection<Pair> _possibleMoves;
		public ObservableCollection<Pair> PossibleMoves
		{
			get => _possibleMoves;
			set
			{
				_possibleMoves = value;
				OnPropertyChanged(nameof(PossibleMoves));
			}
		}

		private string _errorMessage;
		public string ErrorMessage
		{
			get => _errorMessage;
			set
			{
				_errorMessage = value;
				OnPropertyChanged(nameof(ErrorMessage));
			}
		}

		public string MultipleMovesAllow
		{
			get => Game.AllowMultipleMoves ? "Multiple moves are allowed" : "Multiple moves are NOT allowed";
			set
			{
				OnPropertyChanged(nameof(MultipleMovesAllow));
			}
		}

		public ICommand ApplyMoveCommand { get; private set; }

		#endregion

		public GameVM()
		{
			FileManagerVM = new FileManagerVM(this);

			ReInitializeGame();
		}

		public void ReInitializeGame(Game newGame = null)
		{
			Board board;
			if (newGame != null)
			{
				board = newGame.Board;
				Game = newGame;
			}
			else
			{
				board = new Board();

				var result = MessageBox.Show("Do you want to allow multiple moves for this game?", "Allow mutiple moves", MessageBoxButtons.YesNo);

				if (result == DialogResult.Yes)
				{
					Game = new Game(board, true);
					ApplyMoveCommand = new RelayCommand(ApplyCurrentMove, (parameter) => true);
				}
				else
				{
					Game = new Game(board);
					ApplyMoveCommand = new RelayCommand(ApplyCurrentMove, (parameter) => false);
				}
			}

			BoardVM = new BoardVM(this, board);
			TemporaryBoard = board.DeepClone();
			SelectedPiece = null;

			if (PossibleMoves == null)
			{
				PossibleMoves = new ObservableCollection<Pair>();
				PossibleMoves.CollectionChanged += (s, e) => Functions.CollectionChanged(e,
					(pair) => BoardVM.PiecesVM[pair.Item1][pair.Item2].IsSelected = true,
					(pair) => BoardVM.PiecesVM[pair.Item1][pair.Item2].IsSelected = false,
					MultipleMoves);
			}
			else
			{
				Functions.Clear(PossibleMoves);
			}

			if (MultipleMoves == null)
			{
				MultipleMoves = new ObservableCollection<Pair>();
				MultipleMoves.CollectionChanged += (s, e) => Functions.CollectionChanged(e,
					(pair) => BoardVM.PiecesVM[pair.Item1][pair.Item2].IsSelected = true,
					(pair) => BoardVM.PiecesVM[pair.Item1][pair.Item2].IsSelected = false,
					PossibleMoves);
			}
			else
			{
				Functions.Clear(MultipleMoves);
			}

			ErrorMessage = string.Empty;
		}

		public void PieceClicked(PieceVM piece)
		{
			if (SelectedPiece == null)
			{
				HandleCaseSelectedNull(piece);
				return;
			}

			if (!Game.AllowMultipleMoves)
			{
				HandleCaseNotAllowMultiple(piece);
				return;
			}

			if (Game.AllowMultipleMoves)
			{
				HandleAllowMultipleMoves(piece);
				return;
			}

			if (Game.AllowMultipleMoves)
			{
				UpdateImages();
			}
		}

		private void HandleCaseSelectedNull(PieceVM piece)
		{
			if (Game.Board[piece.BoardPosition].Type == Types.None)
			{
				return;
			}
			if (Game.Board[piece.BoardPosition].Color != Game.Turn)
			{
				return;
			}

			SelectedPiece = piece;
			Functions.AddRange(PossibleMoves, Game.GetLegalMoves(Game.Board, false, SelectedPiece.BoardPosition));
		}

		private void HandleCaseNotAllowMultiple(PieceVM piece)
		{
			if (SelectedPiece != piece)
			{
				try
				{
					if (Game.Move(SelectedPiece.BoardPosition, piece.BoardPosition))
					{
						new Thread(() => MessageBox.Show($"{Functions.OppositeColor(Game.Turn)} won!")).Start();
					}
				}
				catch (GameException exception)
				{
					ErrorMessage = exception.Message;
				}
			}

			SelectedPiece = null;
			Functions.Clear(PossibleMoves);
			UpdateImages();
		}

		private void HandleAllowMultipleMoves(PieceVM piece)
		{
			HandleCaseNotAllowMultiple(piece);
		}

		private void ApplyCurrentMove(object parameter)
		{

		}

		private void UpdateImages()
		{
			for (int i = 0; i < Game.Board.Rows; i++)
			{
				for (int j = 0; j < Game.Board.Columns; j++)
				{
					BoardVM.PiecesVM[i][j].ImageType = Functions.MakeImageType(Game.Board[i, j].Type, Game.Board[i, j].Color);
				}
			}
		}
	}
}
