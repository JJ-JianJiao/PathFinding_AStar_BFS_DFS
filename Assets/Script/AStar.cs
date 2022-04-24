using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar {

    const int D = 10;

    private enum MoveTypes { 
        CROSS,
        Diagonal
    }
    private List<NodeAStar> openSet;
    private List<NodeAStar> closeSet;
    private List<NodeAStar> TraverseNodes;
    private int[,] map;
    private int mapRow;
    private int mapCol;
    private NodeAStar startNode;
    private NodeAStar endNode;
    private MoveTypes moveTypes;
    private Dictionary<string, string> childParent;

    public Astar(Point s, Point e, int[,] m) {
        map = m;
        mapRow = map.GetLength(0);
        mapCol = map.GetLength(1);
        startNode = new NodeAStar(s,0);
        startNode.SetParent();
        endNode = new NodeAStar(e);
        openSet = new List<NodeAStar>();
        closeSet = new List<NodeAStar>();
        TraverseNodes = new List<NodeAStar>();
        childParent = new Dictionary<string, string>();
        moveTypes = MoveTypes.CROSS;
    }

    public List<NodeAStar> GetTraversNodes() {
        return TraverseNodes;
    }

    public void SetMoveType(int t) {
        switch (t)
        {
            case 0:
                moveTypes = MoveTypes.CROSS;
                break;
            case 1:
                moveTypes = MoveTypes.Diagonal;
                break;
            default:
                break;
        }
    }

    private bool IsInNodeList(NodeAStar n, List<NodeAStar> nodes) {
        foreach (NodeAStar node in nodes)
        {
            if (n.Vertex.X == node.Vertex.X && n.Vertex.Y == node.Vertex.Y) {
                return true;
            }
        }
        return false;
    }

    public List<string> StartAstarAsc() {
        //Init openSet and closeSet list
        openSet.Clear();
        closeSet.Clear();
        TraverseNodes.Clear();
        openSet.Add(startNode);
        TraverseNodes.Add(startNode);
        while (true) {
            int index = SelectNodeInOpenListAsc();
            NodeAStar currentNode = openSet[index];
            closeSet.Add(currentNode);
            openSet.RemoveAt(index);

            //TODO: check the current Node is EndNode
            if (IsEndNode(currentNode)) {
                break;
            }
            ProcessNodeNeighbors(currentNode);           
        }

        PrintNodesInfo(openSet, "OpenSet");
        PrintNodesInfo(closeSet, "CloseSet");
        PrintNodesInfo(TraverseNodes, "TotalTraverse");

        foreach (var item in childParent)
        {
            Console.WriteLine("child = {0}, + Parent = {1}", item.Key, item.Value);
        }
        //return GetPathInOrder();
        return GetPathInOrder();
    }

    public List<string> StartAstarDesc()
    {
        //Init openSet and closeSet list
        openSet.Clear();
        closeSet.Clear();
        TraverseNodes.Clear();
        openSet.Add(startNode);
        TraverseNodes.Add(startNode);
        while (true)
        {
            int index = SelectNodeInOpenListDesc();
            if (index == -1)
            {
                return null;
            }
            NodeAStar currentNode = openSet[index];

            closeSet.Add(currentNode);
            //test
            string key = currentNode.Vertex.X.ToString() + "-" + currentNode.Vertex.Y.ToString();
            string value = null;
            if (currentNode.Parent != null)
            {
                value = currentNode.Parent.X.ToString() + "-" + currentNode.Parent.Y.ToString();
            }
            else {
                value = null;
            }
            childParent.Add(key, value);
            openSet.RemoveAt(index);

            //TODO: check the current Node is EndNode
            if (IsEndNode(currentNode))
            {
                break;
            }
            ProcessNodeNeighbors(currentNode);
        }

        PrintNodesInfo(openSet,"OpenSet");
        PrintNodesInfo(closeSet, "CloseSet");
        PrintNodesInfo(TraverseNodes, "TotalTraverse");

        foreach (var item in childParent)
        {
            Console.WriteLine("child = {0}, + Parent = {1}", item.Key,item.Value);
        }
        //return GetPathInOrder();
        return GetPathInOrder();
    }

    private List<string> GetPathInOrder()
    {
        List<string> path = new List<string>();
        string endPoint = endNode.Vertex.X.ToString() + "-" + endNode.Vertex.Y.ToString();
        while (endPoint != null) {
            path.Add(endPoint);
            Console.Write("( " + endPoint + " ) ->");
            endPoint = childParent[endPoint];
        }
        path.Reverse();
        return path;
    }

    private void ProcessNodeNeighbors(NodeAStar currentNode)
    {
        int x = currentNode.Vertex.X;
        int y = currentNode.Vertex.Y;

        switch (moveTypes)
        {
            case MoveTypes.Diagonal:
                //check the left Node
                if (y - 1 >= 0 && map[x, y - 1] == 0)
                {
                    NodeAStar leftNode = new NodeAStar(x, y - 1);
                    ProcessNode(leftNode, currentNode);
                }
                //check the bottom Node
                if (x + 1 < mapRow && map[x + 1, y] == 0)
                {
                    NodeAStar bottomNode = new NodeAStar(x + 1, y);
                    ProcessNode(bottomNode, currentNode);
                }
                //check the right Node
                if (y + 1 < mapCol && map[x, y + 1] == 0)
                {
                    NodeAStar rightNode = new NodeAStar(x, y + 1);
                    ProcessNode(rightNode, currentNode);
                }
                //check the Upper Node
                if (x - 1 >= 0 && map[x - 1, y] == 0)
                {
                    NodeAStar UpperNode = new NodeAStar(x - 1, y);
                    ProcessNode(UpperNode, currentNode);
                }
                //check the leftBottom
                if ( (y - 1 >= 0 && x + 1 < mapRow )&& map[x + 1, y - 1] == 0)
                {
                    NodeAStar leftBottomNode = new NodeAStar(x + 1, y - 1);
                    ProcessNode(leftBottomNode, currentNode, 14);
                }
                //check the rightBottom
                if ( (y + 1 < mapCol  && x + 1 < mapRow) && map[x + 1, y + 1] == 0)
                {
                    NodeAStar rightBottomNode = new NodeAStar(x + 1, y + 1);
                    ProcessNode(rightBottomNode, currentNode, 14);
                }
                //check the rightUpper
                if ((y + 1 < mapCol && x - 1 >=0) && map[x - 1, y + 1] == 0)
                {
                    NodeAStar rightUpperNode = new NodeAStar(x - 1, y + 1);
                    ProcessNode(rightUpperNode, currentNode, 14);
                }
                //check the leftUpper
                if ((y - 1 >= 0 && x - 1 >= 0) && map[x - 1, y - 1] == 0)
                {
                    NodeAStar leftUpperNode = new NodeAStar(x - 1, y - 1);
                    ProcessNode(leftUpperNode, currentNode, 14);
                }
                break;
            case MoveTypes.CROSS:
                //check the left Node
                if (y - 1 >= 0 && map[x, y - 1] == 0)
                {
                    NodeAStar leftNode = new NodeAStar(x, y - 1);
                    ProcessNode(leftNode, currentNode);
                }
                //check the bottom Node
                if (x + 1 < mapRow && map[x + 1, y] == 0)
                {
                    NodeAStar bottomNode = new NodeAStar(x + 1, y);
                    ProcessNode(bottomNode, currentNode);
                }
                //check the right Node
                if (y + 1 < mapCol && map[x, y + 1] == 0)
                {
                    NodeAStar rightNode = new NodeAStar(x, y + 1);
                    ProcessNode(rightNode, currentNode);
                }
                //check the Upper Node
                if (x - 1 >= 0 && map[x - 1, y] == 0)
                {
                    NodeAStar UpperNode = new NodeAStar(x - 1, y);
                    ProcessNode(UpperNode, currentNode);
                }
                break;
            default:
                break;
        }
        //if (true)
        //{
        //    //check the left Node
        //    if (y - 1 >= 0 && map[x, y - 1] == 0) {
        //        Node leftNode = new Node(x, y - 1);
        //        ProcessNode(leftNode, currentNode);
        //    }
        //    //check the bottom Node
        //    if (x + 1 < mapRow && map[x + 1, y] == 0) {
        //        Node bottomNode = new Node(x + 1, y);
        //        ProcessNode(bottomNode, currentNode);
        //    }
        //    //check the right Node
        //    if (y +1  < mapCol && map[x, y + 1] == 0)
        //    {
        //        Node rightNode = new Node(x , y + 1);
        //        ProcessNode(rightNode, currentNode);
        //    }
        //    //check the Upper Node
        //    if (x - 1  >= 0 && map[x-1, y] == 0)
        //    {
        //        Node UpperNode = new Node(x -1 , y);
        //        ProcessNode(UpperNode, currentNode);
        //    }
        //}
        ////TODO diagonal distance
        //else { 
            
        //}
    }

    private void ProcessNode(NodeAStar node, NodeAStar currentNode,int moveDisctance = 10)
    {
        if (IsInCLoseSet(node)) {
            return;
        }

        if (!IsInOpenSet(node)) {
            int weight = TotalCost(node, moveDisctance);
            node.SetParent(currentNode);
            node.Weight = weight;
            openSet.Add(node);
            TraverseNodes.Add(node);
        }
    }

    private int TotalCost(NodeAStar node, int oneMoveWeight)
    {
        return oneMoveWeight + Math.Abs(node.Vertex.X - endNode.Vertex.X) * 10 + Math.Abs(node.Vertex.Y - endNode.Vertex.Y) * 10;
    }


    private bool IsInCLoseSet(NodeAStar node)
    {
        return IsInNodeList(node, closeSet);
    }
    private bool IsInOpenSet(NodeAStar node)
    {
        return IsInNodeList(node, openSet);
    }

    private bool IsEndNode(NodeAStar currentNode)
    {
        if (currentNode.Vertex.X == endNode.Vertex.X && currentNode.Vertex.Y == endNode.Vertex.Y) {
            return true;
        }
        return false;
    }

    private int SelectNodeInOpenListAsc()
    {
        int minValue = Int32.MaxValue;
        int index = -1;
        for (int i = 0; i < openSet.Count; i++)
        {
            //minValue = openSet[i].Weight < minValue ? openSet[index].Weight : minValue;
            if (openSet[i].Weight < minValue) {
                index = i;
                minValue = openSet[i].Weight;
            }
        }
        return index; 
    }

    private int SelectNodeInOpenListDesc()
    {
        int minValue = Int32.MaxValue;
        int index = -1;
        for (int i = openSet.Count-1; i >= 0; i--)
        {
            //minValue = openSet[i].Weight < minValue ? openSet[index].Weight : minValue;
            if (openSet[i].Weight < minValue)
            {
                index = i;
                minValue = openSet[i].Weight;
            }
        }
        return index;
    }

    private void PrintNodesInfo(List<NodeAStar> nodes, string str) {
        Console.WriteLine(str + " number is " + nodes.Count.ToString());
        foreach (NodeAStar node in nodes)
        {
            Console.Write("(" + node.Vertex.X.ToString() + ", " + node.Vertex.Y.ToString() + " | " + node.Weight.ToString() + ") ");
        }
        Console.WriteLine();
    }
}
