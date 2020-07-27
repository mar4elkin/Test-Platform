using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;
    protected AudioSource death;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        death = GetComponent<AudioSource>();
    }

    //When player jumped on this enemy
    public void JumpedOn()
    {
        anim.SetTrigger("Death");//Death animation
        death.Play(); //Play sound
        rb.bodyType = RigidbodyType2D.Kinematic; //It have to don't let animation mooving, but it dosen't work
        GetComponent<Collider2D>().enabled = false; //The same
    }

    private void Death()
    {
        //Destroy enemy
        Destroy(this.gameObject);
    }
}
