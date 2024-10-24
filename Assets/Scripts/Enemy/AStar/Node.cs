using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector2Int Position;
    public bool IsTraversable { get; set; }
    public Node Parent { get; set; }
    public int FCost { get; private set; }

    public Node(Node parent, Vector2Int position, bool isTraversable)
    {
        Position = position;
        Parent = parent;
        IsTraversable = isTraversable;
    }

    public void EvaluateCost(Node goal)
    {
        int gCost = GetGCost(this);
        int x = (int)(Position.x - goal.Position.x);
        int y = (int)(Position.y - goal.Position.y);
        int hCost = (int)Math.Floor(Math.Sqrt(x * x + y * y)); // get euclidian distance to goal

        FCost = gCost + hCost;
    }

    // Untested
    public int GetGCost(Node current, int cost = 0)
    {
        if (current.Parent != null)
        {
            return GetGCost(current.Parent, cost + 1);
        }

        else
        {
            return cost;
        }
    }
}