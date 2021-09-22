using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ColorControl.ColorModes
{
	abstract class ColorMode : INotifyPropertyChanged, IDisposable
	{
		public event PropertyChangedEventHandler PropertyChanged;
		
		public Visibility ColorPickerVisibility
		{
			get => colorPickerVisibility;
			set
			{
				if (colorPickerVisibility != value)
				{
					colorPickerVisibility = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentColor"));
				}
			}
		}

		public string Name 
		{ 
			get => name; 
			set 
			{ 
				if (!string.Equals(name, value)) 
				{ 
					name = value; 
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name")); 
				} 
			} 
		}

		public Color LastColor { get; protected set; }

		public Color CurrentColor
		{
			get => currentColor;
			set
			{
				if (currentColor != value)
				{
					currentColor = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentColor"));
				}
			}
		}

		private Visibility colorPickerVisibility;

		private string name;

		private Color currentColor;

		public ColorMode()
		{
			CurrentColor = Colors.Black;
			name = GetType().Name;
		}

		public virtual async Task UpdateAsync(string address, bool force = false)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(address))
					return;

				if (!force && CurrentColor == LastColor)
					return;

				if (LastColor != CurrentColor)
				{
					LastColor = CurrentColor;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LastColor"));
				}

				var request = $"http://{address}/color?R={CurrentColor.R}&G={CurrentColor.G}&B={CurrentColor.B}";

				HttpWebRequest query = WebRequest.CreateHttp(request);
				query.KeepAlive = false;

				var response = await query.GetResponseAsync();

				using (var stream = response.GetResponseStream())
				using (var reader = new StreamReader(stream))
				{
					_ = await reader.ReadToEndAsync();
				}
			}
			catch { }
		}

		public virtual void Dispose()
		{
			// none
		}
	}
}
