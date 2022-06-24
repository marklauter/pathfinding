﻿namespace PathFinding
{
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