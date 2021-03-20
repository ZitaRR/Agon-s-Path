using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class ASTAR
{
    public static Node[,] Nodes { get; private set; }
    public static BoundsInt Bounds { get; private set; }

    public static void SetTilemap(Tilemap map)
    {
        Bounds = map.cellBounds;
        TileBase[] tiles = map.GetTilesBlock(Bounds);
        Nodes = new Node[Bounds.size.x, Bounds.size.y];

        for (int x = Bounds.xMin; x < Bounds.xMax; x++)
        {
            for (int y = Bounds.yMin; y < Bounds.yMax; y++)
            {
                int xIndex = Mathf.Abs(x - Bounds.xMin);
                int yIndex = Mathf.Abs(y - Bounds.yMin);
                TileBase tile = tiles[xIndex + yIndex * Bounds.size.x];
                Nodes[xIndex, yIndex] = new Node(x + .5f, y + .5f, tile != null);
            }
        }
    }

    public static Stack<Node> FindPath(Vector2Int start, Vector2Int target)
    {
        int x = Mathf.Abs(Bounds.xMin) + start.x;
        int y = Mathf.Abs(Bounds.yMin) + start.y;
        Node nStart = Nodes[x, y];

        x = Mathf.Abs(Bounds.xMin) + target.x;
        y = Mathf.Abs(Bounds.yMin) + target.y;
        Node nTarget = Nodes[x, y];

        var path = new Stack<Node>();
        var opened = new List<Node>();
        var closed = new List<Node>();
        var adjacencies = new List<Node>();
        Node current = nStart;

        opened.Add(current);
        while (opened.Count != 0 && !closed.Exists(t => t.X == nTarget.X && t.Y == nTarget.Y))
        {
            current = opened[0];
            opened.Remove(current);
            closed.Add(current);
            adjacencies = GetAdjacentNodes(current);

            foreach (var node in adjacencies)
            {
                if (closed.Contains(node) || !node.Walkable)
                    continue;
                if (opened.Contains(node))
                    continue;

                node.Parent = current;
                node.Distance = Mathf.Abs(node.X - nTarget.X) + Mathf.Abs(node.Y - nTarget.Y);
                node.Cost = node.Weight + node.Parent.Cost;
                opened.Add(node);
                opened = opened.OrderBy(n => n.F).ToList();
            }
        }

        if (!closed.Exists(n => n.X == nTarget.X && n.Y == nTarget.Y))
        {
            Debug.Log($"Target [{nTarget}] does not exist");
            return new Stack<Node>();
        }

        Node temp = closed[closed.IndexOf(current)];
        if (temp is null)
        {
            Debug.Log("Current is not closed");
            return new Stack<Node>();
        }
        do
        {
            path.Push(temp);
            temp = temp.Parent;
        } while (temp != nStart && temp != null);

        path.Push(nStart);
        return path;
    }

    public static List<Node> GetAdjacentNodes(Node node)
    {
        var list = new List<Node>();
        int x = Mathf.Abs(Bounds.xMin) + (int)Mathf.Floor(node.X);
        int y = Mathf.Abs(Bounds.yMin) + (int)Mathf.Floor(node.Y);

        if (x < Nodes.GetLength(0) - 1)
            list.Add(Nodes[x + 1, y]);
        if (x > 1)
            list.Add(Nodes[x - 1, y]);
        if (y < Nodes.GetLength(1) - 1)
            list.Add(Nodes[x, y + 1]);
        if (y > 1)
            list.Add(Nodes[x, y - 1]);
        return list;
    }
}
