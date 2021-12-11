using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints_Holder : MonoBehaviour
{
    public Transform[] wayPoints;    
    // Start is called before the first frame update
    void Start()
    {
        wayPoints = GetComponentsInChildren<Transform>();
        
    }

}
