using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar_Collider : MonoBehaviour
{
    [HideInInspector] GameObject myPlayer;
    private Jumping_Ball jumping_Ball;
    Camera_Jump camera_Jump;
    // Start is called before the first frame update
    void Start()
    {
        jumping_Ball = FindObjectOfType<Jumping_Ball>();
        camera_Jump = FindObjectOfType<Camera_Jump>();
    }
    void ReleasePlayer()
    {
        myPlayer.transform.parent = null;
        myPlayer.transform.GetComponent<Rigidbody>().isKinematic = false;
        camera_Jump.ResetPose();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.transform.tag=="Player")
        {
            //collision.transform.parent = transform;
            collision.transform.GetComponent<Rigidbody>().isKinematic = true;
            collision.transform.position=new Vector3( transform.position.x, transform.position.y+0.6f, transform.position.z);
            myPlayer = collision.gameObject;
            Invoke("ReleasePlayer", 1.5f);
            jumping_Ball.NextTarget(transform.position);
        }
    }   
}
