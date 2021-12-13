using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
// This class is created for the example scene. There is no support for this script.
public class EnemyHealth : HealthManager
{
	
	public AudioClip deadSound;
	public AudioClip hitSound;
	private Vector3 targetRotation;
	[HideInInspector]public float health, totalHealth = 100;	
	private bool dead;
	public Image healthImage;
	Animator animator;
	[SerializeField] GameObject weapon;
	[SerializeField] PlayerIKController playerIK;
	public GameObject[] items;
	GameController_Grappling gameController;
	void Awake ()
	{
		health = totalHealth;
		UpdateHealthBar();
	}
    private void Start()
    {
		animator = GetComponent<Animator>();
		gameController = FindObjectOfType<GameController_Grappling>();
	}
    public bool IsDead { get { return dead; } }

	public override void TakeDamage(Vector3 location, Vector3 direction, float damage)
	{
        if (health >= damage)
        {   }
        else
        {
			damage = health;
		}
		health -= damage;		
		gameController.ShowPowerup("- " + damage);
		UpdateHealthBar();
		if (health <= 0 )
		{
			Kill();
		}
        if (hitSound) { AudioSource.PlayClipAtPoint(deadSound, transform.position); }
		
	}
	void Kill()
	{
		if (!dead)
		{
			if (weapon) { weapon.SetActive(false); }
			if (playerIK) { playerIK.ikActive = false; }
			dead = true;
			if (transform.GetComponent<Collider>()) { transform.GetComponent<Collider>().isTrigger = true; }
			if (animator) { animator.SetBool("Walk", false); }
			if (animator) { animator.SetTrigger("die"); }
			if (deadSound)
			{ AudioSource.PlayClipAtPoint(deadSound, transform.position); }
			Game.currentScore++;
			ReleaseItem();
			Destroy(gameObject, 3);
			gameController.UpdateUI();
			gameController.ShowKill();			
		}
	}
	public void ReleaseItem()
    {
		int index = Random.Range(0, items.Length);
        if (items.Length > 0)
        {
			Vector3 po = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
			Instantiate(items[index], po, Quaternion.identity);
        }
    }
	public void Revive()
	{
		
		health = totalHealth;			
		UpdateHealthBar();
		
		dead = false;
		targetRotation.x = 0;
		AudioSource.PlayClipAtPoint(deadSound, transform.position);
	}

	private void UpdateHealthBar()
	{
		healthImage.fillAmount = Mathf.Clamp(health / totalHealth, 0, 1f);
	}
}
