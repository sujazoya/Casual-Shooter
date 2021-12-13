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
    UIManager uIManager;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        UpdateHealth();
        uIManager = FindObjectOfType<UIManager>();
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
        healthImage.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1f);
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
        else
        {
            Die();
        }
    }
    void Die()
    {
        uIManager.OnGameover();
    }
}
