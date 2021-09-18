using System.Timers;
using System.Windows;

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

			timer = new Timer(75D);
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

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			timer?.Dispose();
		}
	}
}
