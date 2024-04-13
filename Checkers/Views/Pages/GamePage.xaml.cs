using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Checkers.Logic;
using Checkers.Models;
using Checkers.ViewModels;
using Microsoft.Win32;
using Checkers.Utilities;

namespace Checkers.Views
{
	/// <summary>
	/// Interaction logic for SeetingsPage.xaml
	/// </summary>
	public partial class GamePage : Page
	{
		public GamePage()
		{
			InitializeComponent();
		}
	}
}
