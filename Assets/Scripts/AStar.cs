using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    WayPointTerrain currentTerrain;
    List<PathNode> openList;
    List<PathNode> closeList;

    List<Vector3> pathList;

    static public AStar instance;

    public void Init(WayPointTerrain terrain)
    {
        currentTerrain = terrain;
        openList = new List<PathNode>();
        closeList = new List<PathNode>();
        pathList = new List<Vector3>();
        instance = this;
    }

    public List<Vector3> GetPath()
    {
        return pathList;
    }

    private bool CheckCloseList(PathNode node)
    {
        foreach (PathNode n in closeList)
        {
            if (n == node)
            {
                return true;
            }
        }
        return false;
    }

    private PathNode GetBestNode()
    {
        PathNode bestNode = null;
        float max = 100000.0f;
        foreach (PathNode n in openList)
        {
            if (n.totalCost < max)
            {
                max = n.totalCost;
                bestNode = n;
            }
        }

        openList.Remove(bestNode);

        return bestNode;
    }

    public bool PerformAStar(Vector3 startPos, Vector3 endPos)
    {
        int startFloor = 0;
        int endFloor = 0;
        if (startPos.y > 4)
        {
            startFloor = 1;//2樓
        }
        if (endPos.y > 4)
        {
            endFloor = 1;
        }
        PathNode startNode = currentTerrain.GetNodeFromPosition(startPos, startFloor);
        PathNode endNode = currentTerrain.GetNodeFromPosition(endPos, endFloor);
        if(startNode == null || endNode == null)
        {
            // 地圖中找不到node
            return false;
        }
        else if (startNode == endNode)
        {
            BuildPath(startPos, endPos, startNode, endNode);
            return true;
        }

        openList.Clear();
        closeList.Clear();
        currentTerrain.ClearAStarInfo();
        PathNode neighborNode;
        PathNode currentNode;
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            currentNode = GetBestNode();
            if (currentNode == null)
            {
                //無法取得bestNode
                return false;
            }
            else if (currentNode == endNode)
            {
                BuildPath(startPos, endPos, startNode, endNode);
                return true;
            }

            int neighborCount = currentNode.neighbors.Count;
            Vector3 distance;
            for (int i = 0; i < neighborCount; i++)
            {
                neighborNode = currentNode.neighbors[i];
                if (neighborNode.nodeState == PathNodeState.NodeClosed)
                {
                    continue;
                }
                else if (neighborNode.nodeState == PathNodeState.NodeOpened)
                {
                    distance = currentNode.pos - neighborNode.pos;
                    float newCostFromStart = currentNode.costFromStart + distance.magnitude;
                    if(newCostFromStart < neighborNode.costFromStart)
                    {
                        neighborNode.costFromStart = newCostFromStart;
                        neighborNode.totalCost = neighborNode.costFromStart + neighborNode.costToGoal;
                        neighborNode.parent = currentNode;
                    }
                    continue;
                }
                neighborNode.nodeState = PathNodeState.NodeOpened;
                distance = currentNode.pos - neighborNode.pos;
                neighborNode.costFromStart = currentNode.costFromStart + distance.magnitude;
                distance = endNode.pos - neighborNode.pos;
                neighborNode.costToGoal = distance.magnitude;
                neighborNode.totalCost = neighborNode.costFromStart + neighborNode.costToGoal;
                neighborNode.parent = currentNode;
                openList.Add(neighborNode);
            }
            currentNode.nodeState = PathNodeState.NodeClosed;
        }

        return false;
    }

    private void BuildPath(Vector3 startPos, Vector3 endPos, PathNode startNode, PathNode endNode)
    {
        pathList.Clear();
        pathList.Add(startPos);
        if (startNode == endNode)
        {
            pathList.Add(startNode.pos);
        }
        else
        {
            PathNode pathNode = endNode;
            while (pathNode != null)
            {
                pathList.Insert(1, pathNode.pos);
                pathNode = pathNode.parent;
            }
        }
        pathList.Add(endPos);
    }
}
