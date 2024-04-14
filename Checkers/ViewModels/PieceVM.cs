using Checkers.Utilities;
using Newtonsoft.Json;
using System.Windows.Media.Imaging;
using Checkers.Models;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using Checkers.ViewModels.Commands;
using static Checkers.Utilities.Enums;

namespace Checkers.ViewModels
{
	internal class PieceVM : BaseViewModel
	{
		public static readonly Dictionary<ImageTypes, BitmapImage> ImagesDictionary = new Dictionary<ImageTypes, BitmapImage> {
			{ ImageTypes.WhiteSquare, Functions.LoadImage(Path.Combine(Functions.AssetsFolderPath, "WhiteSquare.png")) },
			{ ImageTypes.BlackSquare, Functions.LoadImage(Path.Combine(Functions.AssetsFolderPath, "BlackSquare.png")) },
			{ ImageTypes.WhiteQueen, Functions.LoadImage(Path.Combine(Functions.AssetsFolderPath, "WhiteQueen.png")) },
			{ ImageTypes.BlackQueen, Functions.LoadImage(Path.Combine(Functions.AssetsFolderPath, "BlackQueen.png")) }
		};

		private readonly Pair _boardPosition;
		public Pair BoardPosition { get => _boardPosition; }

		private ImageTypes _imageType;
		public ImageTypes ImageType
		{
			get { return _imageType; }
			set
			{
				if (_imageType != value && value != ImageTypes.None)
				{
					_imageType = value;
					Image = ImagesDictionary[value];
				}
				OnPropertyChanged(nameof(ImageType));
			}
		}

		private BitmapImage _image;
		public BitmapImage Image
		{
			get { return _image; }
			private set
			{
				_image = value;
				OnPropertyChanged(nameof(Image));
			}
		}

		private bool _isSelected;
		public bool IsSelected
		{
			get { return _isSelected; }
			set
			{
				_isSelected = value;
				ImageOpacity = value == true ? 0.5 : 1;
				OnPropertyChanged(nameof(IsSelected));
			}
		}

		private double _imageOpacity;
		public double ImageOpacity
		{
			get { return _imageOpacity; }
			private set
			{
				_imageOpacity = value;
				OnPropertyChanged(nameof(ImageOpacity));
			}
		}

		private readonly PieceClickCommand _clickCommand;
		public PieceClickCommand ClickCommand => _clickCommand;

		public PieceVM(GameVM gameVM, ImageTypes imageType, Pair boardPosition)
		{
			_boardPosition = boardPosition;
			_clickCommand = new PieceClickCommand(gameVM, this);
			ImageType = imageType;
			IsSelected = false;
		}

		public override string ToString()
		{
			return $"Piece at {BoardPosition}: {ImageType}";
		}
	}
}
