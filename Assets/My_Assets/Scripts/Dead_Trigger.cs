using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead_Trigger : MonoBehaviour
{   
    void DeadPlayer()
    {
        FindObjectOfType<UIManager>().OnGameover();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Game.playerTag)
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(20,0);
            Invoke("DeadPlayer", 1);            
        }
    }
}
