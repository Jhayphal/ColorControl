using ColorControl.Models;
using System.Windows;

namespace ColorControl
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		readonly PropertiesModel context;

		public MainWindow()
		{
			InitializeComponent();

			context = this.FindResource("context") as PropertiesModel;
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			context.Dispose();
		}
	}
}
