using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics;

public class PlayerControl : MonoBehaviour
{
    //Start varibels
    private Rigidbody2D rb;
    private Animator anim;
    public LayerMask groundLayer;
    private enum State { idle, running, jumping, falling, hurt };
    private State state = State.idle;


    //Hero params
    [SerializeField] private float speed = 10f;
    [SerializeField] private float JumpForce = 10f;
    [SerializeField] private int cherries = 0;
    [SerializeField] private TextMeshProUGUI cherryText;
    [SerializeField] private float hurtforce = 10f;
    [SerializeField] private AudioSource cherry;
    [SerializeField] private AudioSource diamond;
    [SerializeField] private AudioSource footstep;

    //загрузчик сцены
    public SceneLoader sceneLoader;

    private void Start()
    {
        //get player's Rigidbody and animator objects
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        //некая ссылка на SceneLoader
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    private void Update()
    {
        if(state != State.hurt)
        {
            InputManager();
        }


        VeolcityState();
        //Convert Player's state to integer
        anim.SetInteger("state", (int)state); // Set animation based on Enumerator state
    }

    //This function works, when player touch another object
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If player touched object with tag "Collectable" (Cherry)
        if(collision.tag == "Collectable")
        {
            cherry.Play(); //Play cherry's sound (was declarated higher)
            Destroy(collision.gameObject); // Destroy this Cherry
            cherries += 1; //Chery +1
            cherryText.text = cherries.ToString(); //Convert cherry's value to sting because we cant show integer in text object
        }

        //If player touched object with tag "PowerUp" (Diamond)
        if (collision.tag == "PowerUp")
        {
            diamond.Play(); //Play diamond's sound (was declarated higher)
            Destroy(collision.gameObject); // Destroy this Diamond
            speed = 15f; //Speed up
            GetComponent<SpriteRenderer>().color = Color.red; //Red color for Hero
            StartCoroutine(RestPower()); //Function-counder of diamond's effect
        }
    }

    //Works when player touch the enemy
    private void OnCollisionEnter2D(Collision2D other)
    {
        //If Player touched the enemy
        if(other.gameObject.tag == "EnemyNew")
        {
            //Declarate enemy object
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            //If Player have state Falling
            if (state == State.falling)
            {
                enemy.JumpedOn(); //Enemy's function (Death + Sound + Animation)
                Jump(); //Pleyer jump again when he kills enemy
            }
            //If he dosen't have Falling state
            else
            {
                state = State.hurt; //Set Hurt state
                //Players direction after hurt
                if(other.gameObject.transform.position.x > transform.position.x)
                {
                    //Enemy is to my right, I should be damaged and moved left
                    rb.velocity = new Vector2(-hurtforce, rb.velocity.y);
                }
                else
                {
                    //Enemy is to my left, I should be damaged and moved right
                    rb.velocity = new Vector2(hurtforce, rb.velocity.y);
                }
            }
        }
        //I don't know who did it
        if (other.gameObject.tag == "Friend")
        {

        }
    }

    //Movement Control
    public void InputManager()
    {
        //take input button
        float hDirection = Input.GetAxis("Horizontal");


        //Right movement
        if (hDirection < 0)
        {
            //Move right
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            //Sprite flipping
            transform.localScale = new Vector2(-1, 1);
        }

        //Left movement
        else if (hDirection > 0)
        {
            //Move left
            rb.velocity = new Vector2(speed, rb.velocity.y);
            //Sprite flipping
            transform.localScale = new Vector2(1, 1);
            
        }

        //Jumping
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
        }

    }

    //Checking if Hero is grounded (true or false)
    public bool IsGrounded()
    {
        //Это маленький костыль
        Vector2 brokenVector = new Vector2(0.0F, 0.2F);
        Vector2 position = transform.position;
        position = position - brokenVector;
        Vector2 direction = Vector2.down;
        float distance = 1.0f;


        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

    //Checking state changes
    private void VeolcityState()
    {
        if (state == State.jumping)
        {
            //If object falling, set falling state
            if (rb.velocity.y < 1f)
            {
                state = State.falling;
            }
        }

        //If object falling, set falling state
        else if (state != State.jumping && !IsGrounded())
        {
            state = State.falling;
        }

        //Wait until he's grounded
        else if(state == State.falling)
        {
            if (IsGrounded())
            {
                state = State.idle;
            }
        }

        //check if Hero got Hurt
        else if(state == State.hurt)
        {
            //When he stops moving after Hurt, he set idle
            if(Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }

        //Set running state
        else if (Mathf.Abs(rb.velocity.x) > 2f && IsGrounded())
        {
            state = State.running;
        }
        else
        {
            state = State.idle;
        }
    }

    //Simple jump function
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        state = State.jumping;
    }

    //Footstep sound
    //WE PLAY THIS FUNCTION IN RUNNING ANIMATION!!! (OPEN ANIMATION WINDOW AND YOU'LL SEE THIS FUNCTION THERE)
    private void Footstep()
    {
        footstep.Play();
    }

    //Dimond effect timer
    private IEnumerator RestPower()
    {
        yield return new WaitForSeconds(10);
        //After time return standert player's settings
        speed = 10f;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

}
