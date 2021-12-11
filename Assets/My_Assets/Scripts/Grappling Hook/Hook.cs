using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] float hookForce = 25f;

    Grapple grapple;
    //Rigidbody rigid;
    LineRenderer lineRenderer;
    float stopDistance = 3;
    bool go;
    Vector3 targetPos;
    [SerializeField] GameObject hookObject;
  
    public void Initialize(Grapple grapple, Transform shootTransform)
    {
                   //transform.forward = shootTransform.forward;
            this.grapple = grapple;
            //rigid = GetComponent<Rigidbody>();
            lineRenderer = GetComponent<LineRenderer>();
            //rigid.AddForce(transform.forward * hookForce, ForceMode.Impulse);
            go = true;
            targetPos = shootTransform.position;
       
       
    }
    
    // Update is called once per frame
    void Update()
    {
                //transform.forward = shootTransform.forward;
       
        Vector3[] positions = new Vector3[]
        {
            targetPos,
            grapple.shootTransform.position
        };

        lineRenderer.SetPositions(positions);
        hookObject.transform.LookAt(targetPos);
        if (Vector3.Distance(transform.position, targetPos) <= 0)
        {          
           
            
        }
        else
        {
            if (go)
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, hookForce * Time.deltaTime);
            }           
            if (transform.position == targetPos)
            {
                go = false;
            }

        }

    }
    private Vector3 grapPoint;
    Vector3 GetTargetPos()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(camRay, out hit, 1000))
        {
            grapPoint = hit.point;
        }
        return grapPoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if((LayerMask.GetMask("Grapple") & 1 << other.gameObject.layer) > 0)
        {
            go = false;
            //rigid.useGravity = false;
            //rigid.isKinematic = true;
            transform.position = targetPos;            
            other.transform.GetComponent<Collider>().enabled = false;
            grapple.StartPull();
        }
    }
}
