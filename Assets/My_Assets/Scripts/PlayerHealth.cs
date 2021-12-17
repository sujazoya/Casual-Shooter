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
    [SerializeField ] UIManager uIManager;
    Animator animator;
    BasicBehaviour basicBehaviour;
    [SerializeField] Collider[] bodyColliders;
    private void Awake()
    {
        basicBehaviour = GetComponent<BasicBehaviour>();
        animator = GetComponent<Animator>();
        ResetPlayer();
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }    
    public void TakeDamage(float damage,float wait)
    {              
        StartCoroutine(MakeDamages(damage, wait));
        if (basicBehaviour.IsGrounded())
        {
            animator.SetTrigger("hit");
        }
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
        else if (health <= 0)
        {
            
            //Die();
        }
    }
    public void ResetPlayer()
    {
        bodyColliders[0].enabled = false;
        bodyColliders[1].enabled = true;
        health = maxHealth;
        UpdateHealth();
        Invoke("LateReplay", 2);

    }
    void LateReplay()
    {
        Game.playerIdDead = false;
        Game.gameStatus = Game.GameStatus.isPlaying;
    }
    void Die()
    {
        uIManager.ShowGameOver_Warn();
        gameObject.SetActive(false);
        //uIManager.OnGameover();
    }
    private void LateUpdate()
    {
          if (health <= 0&& !Game.playerIdDead)
        {
            Game.gameStatus = Game.GameStatus.isGameover;
            Game.playerIdDead = true;
            bodyColliders[0].enabled = true;
            bodyColliders[1].enabled = false;
            animator.SetTrigger("die");
            Invoke("Die",3);          
            //Die();
        }
    }
}
