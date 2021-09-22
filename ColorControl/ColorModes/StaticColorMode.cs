using System.Windows;
using System.Windows.Media;

namespace ColorControl.ColorModes
{
	class StaticColorMode : ColorMode
	{
		public StaticColorMode() : base()
		{
			ColorPickerVisibility = Visibility.Visible;

			CurrentColor = Colors.OrangeRed;

			Name = "Static color";
		}
	}
}
