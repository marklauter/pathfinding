using PathFinding;

namespace PathFindingTests
{
    public class PointTests
    {
        [Fact]
        public void Point_Distance()
        {
            var p1 = new Point(0, 0);
            var p2 = new Point(1, 0);
            var d = p1.Distance(p2);
            Assert.Equal(1, d);

            p1 = new Point(0, 0);
            p2 = new Point(1, 1);
            d = p1.Distance(p2);
            Assert.Equal(1.4142, Math.Round(d, 4));
        }
    }

    public class Path_Tests
    {
        [Fact]
        public void Path_Find_NoObstacles()
        {
            var grid = new Grid(3, 3);
            var origin = new Point(0, 0);
            var destination = new Point(2, 2);
            var midpoint = new Point(1, 1);

            var points = PathFinding.Path.Find(grid, origin, destination);

            Assert.NotNull(points);
            Assert.Equal(3, points.Count());
            Assert.Contains(origin, points);
            Assert.Contains(midpoint, points);
            Assert.Contains(destination, points);
        }

        [Fact]
        public void Path_Find_Obstacles()
        {
            var grid = new Grid(3, 3);
            var origin = new Point(0, 0);
            var destination = new Point(2, 2);
            var midpoint = new Point(1, 1);

            grid.AddObstacles(midpoint);

            var points = PathFinding.Path.Find(grid, origin, destination);

            Assert.NotNull(points);
            Assert.Equal(4, points.Count());
            Assert.Contains(origin, points);
            Assert.DoesNotContain(midpoint, points);
            Assert.Contains(destination, points);
        }
    }
}