﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Frog : MonoBehaviour
{

    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;
    public LayerMask groundLayer;

    [SerializeField] private float jumpLength = 10f;
    [SerializeField] private float jumpHeight = 15f;
    private Rigidbody2D rb;

    private bool facingLeft = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (facingLeft)
        {
            //test to see if we are beyond left cap
            if(transform.position.x > leftCap)
            {
                //Sprite facing check and flipping
                if(transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }

                //Tets to see if I on th ground, if so, Jump
                if (IsGrounded())
                {
                    //Jump
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                }
            }
            else
            {
                facingLeft = false;
            }

        }

        else
        {
            //test to see if we are beyond right cap
            if (transform.position.x < rightCap)
            {
                //Sprite facing check and flipping
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }

                //Tets to see if I on th ground, if so, Jump
                if (IsGrounded())
                {
                    //Jump
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                }
            }
            else
            {
                facingLeft = true;
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
}