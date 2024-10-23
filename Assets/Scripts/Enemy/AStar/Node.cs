using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public bool IsTraversable { get; set; }
    public Node Parent { get; set; }
    public int FCost { get; private set; }

    public Node(Node parent, int x, int y, bool isTraversable)
    {
        X = x;
        Y = y;
        Parent = parent;
        IsTraversable = isTraversable;
    }

    public int[] GetPosition()
    {
        return new int[] { X, Y }; // Replace with vector2d in unity
    }

    public void EvaluateCost(Node goal)
    {
        int gCost = GetGCost(this);
        int x = X - goal.X;
        int y = Y - goal.Y;
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