using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WP : MonoBehaviour
{
    public List<GameObject> neighbors;
    public bool link = false;
    public int floor = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if(neighbors != null && neighbors.Count > 0)
        {
            foreach(GameObject go in neighbors)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(this.transform.position, go.transform.position);
            }
        }
    }
}
