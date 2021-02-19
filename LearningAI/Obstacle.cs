namespace LearningAI
{
    public class Obstacle
    {
        public uint X { get; }
        public uint Y { get; }
        public uint Width { get; }
        public uint Height { get; }
        private readonly uint _x1;
        private readonly uint _y1;

        public Obstacle(uint x, uint y, uint width, uint height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            _x1 = x + width;
            _y1 = y + height;
        }

        public bool Hits(DotPosition dotPosition) => dotPosition.X > X && dotPosition.X < _x1 && dotPosition.Y > Y && dotPosition.Y < _y1;
    }
}