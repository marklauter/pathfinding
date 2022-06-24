using System.Diagnostics;

namespace PathFinding
{
    public enum Neighbor
    {
        Left = 0,
        Right = 1,
        Top = 2,
        TopLeft = 3,
        TopRight = 4,
        Bottom = 5,
        BottomLeft = 6,
        BottomRight = 7,
    }

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
            return obj != null && obj is Point p && this.Equals(p);
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

    public class Node
        : IEquatable<Node>
    {
        public Node(
            Point origin,
            Point destination,
            Node? parent,
            Point target)
        {
            this.Parent = parent;
            this.Target = target;

            this.GCost = target.Distance(origin);
            this.HCost = target.Distance(destination);
            this.FCost = this.GCost + this.HCost;
        }

        public readonly Node? Parent;
        public readonly Point Target;

        // G + H
        public readonly double FCost;
        // distance from origin
        public readonly double GCost;
        // distance from destination
        public readonly double HCost;

        public bool Equals(Node? other)
        {
            return other != null &&
                this.Target.Equals(other.Target);
        }

        public override int GetHashCode()
        {
            return this.Target.GetHashCode();
        }
    }

    public readonly struct Grid
    {
        public Grid(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.Span = width * height;
        }

        public readonly HashSet<Point> Obstacles = new();
        public readonly int Height;
        public readonly int Width;
        public readonly int Span;

        public void AddObstacles(params Point[] points)
        {
            for (var i = 0; i < points.Length; ++i)
            {
                _ = this.Obstacles.Add(points[i]);
            }
        }

        public bool IsTraversable(Point point)
        {
            return
                point.X >= 0
                && point.Y >= 0
                && point.X < this.Width
                && point.Y < this.Height
                && !this.Obstacles.Contains(point);
        }
    }

    public static class Path
    {
        public static IEnumerable<Point> Find(Grid grid, Point origin, Point destination)
        {
            var open = new HashSet<Node>();
            var closed = new HashSet<Point>();

            var currentNode = new Node(origin, destination, null, origin);
            _ = open.Add(currentNode);

            while (true)
            {
                currentNode = open.MinBy(c => c.FCost);
                if (currentNode == null)
                {
                    throw new InvalidOperationException("shortest node not found");
                }

                _ = open.Remove(currentNode);
                _ = closed.Add(currentNode.Target);

                if (currentNode.Target == destination)
                {
                    return ResolvePath(currentNode);
                }

                for (var j = 0; j < 8; ++j)
                {
                    var neighbor = currentNode.Target
                        .GetNeighbor((Neighbor)j);

                    // if neighbor is closed or not traversable skip to next
                    if (closed.Contains(neighbor) || !grid.IsTraversable(neighbor))
                    {
                        continue;
                    }

                    var neighborNode = new Node(origin, destination, currentNode, neighbor);
                    if (open.Contains(neighborNode))
                    {
                        var previousCost = open.First(c => c.Target == neighbor);
                        if (neighborNode.FCost < previousCost.FCost)
                        {
                            _ = open.Remove(previousCost);
                        }
                    }

                    _ = open.Add(neighborNode);
                }
            }
        }

        private static List<Point> ResolvePath(Node node)
        {
            var path = new List<Point>();
            while (node.Parent != null)
            {
                path.Add(node.Target);
                node = node.Parent;
            }

            path.Add(node.Target);

            path.Reverse();

            return path;
        }
    }
}