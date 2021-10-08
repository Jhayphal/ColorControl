using DesktopDuplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ColorControl.ColorModes
{
	class ScreenColorMode : ColorMode
	{
		private DesktopDuplicator desktopDuplicator;
		private bool inited = false;

		public ScreenColorMode() : base()
		{
			Name = "Screen flow";

			try
			{
				desktopDuplicator = new DesktopDuplicator(0);

				inited = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		public override async Task UpdateAsync(string address, bool force = false)
		{
			if (!inited)
				return;

			DesktopFrame frame = null;
			
			try
			{
				frame = desktopDuplicator.GetLatestFrame();
			}
			catch
			{
				desktopDuplicator = new DesktopDuplicator(0);
				
				return;
			}

			if (frame != null)
			{
				using(var image = frame.DesktopImage)
					CurrentColor = GetColor(image);
			}

			await base.UpdateAsync(address, force);
		}

		private Color GetColor(System.Drawing.Bitmap image)
		{
			int width = image.Width;
			int height = image.Height;

			int xStep = width / 100;
			int yStep = height / 100;

			var counter = new List<System.Drawing.Color>();

			for (int x = 0; x < width; x += xStep)
				for (int y = 0; y < height; y += yStep)
					counter.Add(image.GetPixel(x, y));

			byte r = (byte)counter.Select(x => (int)x.R).Average();
			byte g = (byte)counter.Select(x => (int)x.G).Average();
			byte b = (byte)counter.Select(x => (int)x.B).Average();

			Color result = Color.FromArgb(byte.MaxValue, r, g, b);

			return result;
		}
	}
}
