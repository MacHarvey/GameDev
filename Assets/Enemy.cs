using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Animator animator;


    public int maxHealth = 200;
    int currentHealth;

    [SerializeField] private AudioClip deathSound;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void takeDamage(int damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("Hurt");
        
        if(currentHealth <= 0 ) 
        {
            Die();
        }
    }
 
    void Die()
    {
        SoundManager.instance.PlaySound(deathSound);
        Debug.Log("Enemy Died!");

        animator.SetBool("IsDead", true);

        //GetComponent<Collider2D>().enabled = false;
        Physics2D.IgnoreLayerCollision(0,10);
        GetComponent<Enemy_behaviour>().enabled = false;
        waitAndDestroy();
        //GetComponent<SpriteRenderer>().enabled = false;
        this.enabled = false;

        
    }

    void waitAndDestroy()
    {
        Object.Destroy(gameObject, 1.0f);
    }
}
