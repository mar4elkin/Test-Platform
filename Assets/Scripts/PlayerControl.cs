using System;
using System.Collections;
using System.Collections.Generic;

using System.Threading;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public LayerMask groundLayer;
    private enum State { idle, running, jumping, falling};
    private State state = State.idle;

    //загрузчик сцены
    public SceneLoader sceneLoader;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        //некая ссылка на SceneLoader
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    // Update is called once per frame
    private void Update()
    {


        float hDirection = Input.GetAxis("Horizontal");



        if (hDirection < 0)
        {
            //Go right
            rb.velocity = new Vector2(-5, rb.velocity.y);
            //Sprite flipping
            transform.localScale = new Vector2(-1, 1);
        }
        else if (hDirection > 0)
        {
            //Go left
            rb.velocity = new Vector2(5, rb.velocity.y);
            //Sprite flipping
            transform.localScale = new Vector2(1, 1);
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded() == true) 
            {
                //Jump

                rb.velocity = new Vector2(rb.velocity.x, 10f);
                state = State.jumping;
            }
        }
        VeolcityState();
        anim.SetInteger("state", (int)state);
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

        Debug.DrawRay(position, direction, Color.green);
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
        Debug.Log("State " + state);
        if (state == State.jumping)
        {
            if (rb.velocity.y < 1f)
            {
                state = State.falling;
            }
        }

        else if(state == State.falling)
        {
            if (IsGrounded())
            {
                state = State.idle;
            }
        }

        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            state = State.running;
        }
        else
        {
            state = State.idle;
        }
    }

    //Enemy test
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            sceneLoader.LoadScene(0);
        }
        if (collision.gameObject.tag == "Friend")
        {
            Debug.Log("Friend");
        }
    }
}
