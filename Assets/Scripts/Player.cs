using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Scripting.APIUpdating;
using UnityEditor;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    Helper helper;
    public HealthBar healthBar;
    public GameObject spawnPoint;
    public GameObject gameOverText;
    public GameObject enemy;
    private float speed = 0;
    private float jumpHeight = 0;
    private float bounceMultiplyer = 2;
    private int knockback = 6;
    private bool facingRight = false;
    private bool groundRay;
    public int maxHealth = 100;
    private float timer = 0;
    private float hitTimer = 0;
    public int currentHealth;
    private bool start = true;
    private bool spawnTimer = false;
    private bool hit = false;
    private int enemyDamage = 5;
    private bool pushBack = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        helper = gameObject.AddComponent<Helper>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        gameOverText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (start == true) // get up from downstate
        {
            anim.SetBool("Start", true);
            if (Input.anyKey == true)
            {
                anim.SetBool("Start", false);
                anim.SetBool("Revive", true);
                start = false;
                jumpHeight = 6;
                speed = 3;
            }
        }
        else // everything else
        {
            Move();
            Jump();
            Animations();
            groundRay = helper.RayGroundCheck(0, 0);
            Hit();
            Death();
        }        
    }
    void Move()
    {
        if (Input.GetKey("d") == true)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            if (facingRight == false && rb.velocity.x > 0)
            {
                Flip();
            }
        }
        else if (Input.GetKey("a") == true)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            if (facingRight == true && rb.velocity.x < 0)
            {
                Flip();
            }
        }
        else
        {
            if (rb.velocity.y != 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
    }
    void Jump()
    {
        if (Input.GetKeyDown("space") == true && groundRay == true)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpHeight, 0);
        }
    }
    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x = -currentScale.x;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }
    void Animations()
    {
        if ((rb.velocity.x > 0 || rb.velocity.x < 0) && groundRay == true)
        {
            anim.SetBool("Run", true);
            anim.SetBool("Jump", false);
        }
        else if (groundRay == false)
        {
            anim.SetBool("Run", false);
            anim.SetBool("Jump", true);
        }
        else if (Input.GetKey("f") == true && groundRay == true)
        {
            anim.SetBool("Attack", true);
            anim.SetBool("Jump", false);
            anim.SetBool("Run", false);
            anim.SetBool("Hit", false);
        }
        else
        {
            anim.SetBool("Run", false);
            anim.SetBool("Jump", false);
            anim.SetBool("Attack", false);
            anim.SetBool("Revive", false);
            anim.SetBool("Death", false);
        }
    }


    void Death()
    {
        if (currentHealth <= 0)
        {
            healthBar.SetHealth(currentHealth);
            anim.SetBool("Run", false);
            anim.SetBool("Jump", false);
            anim.SetBool("Attack", false);
            anim.SetBool("Revive", false);
            anim.SetBool("Death", true);
            speed = 0;
            gameOverText.gameObject.SetActive(true);
            jumpHeight = 0;
            Wait(3);
            if (spawnTimer == true)
            {
                Debug.Log("hello");
                transform.position = spawnPoint.transform.position;
                gameOverText.gameObject.SetActive(false);
                currentHealth = maxHealth;
                healthBar.SetHealth(currentHealth);
                anim.SetBool("Death", false);
                spawnTimer = false;
                start = true;
            }
        }
    }
    void Wait(float seconds)
    {

        timer += Time.deltaTime;

        if (timer >= seconds)
        {
            spawnTimer = true;
            timer = 0;
        }
    }
    void Hit()
    {
        if (hit == true)
        {
            hitTimer += Time.deltaTime;
            anim.SetBool("Hit", true);
            if (hitTimer >= 0.7f)
            {
                currentHealth -= enemyDamage;
                healthBar.SetHealth(currentHealth);
                hitTimer = 0;
                anim.SetBool("Hit", false);
            }
        }
        if (pushBack == true)
        {
            if (enemy.transform.position.x > transform.position.x)
            {
                rb.velocity = new Vector2(-knockback, 0);
            }
            if (enemy.transform.position.x < transform.position.x)
            {
                rb.velocity = new Vector2(knockback, 0);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Spikes"))
        {
            currentHealth = 0;
        }
        else if (other.gameObject.CompareTag("Bounce"))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpHeight * bounceMultiplyer, 0);
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            pushBack = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            pushBack = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            hit = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            hit = false;
            anim.SetBool("Hit", false);
        }
    }


}
