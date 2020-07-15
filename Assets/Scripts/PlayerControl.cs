using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    //Start varibels
    private Rigidbody2D rb;
    private Animator anim;
    public LayerMask groundLayer;
    private enum State { idle, running, jumping, falling};
    private State state = State.idle;


    //Hero params
    [SerializeField] private float speed = 10f;
    [SerializeField] private float JumpForce = 10f;
    [SerializeField] private int cherries = 0;
    [SerializeField] private Text cherryText;

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
        InputManager();


        VeolcityState();
        anim.SetInteger("state", (int)state); // Set animation based on Enumerator state
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Collectable")
        {
            Destroy(collision.gameObject);
            cherries += 1;
            cherryText.text = cherries.ToString();
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
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded() == true) 
            {
                rb.velocity = new Vector2(rb.velocity.x, JumpForce);
                state = State.jumping;
            }
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
