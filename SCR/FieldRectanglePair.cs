using System.Drawing;

namespace SCR
{
	public class FieldRectanglePair
	{
		public Field Field { get; set; }
		public Rectangle Rectangle { get; set; }
		public Brush Brush { get; set; }

		public FieldRectanglePair(int x, int y, int resolution)
		{
			Field = new Field(new Location(x, y));
			Rectangle = new Rectangle(resolution*x, resolution*y, resolution, resolution);
			Brush = new SolidBrush((x + y)%2 == 0 ? Color.FromArgb(255, 0, 255, 0) : Color.LightGreen);
		}
	}
}
