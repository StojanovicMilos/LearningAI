using System.Drawing;
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
            Run();
        }

        private void Run()
        {
            var goal = new Point(400, 10);
            Graphics graphics = pictureBox1.CreateGraphics();
            var population = new Population(1000, graphics, goal);
            var goalBrush = new SolidBrush(Color.Green);
            var obstacleBrush = new SolidBrush(Color.Red);

            while (true)
            {
                Text = $"Generation: {population.Generation}, iteration: {population.Iteration}";
                if (population.ShouldEndGeneration())
                {
                    population.CalculateFitness();
                    population.NaturalSelection();
                    population.MutateBabies();
                }
                else
                {
                    population.Update();

                    pictureBox1.Refresh();
                    graphics.FillRectangle(obstacleBrush, 0, 300, 600, 10);
                    graphics.FillRectangle(obstacleBrush, 200, 500, 610, 10);
                    graphics.FillEllipse(goalBrush, goal.X, goal.Y, 8, 8);

                    population.Show();
                }
            }
        }
    }
}
