using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIK : MonoBehaviour
{
    [SerializeField] Transform leftHandIK;
    [SerializeField] Transform rightHandIK;
    [SerializeField] PlayerIKController iKController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ActivateIK()
    {
        iKController = transform.root.GetComponent<PlayerIKController>();
        iKController.ikActive=true;
        if (iKController)
        {
            if (leftHandIK)
            {
                iKController.lefttHandObj = leftHandIK;
            }
            if (rightHandIK)
            {
                iKController.rightHandObj = rightHandIK;
            }
        }
    }
}
