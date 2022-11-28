using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PathNodeState
{
    NodeNone = -1,
    NodeOpened,
    NodeClosed
}

public class PathNode
{
    public GameObject go;
    public List<PathNode> neighbors;
    public int floor;
    public bool link;

    public Vector3 pos;
    public PathNode parent;
    public float costFromStart;
    public float costToGoal;
    public float totalCost;

    public PathNodeState nodeState;
}
