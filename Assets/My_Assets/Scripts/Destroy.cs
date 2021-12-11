using UnityEngine;

public class Destroy : MonoBehaviour
{
    public float destroyTime = 2;
    public float destroyTimeRandomize = 0;

    private float countToTime;
    [SerializeField] bool destroy;
    [SerializeField] bool disable;
    private void Awake()
    {
        if (destroy)
        {
            destroyTime += Random.value * destroyTimeRandomize;
        }
       
    }

    private void Update()
    {
        if (destroy)
        {
            countToTime += Time.deltaTime;
            if (countToTime >= destroyTime)
                Destroy(gameObject);
        }
           
    }
    private void OnEnable()
    {
        if (disable)
        {
            Invoke("DeactivateSele", destroyTime);
        }
    }
    void DeactivateSele()
    {
        gameObject.SetActive(false);
    }
}