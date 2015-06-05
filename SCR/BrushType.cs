using System.Drawing;

namespace SCR
{
	public sealed class BrushType: Brush
	{
		public static readonly BrushType LightBlue = new BrushType(Color.LightBlue);
		public static readonly BrushType DarkBlue = new BrushType(Color.Blue);

		public readonly Color Color;

		private BrushType(Color color)
		{
			Color = color;
		}

		public override object Clone()
		{
			return new BrushType(Color);
		}
	}
}
