using Checkers.Logic;
using Checkers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using static Checkers.Utilities.Enums;

namespace Checkers.ViewModels.Commands
{
	internal class PieceClickCommand : BaseCommand
	{
		private readonly GameVM _gameVM;
		private readonly PieceVM _pieceVM;

		public PieceClickCommand(GameVM gameVM, PieceVM pieceVM)
		{
			_gameVM = gameVM;
			_pieceVM = pieceVM;
		}

		public override bool CanExecute(object parameter)
		{
			/*if (_gameVM.SelectedPiece == null)
			{
				return PieceNotEmpty() && PieceGoodColor();
			}

			if (!_gameVM.Game.AllowMultipleMoves)
			{
				return PieceGoodMove() || _gameVM.SelectedPiece == _pieceVM;
			}

			if (_gameVM.Game.AllowMultipleMoves)
			{
				return true;
			}*/

			return true;
		}

		public override void Execute(object parameter)
		{
			_gameVM.PieceClicked(_pieceVM);
		}

		#region validations

		private bool PieceNotEmpty()
		{
			return _gameVM.Game.Board[_pieceVM.BoardPosition].Type != Types.None;
		}

		private bool PieceGoodColor()
		{
			return _gameVM.Game.Board[_pieceVM.BoardPosition].Color == _gameVM.Game.Turn;
		}

		private bool PieceGoodMove()
		{
			return _gameVM.PossibleMoves.Contains(_pieceVM.BoardPosition);
		}

		#endregion
	}
}
