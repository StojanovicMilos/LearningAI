﻿using System.Diagnostics;
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
        private bool _theEnd;


        private Bitmap _bmpLive = new Bitmap(1,1);
        private Bitmap _bmpLast = new Bitmap(1,1);

        public Form1()
        {
            InitializeComponent();

            _goal = new Point(400, 20);
            _population = new Population(10000, _goal, PopulationFactory.GetGeneration());

            new Thread(() =>
            {
                while (!_theEnd)
                {
                    _population.Update();
                }
            }).Start();

            new Thread(Render).Start();
        }

        private void Render()
        {
            const int maxFps = 100;
            const int minFramePeriodMilliseconds = 1000 / maxFps;

            Stopwatch stopwatch = Stopwatch.StartNew();
            while (!_theEnd)
            {
                // Render on the "live" Bitmap
                RenderLiveBitmap();

                // Lock and update the "display" Bitmap
                lock (_bmpLast)
                {
                    _bmpLast.Dispose();
                    _bmpLast = (Bitmap)_bmpLive.Clone();
                }

                pictureBox1.BeginInvoke((MethodInvoker)delegate 
                {
                    Text = _population.ToString();
                    pictureBox1.Image?.Dispose();
                    pictureBox1.Image = (Bitmap)_bmpLast.Clone();
                    pictureBox1.Refresh();
                });

                // FPS limiter
                var msToWait = minFramePeriodMilliseconds - stopwatch.ElapsedMilliseconds;
                if (msToWait > 0)
                    Thread.Sleep((int)msToWait);
                stopwatch.Restart();
            }
        }

        private void RenderLiveBitmap()
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

        private void button1_Click(object sender, System.EventArgs e)
        {
            _theEnd = true;
            _population.Save();
            Close();
        }
    }
}
