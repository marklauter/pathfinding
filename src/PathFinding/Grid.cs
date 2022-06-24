using System.Diagnostics;

namespace PathFinding
{
    [DebuggerDisplay("({Width}, {Height})")]
    public readonly struct Grid
    {
        public Grid(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.Span = width * height;
            this.obstacles = new();
        }

        private readonly HashSet<Point> obstacles;
        public readonly int Height;
        public readonly int Width;
        public readonly int Span;

        public void AddObstacles(params Point[] points)
        {
            for (var i = 0; i < points.Length; ++i)
            {
                _ = this.obstacles.Add(points[i]);
            }
        }

        public bool IsTraversable(Point point)
        {
            return
                point.X >= 0
                && point.Y >= 0
                && point.X < this.Width
                && point.Y < this.Height
                && !this.obstacles.Contains(point);
        }
    }
}