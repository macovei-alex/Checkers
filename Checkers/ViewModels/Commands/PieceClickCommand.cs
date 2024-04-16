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
			return true;
		}

		public override void Execute(object parameter)
		{
			_gameVM.PieceClicked(_pieceVM);
		}
	}
}
