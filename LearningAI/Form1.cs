using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace LearningAI
{
    public partial class Form1 : Form
    {
        private readonly SolidBrush _goalBrush = new SolidBrush(Color.Green);
        private readonly SolidBrush _obstacleBrush = new SolidBrush(Color.Red);
        private readonly SolidBrush _dotBrush = new SolidBrush(Color.Black);
        private readonly SolidBrush _backgroundBrush = new SolidBrush(Color.Gray);
        private readonly Pen _dotPen = new Pen(Color.YellowGreen);
        private readonly Point _goal;
        private readonly Population _population;


        private Bitmap _bmpLive = new Bitmap(1,1);
        private Bitmap _bmpLast = new Bitmap(1,1);

        public Form1()
        {
            InitializeComponent();

            _goal = new Point(400, 20);
            _population = new Population(10000, _goal);

            new Thread(() =>
            {
                while (true)
                {
                    _population.Update();
                }
            }).Start();

            new Thread(RenderForever).Start();
        }

        private void RenderForever()
        {
            int maxFPS = 100;
            int minFramePeriodMilliseconds = 1000 / maxFPS;

            Stopwatch stopwatch = Stopwatch.StartNew();
            while (true)
            {
                // Render on the "live" Bitmap
                Render();

                // Lock and update the "display" Bitmap
                lock (_bmpLast)
                {
                    _bmpLast.Dispose();
                    _bmpLast = (Bitmap)_bmpLive.Clone();
                }

                // FPS limiter
                var msToWait = minFramePeriodMilliseconds - stopwatch.ElapsedMilliseconds;
                if (msToWait > 0)
                    Thread.Sleep((int)msToWait);
                stopwatch.Restart();
            }
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            lock (_bmpLast)
            {
                Text = _population.ToString();
                pictureBox1.Image?.Dispose();
                pictureBox1.Image = (Bitmap)_bmpLast.Clone();
                pictureBox1.Refresh();
            }
        }

        private void Render()
        {
            _bmpLive?.Dispose();
            _bmpLive = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using Graphics graphics = Graphics.FromImage(_bmpLive);
            graphics.FillRectangle(_backgroundBrush, 0, 0, _bmpLive.Width, _bmpLive.Height);
            foreach (var obstacle in Obstacles.Get())
            {
                graphics.FillRectangle(_obstacleBrush, obstacle.X, obstacle.Y, obstacle.Width, obstacle.Height);
            }

            graphics.FillEllipse(_goalBrush, _goal.X - 4, _goal.Y - 4, 8, 8);

            foreach (var dotPosition in _population.GetDotPositions())
            {
                if (dotPosition.IsBest)
                {
                    graphics.FillEllipse(_dotBrush, dotPosition.X - DotPosition.Radius, dotPosition.Y - DotPosition.Radius, DotPosition.Diameter, DotPosition.Diameter);
                }
                else
                {
                    graphics.DrawEllipse(_dotPen, dotPosition.X - DotPosition.Radius, dotPosition.Y - DotPosition.Radius, DotPosition.Diameter, DotPosition.Diameter);
                }
            }
        }
    }
}
