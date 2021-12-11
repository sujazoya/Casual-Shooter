using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Trigger : MonoBehaviour
{

    BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider=GetComponent<BoxCollider>();
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //other.transform.GetComponent<Rigidbody>().isKinematic = false;
            //other.transform.position = transform.position;
            //Vector3 vec = other.transform.rotation.eulerAngles;
            //Quaternion rotation = Quaternion.LookRotation(vec, Vector3.up);           
            //other.transform.rotation = rotation;
            //other.transform.rotation =Quaternion.Euler(Vector3.up);
            boxCollider.enabled = false;

        }
    }
}
