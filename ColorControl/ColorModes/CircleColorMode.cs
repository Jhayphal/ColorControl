using System.Threading.Tasks;
using System.Windows.Media;

namespace ColorControl
{
	class CircleColorMode : ColorMode
	{
		private int hue = 0;

		public CircleColorMode() : base()
		{
			Name = "Color circle";
		}

		public static Color HSLToRGB(int hue, float saturation, float lightness)
		{
			byte b;
			byte g;
			byte r;

			if (saturation == 0)
			{
				r = g = b = (byte)(lightness * 255);
			}
			else
			{
				float v1, v2;
				float h = (float)hue / 360;

				v2 = (lightness < 0.5) ? (lightness * (1 + saturation)) : (lightness + saturation - (lightness * saturation));
				v1 = 2 * lightness - v2;

				r = (byte)(255 * HueToRGB(v1, v2, h + (1.0f / 3)));
				g = (byte)(255 * HueToRGB(v1, v2, h));
				b = (byte)(255 * HueToRGB(v1, v2, h - (1.0f / 3)));
			}

			return Color.FromRgb(r, g, b);
		}

		private static float HueToRGB(float v1, float v2, float vH)
		{
			if (vH < 0)
				vH += 1;

			if (vH > 1)
				vH -= 1;

			if ((6 * vH) < 1)
				return v1 + (v2 - v1) * 6 * vH;

			if ((2 * vH) < 1)
				return v2;

			if ((3 * vH) < 2)
				return v1 + (v2 - v1) * ((2.0f / 3) - vH) * 6;

			return v1;
		}

		public override async Task UpdateAsync(string address)
		{
			hue = (hue + 1) % 360;

			CurrentColor = HSLToRGB(hue, 1f, 0.5f);

			await base.UpdateAsync(address);
		}
	}
}
