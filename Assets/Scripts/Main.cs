using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public GameObject control;
    private NPC npc;
    private bool aStarSuccess = false;

    // Start is called before the first frame update
    void Start()
    {
        WayPointTerrain wpt = new WayPointTerrain();
        wpt.Init();

        AStar aStar = new AStar();
        aStar.Init(wpt);
        npc = control.GetComponent<NPC>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if(Physics.Raycast(ray, out raycastHit, 1000.0f, 1 << LayerMask.NameToLayer("Terrain")))
            {
                aStarSuccess = AStar.instance.PerformAStar(control.transform.position, raycastHit.point);
                npc.currentPathPoint = 0;
            }
        }
        if (aStarSuccess)
        {
            List<Vector3> path = AStar.instance.GetPath();
            int finalIndex = path.Count - 1;
            int i;
            for (i = finalIndex; i >= npc.currentPathPoint; i--)
            {
                Vector3 startPos = path[i];
                Vector3 controlPos = control.transform.position;
                if (Physics.Linecast(controlPos, startPos, 1 << LayerMask.NameToLayer("Wall")))
                {
                    continue;
                }
                npc.currentPathPoint = i;
                npc.SetTarget(startPos);
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (aStarSuccess)
        {
            List<Vector3> path = AStar.instance.GetPath();
            Gizmos.color = Color.blue;
            int finalCount = path.Count - 1;
            int i;
            for(i=0; i < finalCount; i++)
            {
                Vector3 startPos = path[i];
                startPos.y += 1.0f;
                Vector3 endPos = path[i + 1];
                endPos.y += 1.0f;
                Gizmos.DrawLine(startPos, endPos);
            }
        }
    }
}
