using System.Diagnostics;

namespace PathFinding
{
    [DebuggerDisplay("({X}, {Y})")]
    public sealed class Node
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

        // just for diagnostics output
        public int X => this.Target.X;
        public int Y => this.Target.Y;

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

        public override bool Equals(object? obj)
        {
            return obj is Node n && this.Equals(n);
        }

        public override int GetHashCode()
        {
            return this.Target.GetHashCode();
        }
    }
}