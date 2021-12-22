using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Trigger : MonoBehaviour
{

    //BoxCollider boxCollider;
    BuildingManager buildingManager;
    // Start is called before the first frame update
    void OnEnable()
    {
        //boxCollider=GetComponent<BoxCollider>();
        buildingManager = FindObjectOfType<BuildingManager>();
        if (buildingManager.currentBuildings.Count > 0)
        {
            DestroyBuildingsNearPlayer();
        }
    }
    [HideInInspector] public List<GameObject> buildingsNearPlayer;
    float BuildingDistance(Transform target)
    {
        float distance = Vector3.Distance(transform.position, target.position);       
        return distance;
    }
    void DestroyBuildingsNearPlayer()
    {

        for (int i = 0; i <buildingManager. currentBuildings.Count; i++)
        {

            if (BuildingDistance(buildingManager.currentBuildings[i].transform) < 30)
            {
                buildingsNearPlayer.Add(buildingManager.currentBuildings[i]);
            }
        }
        if (buildingsNearPlayer.Count > 0)
        {
            for (int i = 0; i < buildingsNearPlayer.Count; i++)
            {
                buildingsNearPlayer[i].GetComponent<Building>().SetActive(false);
            }
        }

       
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        //other.transform.GetComponent<Rigidbody>().isKinematic = false;
    //        //other.transform.position = transform.position;
    //        //Vector3 vec = other.transform.rotation.eulerAngles;
    //        //Quaternion rotation = Quaternion.LookRotation(vec, Vector3.up);           
    //        //other.transform.rotation = rotation;
    //        //other.transform.rotation =Quaternion.Euler(Vector3.up);
    //        boxCollider.enabled = false;

    //    }
    //}
}
