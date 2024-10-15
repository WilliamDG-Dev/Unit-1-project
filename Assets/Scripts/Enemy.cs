using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Helper helper;
    public HealthBar healthBar;
    private Animator animator;
    private float enemySpeed = 2;
    float offset = 0.5f;
    bool playerInAttackRange = false;
    bool playerAttacking = false;
    float dir;
    public int enemyMaxHealth = 60;
    public int currentEnemyHealth = 0;
    private int playerDamage = 10;
    private float timer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        helper = gameObject.AddComponent<Helper>();
        sr.flipX = true;
        dir = enemySpeed;
        currentEnemyHealth = enemyMaxHealth;
        healthBar.SetMaxHealth(enemyMaxHealth);

    }
    void Update()
    {
        EnemyPatrol();
        EnemyAnimations();
        PlayerDealingDamage();
        Death();
    }

    void EnemyPatrol()
    {
        if (playerInAttackRange == false)
        {
            rb.velocity = new Vector2(dir, 0);

            if (helper.RayGroundCheck(-offset, 0) == false && dir < 0)
            {
                sr.flipX = true;
                dir = enemySpeed;
            }
            if (helper.RayGroundCheck(offset, 0) == false && dir > 0)
            {
                sr.flipX = false;
                dir = -enemySpeed;
            }
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
    }
    void EnemyAnimations()
    {
        if (rb.velocity.y > 0 || rb.velocity.y < 0)
        {
            animator.SetBool("Jump", true);
        }
        if (rb.velocity.y == 0)
        {
            animator.SetBool("Jump", false);
        }

        if (playerInAttackRange == true)
        {
            animator.SetBool("Jump", false);
            animator.SetBool("Attack", true);
        }
        else
        {
            animator.SetBool("Attack", false);
        }

    }
    void PlayerDealingDamage()
    {
        if (playerAttacking == true && Input.GetKey("f") == true)
        {
            timer += Time.deltaTime;

            if (timer >= 0.7f)
            {
                currentEnemyHealth -= playerDamage;
                healthBar.SetHealth(currentEnemyHealth);
                animator.SetBool("Hit", true);
                animator.SetBool("Jump", false);
                animator.SetBool("Attack", false);
                timer = 0;
            }
        }
        else if (playerAttacking == true)
        {
            animator.SetBool("Hit", false);
        }
        else
        {
            animator.SetBool("Hit", false);
            playerAttacking = false;
        }
    }
    void Death()
    {
        if (currentEnemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerAttacking = true;
            playerInAttackRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInAttackRange = false;
            playerAttacking = false;
        }
    }
}
