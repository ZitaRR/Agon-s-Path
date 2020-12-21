using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Node
{
    public Node Parent { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public float Distance { get; set; }
    public float Cost { get; set; }
    public float Weight { get; set; }
    public float F
    {
        get
        {
            if (Distance != -1 && Cost != -1)
                return Distance + Cost;
            return -1;
        }
    }
    public bool Walkable { get; set; }
    public GameObject gameObject { get; set; }

    public Node(float x, float y, bool walkable, int weight = 1)
    {
        X = x;
        Y = y;
        Walkable = walkable;
        Weight = weight;
    }

    public Vector2 GetVectorInt()
        => new Vector2(X, Y);

    public override string ToString()
        => $"{X}, {Y}";
}
