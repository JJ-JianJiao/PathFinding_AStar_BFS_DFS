using System.Collections.Generic;


public class BFS_DFS_Algorithm {
    public List<Node> nodes;
    public Dictionary<string, string> dictionaryStr;

    public BFS_DFS_Algorithm() {
        nodes = new List<Node>();
        dictionaryStr = new Dictionary<string, string>();
    }

    public List<Node> TraverseMapFullPath(int[,] map, int XMax, int YMax)
    {
        List<Node> tempNodes = new List<Node>();
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] != 1)
                {
                    Point newPoint = new Point(i, j);
                    Node newNode = new Node(newPoint);
                    if ((newNode.Vertex.Y - 1 >= 0) && map[newNode.Vertex.X, newNode.Vertex.Y - 1] == 0)
                    {
                        Point left = new Point(newNode.Vertex.X, newNode.Vertex.Y - 1);
                        newNode.childrens.Add(left);
                    }
                    if ((newNode.Vertex.X - 1 >= 0) && map[newNode.Vertex.X - 1, newNode.Vertex.Y] == 0)
                    {
                        Point up = new Point(newNode.Vertex.X - 1, newNode.Vertex.Y);
                        newNode.childrens.Add(up);
                    }
                    if ((newNode.Vertex.Y + 1 < YMax) && map[newNode.Vertex.X, newNode.Vertex.Y + 1] == 0)
                    {
                        Point right = new Point(newNode.Vertex.X, newNode.Vertex.Y + 1);
                        newNode.childrens.Add(right);
                    }
                    if ((newNode.Vertex.X + 1 < XMax) && map[newNode.Vertex.X + 1, newNode.Vertex.Y] == 0)
                    {
                        Point bottom = new Point(newNode.Vertex.X + 1, newNode.Vertex.Y);
                        newNode.childrens.Add(bottom);
                    }
                    if (((newNode.Vertex.X - 1 >= 0) && newNode.Vertex.Y - 1 >= 0) && map[newNode.Vertex.X - 1, newNode.Vertex.Y - 1] == 0)
                    {
                        Point leftUp = new Point(newNode.Vertex.X - 1, newNode.Vertex.Y - 1);
                        newNode.childrens.Add(leftUp);
                    }

                    if ((newNode.Vertex.X - 1 >= 0 && newNode.Vertex.Y + 1 < YMax) && map[newNode.Vertex.X - 1, newNode.Vertex.Y + 1] == 0)
                    {
                        Point rightUp = new Point(newNode.Vertex.X - 1, newNode.Vertex.Y + 1);
                        newNode.childrens.Add(rightUp);
                    }

                    if ((newNode.Vertex.X + 1 < XMax && newNode.Vertex.Y - 1 >= 0) && map[newNode.Vertex.X + 1, newNode.Vertex.Y - 1] == 0)
                    {
                        Point leftBottom = new Point(newNode.Vertex.X + 1, newNode.Vertex.Y - 1);
                        newNode.childrens.Add(leftBottom);
                    }
                    if ((newNode.Vertex.X + 1 < XMax && newNode.Vertex.Y + 1 < YMax) && map[newNode.Vertex.X + 1, newNode.Vertex.Y + 1] == 0)
                    {
                        Point rightBottm = new Point(newNode.Vertex.X + 1, newNode.Vertex.Y + 1);
                        newNode.childrens.Add(rightBottm);
                    }
                    tempNodes.Add(newNode);
                }
            }

        }
        return tempNodes;
    }

    public List<Node> DFSTraverseMapFullPath(int[,] map, int XMax, int YMax)
    {
        List<Node> tempNodes = new List<Node>();
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] != 1)
                {
                    Point newPoint = new Point(i, j);
                    Node newNode = new Node(newPoint);

                    if (((newNode.Vertex.X - 1 >= 0) && newNode.Vertex.Y - 1 >= 0) && map[newNode.Vertex.X - 1, newNode.Vertex.Y - 1] == 0)
                    {
                        Point leftUp = new Point(newNode.Vertex.X - 1, newNode.Vertex.Y - 1);
                        newNode.childrens.Add(leftUp);
                    }
                    if ((newNode.Vertex.X - 1 >= 0) && map[newNode.Vertex.X - 1, newNode.Vertex.Y] == 0)
                    {
                        Point up = new Point(newNode.Vertex.X - 1, newNode.Vertex.Y);
                        newNode.childrens.Add(up);
                    }
                    if ((newNode.Vertex.X - 1 >= 0 && newNode.Vertex.Y + 1 < YMax) && map[newNode.Vertex.X - 1, newNode.Vertex.Y + 1] == 0)
                    {
                        Point rightUp = new Point(newNode.Vertex.X - 1, newNode.Vertex.Y + 1);
                        newNode.childrens.Add(rightUp);
                    }

                    if ((newNode.Vertex.Y + 1 < YMax) && map[newNode.Vertex.X, newNode.Vertex.Y + 1] == 0)
                    {
                        Point right = new Point(newNode.Vertex.X, newNode.Vertex.Y + 1);
                        newNode.childrens.Add(right);
                    }
                    if ((newNode.Vertex.X + 1 < XMax && newNode.Vertex.Y + 1 < YMax) && map[newNode.Vertex.X + 1, newNode.Vertex.Y + 1] == 0)
                    {
                        Point rightBottm = new Point(newNode.Vertex.X + 1, newNode.Vertex.Y + 1);
                        newNode.childrens.Add(rightBottm);
                    }
                    if ((newNode.Vertex.X + 1 < XMax) && map[newNode.Vertex.X + 1, newNode.Vertex.Y] == 0)
                    {
                        Point bottom = new Point(newNode.Vertex.X + 1, newNode.Vertex.Y);
                        newNode.childrens.Add(bottom);
                    }
                    if ((newNode.Vertex.X + 1 < XMax && newNode.Vertex.Y - 1 >= 0) && map[newNode.Vertex.X + 1, newNode.Vertex.Y - 1] == 0)
                    {
                        Point leftBottom = new Point(newNode.Vertex.X + 1, newNode.Vertex.Y - 1);
                        newNode.childrens.Add(leftBottom);
                    }
                    if ((newNode.Vertex.Y - 1 >= 0) && map[newNode.Vertex.X, newNode.Vertex.Y - 1] == 0)
                    {
                        Point left = new Point(newNode.Vertex.X, newNode.Vertex.Y - 1);
                        newNode.childrens.Add(left);
                    }
                    tempNodes.Add(newNode);
                }
            }

        }
        return tempNodes;
    }

    public List<Node> TraverseMapCrossPath(int[,] map, int XMax, int YMax)
    {
        List<Node> tempNodes = new List<Node>();
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] != 1)
                {
                    Point newPoint = new Point(i, j);
                    Node newNode = new Node(newPoint);
                    if ((newNode.Vertex.Y - 1 >= 0) && map[newNode.Vertex.X, newNode.Vertex.Y - 1] == 0)
                    {
                        Point left = new Point(newNode.Vertex.X, newNode.Vertex.Y - 1);
                        newNode.childrens.Add(left);
                    }
                    if ((newNode.Vertex.X - 1 >= 0) && map[newNode.Vertex.X - 1, newNode.Vertex.Y] == 0)
                    {
                        Point up = new Point(newNode.Vertex.X - 1, newNode.Vertex.Y);
                        newNode.childrens.Add(up);
                    }
                    if ((newNode.Vertex.Y + 1 < YMax) && map[newNode.Vertex.X, newNode.Vertex.Y + 1] == 0)
                    {
                        Point right = new Point(newNode.Vertex.X, newNode.Vertex.Y + 1);
                        newNode.childrens.Add(right);
                    }
                    if ((newNode.Vertex.X + 1 < XMax) && map[newNode.Vertex.X + 1, newNode.Vertex.Y] == 0)
                    {
                        Point bottom = new Point(newNode.Vertex.X + 1, newNode.Vertex.Y);
                        newNode.childrens.Add(bottom);
                    }
                    tempNodes.Add(newNode);
                }
            }
        }
        return tempNodes;
    }

    public Dictionary<string, string> BFS(List<Node> nodes, Point start)
    {
        Queue<Point> q = new Queue<Point>();

        q.Enqueue(start);
        List<Point> seenPoint = new List<Point>();
        seenPoint.Add(start);

        //Dictionary<Point,Point> tempDic = new Dictionary<Point, Point>();
        Dictionary<string, string> tempDic = new Dictionary<string, string>();

        //tempDic.Add(start,new Point(-1,-1));
        //tempDic.Add(start.X.ToString() + "," + start.Y.ToString(), null);
        tempDic.Add(start.X.ToString() + "-" + start.Y.ToString(), null);

        while (q.Count > 0)
        {
            Point vertex = q.Dequeue();
            Node currentNode = null;
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Vertex.X == vertex.X && nodes[i].Vertex.Y == vertex.Y)
                {

                    currentNode = nodes[i];
                    break;

                }
            }
            foreach (Point point in currentNode.childrens)
            {
                //if (!seenPoint.Contains(point)) {
                //    q.Enqueue(point);
                //    seenPoint.Add(point);                    
                //}
                bool isContains = false;
                for (int i = 0; i < seenPoint.Count; i++)
                {
                    if (seenPoint[i].X == point.X && seenPoint[i].Y == point.Y)
                    {
                        isContains = true;
                        break;
                    }
                }
                if (isContains == false)
                {

                    q.Enqueue(point);
                    seenPoint.Add(point);
                    //tempDic[point] = vertex;
                    //tempDic.Add(point, vertex);
                    tempDic.Add(point.X.ToString() + "-" + point.Y.ToString(), vertex.X.ToString() + "-" + vertex.Y.ToString());
                }
            }
            //Console.WriteLine(vertex.X + "," + vertex.Y);
            //Console.WriteLine(vertex.X.ToString()  + vertex.Y.ToString());
        }

        return tempDic;

    }

    public Dictionary<string, string> DFS(List<Node> nodes, Point start)
    {
        Stack<Point> s = new Stack<Point>();

        s.Push(start);
        List<Point> seenPoint = new List<Point>();
        seenPoint.Add(start);
        Dictionary<string, string> tempDic = new Dictionary<string, string>();
        tempDic.Add(start.X.ToString() + "-" + start.Y.ToString(), null);

        while (s.Count > 0)
        {
            Point vertex = s.Pop();
            Node currentNode = null;
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Vertex.X == vertex.X && nodes[i].Vertex.Y == vertex.Y)
                {
                    currentNode = nodes[i];
                    break;
                }
            }
            foreach (Point point in currentNode.childrens)
            {
                bool isContains = false;
                for (int i = 0; i < seenPoint.Count; i++)
                {
                    if (seenPoint[i].X == point.X && seenPoint[i].Y == point.Y)
                    {
                        isContains = true;
                        break;
                    }
                }
                if (isContains == false)
                {

                    s.Push(point);
                    seenPoint.Add(point);
                    tempDic.Add(point.X.ToString() + "-" + point.Y.ToString(), vertex.X.ToString() + "-" + vertex.Y.ToString());
                }
            }
        }
        return tempDic;
    }
}
