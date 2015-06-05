using System;

namespace SCR
{
	public class Pitch
	{
		public object RedrawLock = new object();

		public int Length { get; private set; }

		public int Width { get; private set; }

		private static Pitch _instance;

		public FieldRectanglePair[,] FieldRectanglePairs;

		public static Pitch GetInstance(int width, int length)
		{
			if(_instance !=null)
				throw new Exception("Object already exists");
			_instance = new Pitch(width, length);
			return _instance;
		}

		private Pitch(int width, int length)
		{
			Width = width;
			Length = length;
			FieldRectanglePairs = new FieldRectanglePair[width, length];
		}

		public FieldRectanglePair GetField(Location location)
		{
			return FieldRectanglePairs[location.X, location.Y];
		}
	}
}
