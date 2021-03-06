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

        public Form1()
        {
            InitializeComponent();

            _goal = new Point(400, 10);
            _population = new Population(35000, _goal);

            new Thread(() =>
            {
                while (true)
                {
                    _population.Update();
                }
            }).Start();
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            using Bitmap bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using Graphics graphics = Graphics.FromImage(bitmap);
            graphics.FillRectangle(_backgroundBrush, 0, 0, bitmap.Width, bitmap.Height);
            foreach (var obstacle in Obstacles.Get())
            {
                graphics.FillRectangle(_obstacleBrush, obstacle.X, obstacle.Y, obstacle.Width, obstacle.Height);
            }
            
            graphics.FillEllipse(_goalBrush, _goal.X, _goal.Y, 8, 8);

            Text = _population.ToString();
            foreach (var dotPosition in _population.GetDotPositions())
            {
                if (dotPosition.IsBest)
                {
                    graphics.FillEllipse(_dotBrush, dotPosition.X, dotPosition.Y, 4, 4);
                }
                else
                {
                    graphics.DrawEllipse(_dotPen, dotPosition.X, dotPosition.Y, 4, 4);
                }
            }

            pictureBox1.Image?.Dispose();
            pictureBox1.Image = bitmap;
            pictureBox1.Refresh();
        }
    }
}
