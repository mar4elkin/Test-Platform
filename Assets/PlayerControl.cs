using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            //Go right
            rb.velocity = new Vector2(-5, rb.velocity.y);
            //Sprite flipping
            transform.localScale = new Vector2(-1, 1);
            //Running animation
            anim.SetBool("running", true);
        }
        else if (Input.GetKey(KeyCode.D))
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
