using System.Threading.Tasks;
using System.Windows.Media;

namespace ColorControl
{
	class StrobeLightMode : ColorMode
	{
		public StrobeLightMode() : base()
		{
			Name = "Strobe light";
		}

		public override async Task UpdateAsync(string address, bool force = false)
		{
			if (CurrentColor == Colors.Black)
				CurrentColor = Colors.White;
			else
				CurrentColor = Colors.Black;

			await base.UpdateAsync(address, force);
		}
	}
}
