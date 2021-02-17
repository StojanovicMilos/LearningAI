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
        private readonly Pen _dotPen = new Pen(Color.Blue);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Click(object sender, System.EventArgs e)
        {
            var goal = new Point(400, 10);
            Mutex mutex = new Mutex();
            var population = new SynchronizingPopulation(new Population(50000, goal), mutex);

            Thread thread = new Thread(population.Update);
            thread.Start();

            while (true)
            {
                mutex.WaitOne();
                Draw(population, goal);
                mutex.ReleaseMutex();
                Thread.Sleep(15);
            }
        }

        private void Draw(IPopulation population, Point goal)
        {
            Bitmap bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.FillRectangle(_obstacleBrush, 0, 300, 600, 10);
            graphics.FillRectangle(_obstacleBrush, 200, 500, 610, 10);
            graphics.FillEllipse(_goalBrush, goal.X, goal.Y, 8, 8);

            Text = population.ToString();
            foreach (var dotPosition in population.GetDotPositions())
            {
                if (dotPosition.IsBest)
                {
                    graphics.FillEllipse(_dotBrush, dotPosition.X, dotPosition.Y, 8, 8);
                }
                else
                {
                    graphics.DrawEllipse(_dotPen, dotPosition.X, dotPosition.Y, 4, 4);
                }
            }

            pictureBox1.Image = bitmap;
            pictureBox1.Refresh();
        }
    }
}
