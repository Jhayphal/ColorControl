using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ColorControl
{
	abstract class ColorMode : INotifyPropertyChanged
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
			currentColor = Colors.Black;
			name = GetType().Name;
		}

		public virtual async Task UpdateAsync(string address)
		{
			if (string.IsNullOrWhiteSpace(address))
				return;

			var request = $"http://{address}/color?R={CurrentColor.R}&G={CurrentColor.G}&B={CurrentColor.B}";

			try
			{
				using (WebClient client = new WebClient())
					await client.UploadStringTaskAsync(request, string.Empty);
			}
			catch { }

			//HttpWebRequest query = WebRequest.CreateHttp(request);
			//query.Timeout = 1;
			//try
			//{
			//	query.GetResponse();
			//}
			//catch { }
			//finally
			//{
			//	query.Abort();
			//}
		}
	}
}
