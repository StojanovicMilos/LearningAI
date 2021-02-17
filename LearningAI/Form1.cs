using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace LearningAI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Click(object sender, System.EventArgs e)
        {
            var goal = new Point(400, 10);
            Graphics graphics = pictureBox1.CreateGraphics();
            var population = new SynchronizingPopulation(new Population(1000, graphics, goal));

            var goalBrush = new SolidBrush(Color.Green);
            var obstacleBrush = new SolidBrush(Color.Red);

            Thread thread = new Thread(population.Update);
            thread.Start();

            while (true)
            {
                pictureBox1.Refresh();
                graphics.FillRectangle(obstacleBrush, 0, 300, 600, 10);
                graphics.FillRectangle(obstacleBrush, 200, 500, 610, 10);
                graphics.FillEllipse(goalBrush, goal.X, goal.Y, 8, 8);

                Text = population.ToString();
                population.Show();

            }
        }
    }
}
