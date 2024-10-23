using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStar
{
    public Node[] Path { get; private set; }

    public AStar(Node[,] map, Node start, Node goal)
    {
        Path = FindOptimalPath(map, start, goal);
    }

    private Node[] FindOptimalPath(Node[,] map, Node start, Node goal)
    {
        List<Node> open = new List<Node>(); // nodes to be evaluated
        List<Node> close = new List<Node>(); // nodes already evaluated

        Node current = start;

        open.Add(start);

        while (open.Count != 0)
        {
            open = open.OrderBy(n => n.FCost).ToList();
            current = open[0];

            open.Remove(current);
            close.Add(current);

            if (current.GetPosition()[0] == goal.GetPosition()[0] && current.GetPosition()[1] == goal.GetPosition()[1])
            {
                break;
            }

            foreach (Node neighbor in FindNeighbors(map, current))
            {
                if (!map[neighbor.X, neighbor.Y].IsTraversable || close.Contains(neighbor))
                {
                    continue;
                }

                if (neighbor.Parent == null)
                {
                    neighbor.Parent = current;
                }

                if (neighbor.GetGCost(neighbor) < current.GetGCost(current) || !open.Contains(neighbor))
                {
                    neighbor.EvaluateCost(goal);
                    neighbor.Parent = current;

                    if (!open.Contains(neighbor))
                    {
                        open.Add(neighbor);
                    }
                }
            }
        }

        if (current.GetPosition()[0] == goal.GetPosition()[0] && current.GetPosition()[1] == goal.GetPosition()[1])
        {
            return SetPath(current);
        }

        return null;
    }

    private Node[] FindNeighbors(Node[,] map, Node node)
    {
        List<Node> neighbors = new List<Node>();

        if (map.GetLength(0) - 1 != node.X)
        {
            neighbors.Add(map[node.X + 1, node.Y]);
        }

        if (node.X != 0)
        {
            neighbors.Add(map[node.X - 1, node.Y]);
        }

        if (map.GetLength(1) - 1 != node.Y)
        {
            neighbors.Add(map[node.X, node.Y + 1]);
        }

        if (node.Y != 0)
        {
            neighbors.Add(map[node.X, node.Y - 1]);
        }

        // Remove non-traversable neighbors
        foreach (Node neighbor in neighbors.ToList())
        {
            if (!map[neighbor.X, neighbor.Y].IsTraversable)
            {
                neighbors.Remove(neighbor);
            }
        }

        return neighbors.ToArray();
    }

    private Node[] SetPath(Node current)
    {
        List<Node> path = new List<Node>();

        path.Add(current);

        if (current.Parent != null)
        {
            path.AddRange(SetPath(current.Parent));
        }

        return path.ToArray();
    }
}
