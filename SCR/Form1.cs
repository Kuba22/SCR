using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SCR.Properties;

namespace SCR
{
	public partial class Form1 : Form
	{
		public Random Random = new Random();

		public int Resolution;
		public const int TeamSize = 3;
		public int PitchWidth;
		public int PitchLength;

		public Bitmap Bitmap;
		public Bitmap PitchBitmap;
		public Bitmap BallImage;

		public Player Player;
		public Pitch FootballPitch;
		public Ball Ball;

		public Form1()
		{
			InitializeComponent();
			InitializePitch();
			InitializePlayersAndBall();
			DrawPitchInitial();
			panelPitch.Show();
		}

		private void InitializePitch()
		{
			PitchWidth = 10;
			PitchLength = 2 * PitchWidth;

			Bitmap = new Bitmap(panelPitch.Width, panelPitch.Height);

			FootballPitch = Pitch.GetInstance(PitchWidth, PitchLength);

			Resolution = panelPitch.Width / PitchWidth;

			for (var i = 0; i < FootballPitch.Width; i++)
			{
				for (var j = 0; j < FootballPitch.Length; j++)
				{
					FootballPitch.FieldRectanglePairs[i, j] = new FieldRectanglePair(i, j, Resolution);
				}
			}
		}

		private void InitializePlayersAndBall()
		{
			Player = new Player(Random, FootballPitch) { Location = new Location(5, 5) };

			Player.FootballPitch.GetField(Player.Location).Field.FieldType = OccupiedBy.PlayerTeamDark;
			Player.Team = Team.Dark;

			Player.Thread.Start();

			Ball = new Ball(Random, FootballPitch) { Location = new Location(PitchWidth / 2, PitchLength / 2) };
			Ball.FootballPitch.GetField(Ball.Location).Field.FieldType = OccupiedBy.Ball;
			BallImage = Resources.ball30;
			BallImage.MakeTransparent();
		}

		private void DrawPitchInitial()
		{
			using (var g = Graphics.FromImage(Bitmap))
			{
				foreach (var fr in FootballPitch.FieldRectanglePairs)
				{
					g.FillRectangle(fr.Brush, fr.Rectangle);
				}
				DrawLines(g);
			}
		}

		private void DrawLines(Graphics g)
		{
			var region = new Region(new Rectangle(0, 0, panelPitch.Width, panelPitch.Height));
			var graphicsPath = new GraphicsPath();
			graphicsPath.AddPolygon(
				new[]
				{
					new Point(4, 4),
					new Point(panelPitch.Width - 4, 4),
					new Point(panelPitch.Width - 4, panelPitch.Height - 4),
					new Point(4, panelPitch.Height - 4)
				});
			region.Exclude(graphicsPath);
			g.FillRegion(new SolidBrush(Color.White), region);

			g.DrawLine(new Pen(Color.White, 4), 0, panelPitch.Height / 2, panelPitch.Width, panelPitch.Height / 2);
			g.DrawEllipse(new Pen(Color.White, 4),
				new Rectangle(panelPitch.Width / 4, panelPitch.Height / 2 - panelPitch.Width / 4, panelPitch.Width / 2, panelPitch.Width / 2));
		}

		private void DrawPitch()
		{
			PitchBitmap = (Bitmap)Bitmap.Clone();
			using (var g = Graphics.FromImage(PitchBitmap))
			{
				DrawBallAndPlayers();
				DrawLines(g);
			}
		}

		private void DrawBallAndPlayers()
		{
			using (var g = Graphics.FromImage(PitchBitmap))
			{
				g.DrawImage(BallImage, Resolution * Ball.Location.X, Resolution * Ball.Location.Y);

				g.FillRectangle(new SolidBrush(Player.Team == Team.Light ? Color.LightSlateGray : Color.Blue),
						new Rectangle(
							Resolution * Player.Location.X,
							Resolution * Player.Location.Y,
							Resolution,
							Resolution
							));
			}
		}

		private void panelPitch_Paint(object sender, PaintEventArgs e)
		{
			try
			{
				DrawPitch();
				panelPitch.BackgroundImage = PitchBitmap;
			}
			catch (Exception exception)
			{
				Console.WriteLine(@"Painting error: {0}", exception.Message);
			}
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			Player.Stop();
			Ball.Stop();
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			panelPitch.Invalidate();
		}
	}
}
