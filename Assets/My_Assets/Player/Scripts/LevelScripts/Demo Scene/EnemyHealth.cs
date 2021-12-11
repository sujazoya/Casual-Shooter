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
	private float health, totalHealth = 100;	
	private bool dead;
	public Image healthImage;
	Animator animator;
	void Awake ()
	{
		health = totalHealth;
		healthImage.fillAmount = 1 / health * totalHealth;
	}
    private void Start()
    {
		animator = GetComponent<Animator>();
    }
    public bool IsDead { get { return dead; } }

	public override void TakeDamage(Vector3 location, Vector3 direction, float damage)
	{		
		health -= damage;
		UpdateHealthBar();
		if (health <= 0 )
		{
			Kill();
		}
        if (hitSound) { AudioSource.PlayClipAtPoint(deadSound, transform.position); }
		
	}
	void Kill()
	{
		dead = true;
		if (transform.GetComponent<Collider>()) { transform.GetComponent<Collider>().isTrigger = true; }
		if (animator) { animator.SetBool("Walk", false); }
		if (animator) { animator.SetBool("die",true); }
		if (deadSound)
		{ AudioSource.PlayClipAtPoint(deadSound, transform.position); }
		Destroy(gameObject, 3);		
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
		healthImage.fillAmount = 1 / health * totalHealth;
	}
}
