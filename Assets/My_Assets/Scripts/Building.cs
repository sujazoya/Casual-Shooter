using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    GameObject body;
    private void Awake()
    {
        body = transform.Find("body").gameObject;
    }
     public void SetActive(bool value)
    {
        if (body)
        {
            body.SetActive(value);
        }
    }
}
