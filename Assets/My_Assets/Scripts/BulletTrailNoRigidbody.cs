using UnityEngine;

public class BulletTrailNoRigidbody : MonoBehaviour
{
    public float speed = 200;
    int damageAmmount = 10;
    private void Update()
    {
        transform.position = transform.position + transform.forward * Time.deltaTime * speed;
    }
//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag ("Player"))
//        {
//            if (other.gameObject.GetComponent<PlayerHealth>())
//            {
//                other.gameObject.GetComponent<PlayerHealth>().TakeDamage(damageAmmount,0);
//}
//        }
//    }
}