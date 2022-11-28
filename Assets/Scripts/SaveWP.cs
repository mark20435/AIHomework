using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveWP : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("WP");

        StreamWriter sw = new StreamWriter("Assets/Resources/WPData.txt", false);

        string s = "";
        for(int i = 0; i< gos.Length; i++)
        {
            s = "";
            s += gos[i].name;
            s += " ";
            WP wp = gos[i].GetComponent<WP>();
            s += wp.floor;
            s += " ";
            s += wp.link;
            s += " ";
            s += wp.neighbors.Count;
            s += " ";
            for(int j = 0; j < wp.neighbors.Count; j++)
            {
                s += wp.neighbors[j].name;
                s += " ";
            }
            sw.WriteLine(s);
        }
        sw.Close();
    }
}
