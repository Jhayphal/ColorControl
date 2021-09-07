using System;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ColorControl
{
	class FlashColorMode : ColorMode
	{
		private double shift = 0.01D;

		public FlashColorMode() : base()
		{
			CurrentColor = Color.FromRgb(128, 128, 128);
		}

		public override async Task UpdateAsync(string address)
		{
			var led_r = Math.Sin(shift * CurrentColor.R) * 255D;
			var led_g = Math.Sin(shift * CurrentColor.G) * 255D;
			var led_b = Math.Sin(shift * CurrentColor.B) * 255D;

			shift += 0.001D;

			if (shift > 0.1D)
				shift = 0.01D;

			CurrentColor = Color.FromRgb((byte)led_r, (byte)led_g, (byte)led_b);

			await base.UpdateAsync(address);
		}
	}
}
