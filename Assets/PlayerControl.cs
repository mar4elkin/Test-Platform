using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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
            //Running animation
            anim.SetBool("running", true);
        }
        else if (hDirection > 0)
        {
            //Go left
            rb.velocity = new Vector2(5, rb.velocity.y);
            //Sprite flipping
            transform.localScale = new Vector2(1, 1);
            //Running animation
            anim.SetBool("running", true);
        }

        else
        {
            //Running animation stop
            anim.SetBool("running", false);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            //Jump
            rb.velocity = new Vector2(rb.velocity.x, 10f);
        }
    }
}
