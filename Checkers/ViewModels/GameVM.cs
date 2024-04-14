using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Shapes;
using Checkers.Logic;
using Checkers.Models;
using Checkers.Utilities;
using static Checkers.Utilities.Enums;

namespace Checkers.ViewModels
{
	internal class GameVM : BaseViewModel
	{
		private readonly FileManagerVM _fileManagerVM;
		public FileManagerVM FileManagerVM => _fileManagerVM;

		private Game _game;
		public Game Game
		{
			get => _game;
			set
			{
				_game = value;
				BoardVM = new BoardVM(this, _game.Board);
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

		private List<Pair> _moves;
		public List<Pair> Moves
		{
			get => _moves;
			set
			{
				_moves = value;
				OnPropertyChanged(nameof(Moves));
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

		public GameVM()
		{
			_fileManagerVM = new FileManagerVM(this);
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
				Game = new Game(board);
			}

			BoardVM = new BoardVM(this, board);
			SelectedPiece = null;

			if (PossibleMoves == null)
			{
				PossibleMoves = new ObservableCollection<Pair>();
				PossibleMoves.CollectionChanged += PossibleMoves_CollectionChanged;
			}
			else
			{
				Functions.Clear(PossibleMoves);
			}

			ErrorMessage = string.Empty;
		}

		private void PossibleMoves_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			Pair pair;
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					pair = e.NewItems[e.NewItems.Count - 1] as Pair;
					BoardVM.PiecesVM[pair.Item1][pair.Item2].IsSelected = true;
					break;

				case NotifyCollectionChangedAction.Remove:
					pair = e.OldItems[e.OldItems.Count - 1] as Pair;
					BoardVM.PiecesVM[pair.Item1][pair.Item2].IsSelected = false;
					break;
			}
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
			Functions.AddRange(PossibleMoves, Game.GetLegalMoves(Game.Board, SelectedPiece.BoardPosition));
		}

		private void HandleCaseNotAllowMultiple(PieceVM piece)
		{
			if (SelectedPiece != piece)
			{
				try
				{
					if (Game.Move(SelectedPiece.BoardPosition, piece.BoardPosition))
					{
						MessageBox.Show($"{Game.Turn} won!");
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
