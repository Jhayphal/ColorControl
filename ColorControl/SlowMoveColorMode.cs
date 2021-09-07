using System.Threading.Tasks;
using System.Windows.Media;

namespace ColorControl
{
	class SlowMoveColorMode : ColorMode
	{
		public override async Task UpdateAsync(string address)
		{
			var rgbled_r = (CurrentColor.R + 1) & 0xFF;
			var rgbled_g = (CurrentColor.G + 2) & 0xFF;
			var rgbled_b = (CurrentColor.B + 3) & 0xFF;

			CurrentColor = Color.FromRgb((byte)rgbled_r, (byte)rgbled_g, (byte)rgbled_b);

			await base.UpdateAsync(address);
		}
	}
}
