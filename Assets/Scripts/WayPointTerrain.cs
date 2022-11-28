using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WayPointTerrain
{
    public List<PathNode> nodeList;
    public GameObject[] walls;

    public void Init()
    {
        walls = GameObject.FindGameObjectsWithTag("Wall");
        nodeList = new List<PathNode>();
        GameObject[] gos = GameObject.FindGameObjectsWithTag("WP");
        foreach(GameObject go in gos)
        {
            PathNode p = new PathNode();
            p.link = false;
            p.floor = 0;
            p.totalCost = p.costFromStart = p.costToGoal = 0.0f;
            p.parent = null;
            p.neighbors = new List<PathNode>();
            p.pos = go.transform.position;
            p.go = go;
            nodeList.Add(p);
        }
        LoadWP();
    }

    public void LoadWP()
    {
        StreamReader sr = new StreamReader("Assets/Resources/WPData.txt");
        if (sr == null)
        {
            return;
        }
        sr.Close();

        TextAsset ta = Resources.Load("WPData") as TextAsset;
        string all = ta.text;
        string[] lines = all.Split("\n");
        int lineLength = lines.Length;
        int lineIndex = 0;

        while (lineIndex < lineLength)
        {
            string s = lines[lineIndex];
            lineIndex++;
            string[] ss = s.Split(' ');
            PathNode currentNode = null;
            for(int i = 0; i < nodeList.Count; i++)
            {
                if (nodeList[i].go.name.Equals(ss[0]))
                {
                    currentNode = nodeList[i];
                    break;
                }
            }
            if (currentNode == null)
            {
                continue;
            }
            currentNode.floor = int.Parse(ss[1]);
            if (ss[2].Equals("True"))
            {
                currentNode.link = true;
            }
            int neighborsCount = int.Parse(ss[3]);
            int index = 4;
            for(int i = 0; i< neighborsCount; i++)
            {
                string currentName = ss[index + i];
                for(int j = 0; j < nodeList.Count; j++)
                {
                    if (nodeList[j].go.name.Equals(currentName))
                    {
                        currentNode.neighbors.Add(nodeList[j]);
                        break;
                    }
                }
            }
        }
    }

    public void ClearAStarInfo()
    {
        foreach(PathNode node in nodeList)
        {
            node.parent = null;
            node.costFromStart = 0.0f;
            node.costToGoal = 0.0f;
            node.totalCost = 0.0f;
            node.nodeState = PathNodeState.NodeNone;
        }
    }

    public PathNode GetNodeFromPosition(Vector3 pos, int floor)
    {
        PathNode returnNode = null;
        PathNode currentNode;
        int count = nodeList.Count;
        float max = 10000.0f;
        for(int i = 0; i < count; i++)
        {
            currentNode = nodeList[i];
            if (currentNode.floor != floor)
            {
                continue;
            }
            if(Physics.Linecast(pos, currentNode.pos, 1 << LayerMask.NameToLayer("Wall")))
            {
                continue;
            }
            Vector3 vec = currentNode.pos - pos;
            vec.y = 0.0f; // 視個案而定
            float distance = vec.magnitude;
            if (distance < max)
            {
                max = distance;
                returnNode = currentNode;
            }
        }
        return returnNode;
    }
}
