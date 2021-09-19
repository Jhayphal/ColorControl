using System.Threading.Tasks;
using System.Windows.Media;

namespace ColorControl
{
	class FastMoveColorMode : ColorMode
	{
		public FastMoveColorMode() : base()
		{
			Name = "Fast move";
		}

		public override async Task UpdateAsync(string address, bool force = false)
		{
			CurrentColor = Color.FromRgb(
				(byte)((CurrentColor.R + 1) % 255),
				(byte)((CurrentColor.G + 3) % 255),
				(byte)((CurrentColor.B + 5) % 255));

			await base.UpdateAsync(address, force);
		}
	}
}
