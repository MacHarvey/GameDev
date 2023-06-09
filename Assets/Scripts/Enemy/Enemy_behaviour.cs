using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_behaviour : MonoBehaviour
{
    #region Public Variables 
    public Transform rayCast;
    public LayerMask raycastMask;
    public float rayCastLength;
    public float attackDistance; // Min distance for attack 
    public float moveSpeed;
    public float timer; // Timer for cooldown between attacks 
    public Transform leftLimit;
    public Transform rightLimit; 
    // Added this 
    public float damage;

    


    #endregion

    #region Private Variables 
    private RaycastHit2D hit;
    private Transform target;
    private Animator anim;
    private float distance; // Distance between enemy and player 
    private bool attackMode;
    private bool inRange; // player in range
    private bool cooling; // enemy cool after attack 
    private float intTimer;

    // Added this 

    private Health playerHealth;
    #endregion




    void Awake()
    {
        SelectTarget();
        intTimer = timer;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!attackMode)
        {
            Move();
        }

        if(!InsideofLimits() && !inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_attack"))
        {
            SelectTarget();
        }

        if (inRange) 
        {
      
            hit = Physics2D.Raycast(rayCast.position, transform.right, rayCastLength, raycastMask);
            RaycastDebugger();
            anim.SetBool("Attack", false);
        }

        // When Detected 
        if(hit.collider != null)
        {
            EnemyLogic();
            playerHealth = hit.transform.GetComponent<Health>();
        }
        else if (hit.collider == null)
        {
            inRange = false;
        }

        if (inRange == false) 
        {
            //anim.SetBool("canMove", false);
            StopAttack();
        }

    }

    void OnTriggerEnter2D(Collider2D trig)
    {
        if(trig.gameObject.tag == "Player")
        {
            target = trig.transform;
            inRange = true;
            Flip();
                
        }
    }

    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);
        if(distance > attackDistance)
        {
            
            StopAttack();
        }
        else if(attackDistance >= distance && cooling == false)
        {
            Attack(); 
        }
        if (cooling)
        {
            Cooldown();
            anim.SetBool("Attack", false); 
        }
    }

    void Move()
    {
        anim.SetBool("canMove", true);
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_attack"))
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    void Attack()
    {
        timer = intTimer;
        attackMode = true;

        anim.SetBool("canMove", false);
        anim.SetBool("Attack", true);
        DamagePlayer();
    }

    void DamagePlayer()
    {
       
        playerHealth.TakeDamage(damage);

    }

    void Cooldown()
    {
        timer -= Time.deltaTime;

        if(timer <= 0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }

    void StopAttack()
    {
        cooling = false;
        attackMode = false; 

    }

    void RaycastDebugger()
    {
        if(distance > attackDistance)
        {
            Debug.DrawRay(rayCast.position, transform.right * rayCastLength, Color.red);
        }
        else if(attackDistance > distance)
        {
            Debug.DrawRay(rayCast.position, transform.right * rayCastLength, Color.green);
        }
    }

    public void TriggerCooling()
    {
        cooling = true;
    }

    private bool InsideofLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    private void SelectTarget()
    {
        float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);

        if(distanceToLeft > distanceToRight) 
        {
            target = leftLimit;
        }
        else 
        {
            target = rightLimit;
        }

        Flip();
    }

    private void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        if(transform.position.x > target.position.x )
        {
            rotation.y = 180f;
        }
        else
        {
            rotation.y = 0f;
        }

        transform.eulerAngles = rotation;
    }

   
}
