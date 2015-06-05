using System.Windows.Forms;

namespace SCR
{
	public sealed class DoubleBufferedPanel: Panel
	{
		public DoubleBufferedPanel()
		{
			DoubleBuffered = true;
		}
	}
}
