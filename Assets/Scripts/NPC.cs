using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    AIData data;
    public int currentPathPoint;
    // Start is called before the first frame update
    void Start()
    {
        data = new AIData();
        data.speed = 0.0f;
        data.arriveRange = 10.0f;
        data.maxSpeed = 5.0f;
        data.maxRotate = 5.0f;
        data.go = this.gameObject;
        data.target = Vector3.zero;
        currentPathPoint = -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTarget(Vector3 vec)
    {
        data.target = vec;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 2.0f);

        if(data != null)
        {
            Gizmos.DrawWireSphere(data.target, 1.0f);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, data.arriveRange);
        }
    }
}
