using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCollider : MonoBehaviour
{
   [HideInInspector]public Collider[] hitColliders;
    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.tag == "building")
        //{
        //    hitColliders = Physics.OverlapSphere(transform.position, 30f);
        //    foreach (var hitCollider in hitColliders)
        //    {
        //        if (hitCollider.gameObject.tag == "building")
        //        {
        //            Destroy(hitCollider.transform.parent.gameObject);
        //        }
        //    }
        //}
    }
    private void OnEnable()
    {
        //hitColliders = Physics.OverlapSphere(transform.position, 3f);
        //foreach (var hitCollider in hitColliders)
        //{
        //    if (hitCollider.gameObject.tag == "building")
        //    {
        //        Destroy(hitCollider.transform.parent.gameObject);
        //    }
        //}
    }
}
