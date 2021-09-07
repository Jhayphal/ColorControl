using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace ColorControl
{
	class PropertiesModel : INotifyPropertyChanged
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

		private IPAddress address
			= new IPAddress(new byte[] { 192, 168, 1, 166 });

		private ColorMode mode = new StaticColorMode { Name = "Статический цвет", CurrentColor = Colors.Red };

		private List<ColorMode> modes = new List<ColorMode>();

		public PropertiesModel()
		{
			modes.Add(mode);
			modes.Add(new FastMoveColorMode { Name = "Плывущая пиздота", CurrentColor = Colors.BurlyWood });
			modes.Add(new SlowMoveColorMode());
			modes.Add(new FlowColorMode());
			modes.Add(new FlashColorMode());
		}
	}
}
