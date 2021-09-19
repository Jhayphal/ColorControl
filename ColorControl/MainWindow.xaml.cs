﻿using System.Windows;

namespace ColorControl
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			var context = this.FindResource("context") as PropertiesModel;
			context?.Dispose();
		}
	}
}
