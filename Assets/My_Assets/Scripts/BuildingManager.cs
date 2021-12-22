using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    int needBuildingCount = 400;
    [SerializeField] GameObject[] buildings;
    public List<GameObject> currentBuildings;
    int index;
    bool buildingsCreated;
    // Start is called before the first frame update
    void Start()
    {
        CreatBuildings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator CheckBuilding()
    {       
        if (buildingsCreated)
        {            
            for (int i = 0; i < currentBuildings.Count; i++)
            {
                BuildingDistance(currentBuildings[i].transform);
            }
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(CheckBuilding());
    }
    float BuildingDistance(Transform target)
    {
        float distance = Vector3.Distance(transform.position, target.position);
        //Vector3 pos = new Vector3(transform.position.x + Random.insideUnitSphere.x * 150,
        //                    transform.position.y,
        //                    transform.position.z + Random.insideUnitSphere.z * 150);
        if (Vector3.Distance(transform.position,target.position)<300)
        {
            target.gameObject.GetComponent<Building>().SetActive(true);
        }
        else
        {
            target.gameObject.GetComponent<Building>().SetActive(false);
        }
        return distance;
    }
    void CreatBuildings()
    {
        GameObject building = new GameObject();
        building.transform.name = "buildings";
        for (int i = 0; i < needBuildingCount; i++)
        {
            GameObject cb = Instantiate(buildings[currentIndex()]);
            cb.transform.position = PosForNewBuilding();
            cb.transform.parent = building.transform;
            currentBuildings.Add(cb);
        }
        DestroyBuildingsNearPlayer();
        buildingsCreated = true;
        //for (int i = 0; i < currentBuildings.Count; i++)
        //{
        //    currentBuildings[i].SetActive(false);
        //}       
        StartCoroutine(CheckBuilding());
    }
    [HideInInspector] public List<GameObject> buildingsNearPlayer;
    void DestroyBuildingsNearPlayer()
    {

        for (int i = 0; i < currentBuildings.Count; i++)
        {  
           
            if(BuildingDistance(currentBuildings[i].transform) < 50)
            {
                buildingsNearPlayer.Add(currentBuildings[i]);
            }
        }
        if (buildingsNearPlayer.Count > 0)
        {
            for (int i = 0; i < buildingsNearPlayer.Count; i++)
            {
                buildingsNearPlayer[i].GetComponent<Building>().SetActive(false);
            }
        }

        //hitColliders = Physics.OverlapSphere(transform.position, 50f);
        //foreach (var hitCollider in hitColliders)
        //{
        //    if (hitCollider.gameObject.tag == "building")
        //    {
        //        Destroy(hitCollider.transform.parent.gameObject);
        //    }
        //}
    }
    int currentIndex()
    {
        index++;
        if (index >= buildings.Length)
        {
            index = 0;
        }
        return index;
    }
    Vector3 pos;
    float minPose = -750;
    float mAXPose =  750;
    Vector3 PosForNewBuilding()
    {
        pos.x = Random.Range(minPose, mAXPose);
        pos.y = Random.Range(10, -10);
        pos.z = Random.Range(minPose, mAXPose);
        return pos;
    }
    public Collider[] hitColliders;


}
