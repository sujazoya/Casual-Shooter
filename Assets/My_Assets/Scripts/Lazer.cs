using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    LineRenderer lineRenderer;
    [SerializeField]
    // Start is called before the first frame update
    void Start()
    {
     //   lineRenderer = GetComponent<LineRenderer>();
     //   Vector3[] positions = new Vector3[]
     //{
     //       MiddleScreen(),
     //       transform.parent.position
     //};

     //   lineRenderer.SetPositions(positions);
    }

    // Update is called once per frame
    void LateUpdate()
    {
      
    }
    Vector3 MiddleScreen()
    {
        var cam = Camera.main;
        var v = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, cam.nearClipPlane));
        transform.position = v;
        return v;
     
    }
}
