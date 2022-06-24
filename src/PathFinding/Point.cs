using System.Diagnostics;

namespace PathFinding
{
    // todo: reimplement as Vector3 for Unity
    [DebuggerDisplay("({X}, {Y})")]
    public readonly struct Point
        : IEquatable<Point>
    {
        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public readonly int X;
        public readonly int Y;

        public Point GetNeighbor(Neighbor neighbor)
        {
            // todo: I have code to precompute the neighbors in the GameOfLife repo
            var x = this.X;
            var y = this.Y;
            switch (neighbor)
            {
                case Neighbor.Left:
                    x--;
                    break;
                case Neighbor.Right:
                    x++;
                    break;
                case Neighbor.Top:
                    y--;
                    break;
                case Neighbor.TopLeft:
                    y--;
                    x--;
                    break;
                case Neighbor.TopRight:
                    y--;
                    x++;
                    break;
                case Neighbor.Bottom:
                    y++;
                    break;
                case Neighbor.BottomLeft:
                    y++;
                    x--;
                    break;
                case Neighbor.BottomRight:
                    y++;
                    x++;
                    break;
                default:
                    break;
            }

            return new Point(x, y);
        }

        public double Distance(Point other)
        {
            var a = Math.Abs(other.X - this.X);
            var b = Math.Abs(other.Y - this.Y);
            return Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
        }

        public int ManhattanDistance(Point other)
        {
            var a = Math.Abs(other.X - this.X);
            var b = Math.Abs(other.Y - this.Y);
            return a + b;
        }

        public static bool operator ==(Point x, Point y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Point x, Point y)
        {
            return !x.Equals(y);
        }

        public override bool Equals(object? obj)
        {
            return obj is Point p && this.Equals(p);
        }

        public bool Equals(Point other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.X, this.Y);
        }
    }
}