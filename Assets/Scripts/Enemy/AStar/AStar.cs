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

            if (current.Position.x == goal.Position.x && current.Position.y == goal.Position.y)
            {
                break;
            }

            foreach (Node neighbor in FindNeighbors(map, current))
            {
                if (!map[neighbor.Position.x, neighbor.Position.y].IsTraversable || close.Contains(neighbor))
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

        if (current.Position.x == goal.Position.x && current.Position.y == goal.Position.y)
        {
            return SetPath(current);
        }

        return null;
    }

    private Node[] FindNeighbors(Node[,] map, Node node)
    {
        List<Node> neighbors = new List<Node>();

        if (map.GetLength(0) - 1 != node.Position.x)
        {
            neighbors.Add(map[node.Position.x + 1, node.Position.y]);
        }

        if (node.Position.x != 0)
        {
            neighbors.Add(map[node.Position.x - 1, node.Position.y]);
        }

        if (map.GetLength(1) - 1 != node.Position.y)
        {
            neighbors.Add(map[node.Position.x, node.Position.y + 1]);
        }

        if (node.Position.y != 0)
        {
            neighbors.Add(map[node.Position.x, node.Position.y - 1]);
        }

        // Remove non-traversable neighbors
        foreach (Node neighbor in neighbors.ToList())
        {
            if (!map[neighbor.Position.x, neighbor.Position.y].IsTraversable)
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
