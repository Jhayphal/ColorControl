using System.Threading.Tasks;
using System.Windows.Media;

namespace ColorControl
{
	class FlowColorMode : ColorMode
	{
		private bool rUp = true, gUp = true, bUp = true;

		public FlowColorMode() : base()
		{
			Name = "Color flow";
		}

		public override async Task UpdateAsync(string address)
		{
			var rgbled_r = (int)CurrentColor.R;
			var rgbled_g = (int)CurrentColor.G;
			var rgbled_b = (int)CurrentColor.B;

			if (rUp)
			{
				rgbled_r += 1;

				if (rgbled_r > byte.MaxValue)
				{
					rgbled_r -= 2;

					rUp = false;
				}
			}
			else
			{
				rgbled_r -= 1;

				if (rgbled_r < 0)
				{
					rgbled_r += 2;

					rUp = true;
				}
			}


			if (gUp)
			{
				rgbled_g += 2;

				if (rgbled_g > byte.MaxValue)
				{
					rgbled_g -= 3;

					gUp = false;
				}
			}
			else
			{
				rgbled_g -= 2;

				if (rgbled_g < 0)
				{
					rgbled_g += 3;

					gUp = true;
				}
			}

			if (bUp)
			{
				rgbled_b += 3;

				if (rgbled_b > byte.MaxValue)
				{
					rgbled_b -= 4;

					bUp = false;
				}
			}
			else
			{
				rgbled_b -= 3;

				if (rgbled_b < 0)
				{
					rgbled_b += 4;

					bUp = true;
				}
			}

			CurrentColor = Color.FromRgb((byte)rgbled_r, (byte)rgbled_g, (byte)rgbled_b);

			await base.UpdateAsync(address);
		}
	}
}
