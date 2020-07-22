﻿using System;
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
        anim.SetInteger("state", (int)state); // Set animation based on Enumerator state
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Collectable")
        {
            cherry.Play();
            Destroy(collision.gameObject);
            cherries += 1;
            cherryText.text = cherries.ToString();
        }

        if(collision.tag == "PowerUp")
        {
            diamond.Play();
            Destroy(collision.gameObject);
            speed = 15f;
            GetComponent<SpriteRenderer>().color = Color.red;
            StartCoroutine(RestPower());
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "EnemyNew")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (state == State.falling)
            {
                enemy.JumpedOn();
                Jump();
            }
            else
            {
                state = State.hurt;
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

        if (other.gameObject.tag == "enemy")
        {
            sceneLoader.LoadScene(0);
        }
        if (other.gameObject.tag == "Friend")
        {

        }
    }

    public void InputManager()
    {
        float hDirection = Input.GetAxis("Horizontal");


        //Right movement
        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            //Sprite flipping
            transform.localScale = new Vector2(-1, 1);
        }

        //Left movement
        else if (hDirection > 0)
        {
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
            if (rb.velocity.y < 1f)
            {
                state = State.falling;
            }
        }

        else if(state != State.jumping && !IsGrounded())
        {
            state = State.falling;
        }

        else if(state == State.falling)
        {
            if (IsGrounded())
            {
                state = State.idle;
            }
        }

        else if(state == State.hurt)
        {
            if(Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }

        else if (Mathf.Abs(rb.velocity.x) > 2f && IsGrounded())
        {
            state = State.running;
        }
        else
        {
            state = State.idle;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        state = State.jumping;
    }

    private void Footstep()
    {
        footstep.Play();
    }

    private IEnumerator RestPower()
    {
        yield return new WaitForSeconds(10);
        speed = 10f;
        GetComponent<SpriteRenderer>().color = Color.white;
    }


    //Enemy test
   // public void OnCollisionEnter2D(Collision2D collision)
    //{
       // if (collision.gameObject.tag == "enemy")
       // {
       //     sceneLoader.LoadScene(0);
       // }
       // if (collision.gameObject.tag == "Friend")
       // {

       // }
   // }
}
