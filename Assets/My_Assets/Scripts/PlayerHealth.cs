using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] GameObject hit;
    [SerializeField] Text healthText;
    [SerializeField] Image healthImage;
    public float health;
    float maxHealth=100;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        UpdateHealth();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(float damage,float wait)
    {
        StartCoroutine(MakeDamages(damage, wait));
    }
    void UpdateHealth()
    {
        healthText.text = health.ToString();
        healthImage.fillAmount = 1 / 100 * health;
    }
    IEnumerator MakeDamages(float damage, float wait)
    {
        yield return new WaitForSeconds(wait);
        if (health > 0)
        {
            hit.SetActive(true);
            health -= damage;
            UpdateHealth();



            yield return new WaitForSeconds(0.4f);
            hit.SetActive(false);
        }
    }
}
