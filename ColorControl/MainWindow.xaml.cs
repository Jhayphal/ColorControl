using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ColorControl
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Timer timer;

		public MainWindow()
		{
			InitializeComponent();

			timer = new Timer(250D);
			timer.Elapsed += Timer_Elapsed;
			timer.AutoReset = true;
			timer.Start();
		}

		private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			var context = this.FindResource("context") as PropertiesModel;

			if (context == null)
				return;

			await context.Mode.UpdateAsync(context.Address);
		}
	}
}
