using Checkers.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
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
using Checkers.Utilities;

namespace Checkers
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			SwapPage(Functions.Pages.GamePage);
		}

		internal void SwapPage(Functions.Pages page)
		{
			string pageName = Functions.GetPageName(page);
			mainFrame.Navigate(new Uri($"Views\\Pages\\{pageName}.xaml", UriKind.Relative));
			Title = $"Dictionary/{pageName}";
		}
	}
}
