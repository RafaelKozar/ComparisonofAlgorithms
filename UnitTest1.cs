using System.Text.Json;

namespace TestProject1
{
    public class UnitTest1
    {
        private int number = 1000000;
        [Fact]
        public void Test1()
        { 
            //NearPoints(ReadJson(), 10);
            NearPoints(GenerateRandomPoints(number), 10);
        }

        [Fact]
        public void Test2()
        {
            //NearPoints2(ReadJson(), 10);
            NearPoints2(GenerateRandomPoints(number), 10);
        }

        [Fact]
        public void Test3()
        {
            //NearPoints3(ReadJson(), 10);
            NearPoints3(GenerateRandomPoints(number), 10);
        }



        public List<Point> ReadJson()
        {            
            string jsonData = File.ReadAllText("");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return  JsonSerializer.Deserialize<List<Point>>(jsonData, options);
        }

        public List<Point> GenerateRandomPoints(int count)
        {
            List<Point> points = new List<Point>();
            Random random = new Random();

            for (int i = 0; i < count; i++)
            {
                int x = random.Next(20000);
                int y = random.Next(20000);

                points.Add(new Point { x = x, y = y });
            }

            return points;
        }

        public List<Point> NearPoints (List<Point> points, int k)
        {            
            List<PointDistance> pointsDistance = new List<PointDistance>();
            foreach (Point p in points)
            {
                var distance =  Math.Sqrt((p.x*p.x)+(p.y*p.y));
                pointsDistance.Add(new PointDistance
                {
                    point = p,
                    distance = distance
                });
            }

            pointsDistance.Sort((a, b) => a.distance.CompareTo(b.distance));

            var distancesLess = pointsDistance.SkipLast(k).ToList();

            return distancesLess.Select(x => x.point).ToList();
        }

        //notation O = O(n) + O(n log n) + O(k)

        public List<Point> NearPoints2(List<Point> points, int k)
        {
            var pointTree = new SortedSet<PointDistance>(Comparer<PointDistance>.Create((x, y) => x.distance.CompareTo(y.distance)));            
            foreach (Point p in points)
            {
                var distance = Math.Sqrt((p.x * p.x) + (p.y * p.y));
                pointTree.Add(new PointDistance
                {
                    point = p,
                    distance = distance
                });                
            }            

            var distancesLess = pointTree.SkipLast(k).ToList();

            return distancesLess.Select(x => x.point).ToList();
        }

        //notation O = O(n) + O(n*log*n) + O(k)


        public List<Point> NearPoints3(List<Point> points, int k)
        {
            var pointTree = new SortedSet<PointDistance>(Comparer<PointDistance>.Create((x, y) => x.distance.CompareTo(y.distance)));
            foreach (Point p in points)
            {
                var distance = Math.Sqrt((p.x * p.x) + (p.y * p.y));
                pointTree.Add(new PointDistance
                {
                    point = p,
                    distance = distance
                });

                if (pointTree.Count > k)
                {
                    pointTree.Remove(pointTree.Max);
                }
            }

            var distancesLess = pointTree.SkipLast(k).ToList();

            return distancesLess.Select(x => x.point).ToList();
        }

        //notation O = O(n) + O(n*log*k) + O(k)


    }

    //    

    struct PointDistance
    {
        public Point point;
        public double distance;
    }

    public struct Point
    {
        public int x;
        public int y;
    }
}