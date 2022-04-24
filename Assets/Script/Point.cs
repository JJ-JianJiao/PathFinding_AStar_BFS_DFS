using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point
{
    private int _x;
    private int _y;

    public Point(int x, int y)
    {
        _x = x;
        _y = y;
    }

    public int X { get => _x; set => _x = value; }
    public int Y { get => _y; set => _y = value; }
}
