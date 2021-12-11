using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Jump : MonoBehaviour
{
    public Transform cameraPos;
    bool timeToGo;

    // Start is called before the first frame update
    void Start()
    {
        //offset = new Vector3(transform.position.x - ball.position.x,
        //   transform.position.y - ball.position.y, transform.position.z - ball.position.z);
    }
    public void ResetPose()
    {
        //pos = transform.position - offset;
        timeToGo = true;
        Invoke("OffGo", 2);
        
    }

    void OffGo()
    {
        timeToGo = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (timeToGo )
        {
            if (transform.position != cameraPos.position)
            {
                transform.position = Vector3.Lerp(transform.position, cameraPos.position, 5f * Time.deltaTime);
            }           

        }
       
    }
}
