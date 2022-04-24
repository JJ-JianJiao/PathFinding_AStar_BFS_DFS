using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private Point _vertex;

    public Node(Point point)
    {

        _vertex = point;

    }

    public List<Point> childrens = new List<Point>();
    public Point Vertex { get => _vertex; set => _vertex = value; }

    public void Print()
    {

        Debug.Log("Node(" + _vertex.X + ", " + _vertex.Y + ") ");
        Debug.Log("Childrens: ");
        for (int i = 0; i < childrens.Count; i++)
        {
            Debug.Log("(" + childrens[i].X + "," + childrens[i].Y + ") ");
        }
        //Console.WriteLine();
    }
}

public class NodeAStar
{
    private Point _vertex;
    private Point _parent;
    private int _weight;

    public Point Vertex { get => _vertex; }
    public Point Parent { get => _parent; }
    public int Weight { get => _weight; set => _weight = value; }

    public NodeAStar(Point p)
    {
        //_vertex.X = p.X;
        //_vertex.Y = p.Y;
        _vertex = new Point(p.X, p.Y);
        _parent = null;
        _weight = -1;
    }

    public NodeAStar(int X, int Y)
    {
        //_vertex.X = X;
        //_vertex.Y = Y;
        _vertex = new Point(X, Y);
        _parent = null;
        _weight = -1;
    }

    public NodeAStar(Point p, int w)
    {
        //_vertex.X = p.X;
        //_vertex.Y = p.Y;
        _vertex = new Point(p.X, p.Y);
        _parent = null;
        _weight = w;
    }

    public void SetParent(Point p)
    {
        //_parent.X = p.X;
        //_parent.Y = p.Y;
        _parent = new Point(p.X, p.Y);
    }

    public void SetParent()
    {
        //_parent.X = p.X;
        //_parent.Y = p.Y;
        _parent = null;
    }

    public void SetParent(NodeAStar n)
    {
        //_parent.X = n.Vertex.X;
        //_parent.Y = n.Vertex.Y;
        _parent = new Point(n.Vertex.X, n.Vertex.Y);
    }
}
