using System;
using System.Timers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Windows;
using ColorControl.ColorModes;

namespace ColorControl.Models
{
	class PropertiesModel : INotifyPropertyChanged, IDisposable
	{
		public event PropertyChangedEventHandler PropertyChanged;
		
		public string Address
		{
			get => address.ToString();
			set
			{
				try
				{
					var prepared = value.Replace(',', '.').Replace("_", string.Empty);
					var newAddress = IPAddress.Parse(prepared);

					if (address != newAddress)
					{
						address = newAddress;
						PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Address"));
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString());
				}
			}
		}

		public ColorMode Mode
		{
			get => mode;
			set
			{
				if (mode != value)
				{
					mode = value;
					mode.UpdateAsync(Address, true).GetAwaiter();
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Mode"));
				}
			}
		}

		public List<ColorMode> Modes
		{
			get => modes;
			set
			{
				if (modes != value)
				{
					modes = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Modes"));
				}
			}
		}

		private Timer timer;

		private IPAddress address
			= new IPAddress(new byte[] { 192, 168, 1, 166 });

		private ColorMode mode = new StaticColorMode();

		private List<ColorMode> modes = new List<ColorMode>();

		public PropertiesModel()
		{
			modes.Add(mode);
			modes.Add(new CircleColorMode());
			modes.Add(new FlowColorMode());
			modes.Add(new StrobeLightMode());

			timer = new Timer(100D);
			timer.Elapsed += Timer_Elapsed;
			timer.AutoReset = true;
			timer.Start();
		}

		public void Dispose()
		{
			if (timer != null)
			{
				timer.Dispose();
				timer = null;
			}

			if (modes != null)
			{
				foreach (var mode in modes)
					mode.Dispose();

				modes = null;
			}
		}

		private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			if (timer != null)
				timer.Enabled = false;

			if (timer != null)
				await mode?.UpdateAsync(Address);

			if (timer != null)
				timer.Enabled = true;
		}
	}
}
