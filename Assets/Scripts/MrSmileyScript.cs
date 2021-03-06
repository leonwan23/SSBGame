﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrSmileyScript : MonoBehaviour {

    Rigidbody2D rb;
    Animator anim;
    float dirX;
    public float moveSpeed = 5f;
    public float jumpForce = 50f;
    bool facingRight = true;
    Vector3 localScale;
    bool isOnGround = true;
    bool canDoubleJump = false;
    bool isCrouching = false;

    public float timeBetweenHadouken = 2f;
    public GameObject hadouken;
    private float nextHadouken;
    public float hadoukenSpeed;
    public Transform hadoukenPoint;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        localScale = transform.localScale;
        nextHadouken = 0;
        hadouken.GetComponent<SpriteRenderer>().flipX = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (isOnGround)
        {
            if (Input.GetButtonDown("Jump"))
            {
                rb.AddForce(Vector2.up * jumpForce);
                isOnGround = false;
                canDoubleJump = true;
            }
        } else
        {
            if(Input.GetButtonDown("Jump") && canDoubleJump)
            {
                canDoubleJump = false;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jumpForce);
            }
        }

        SetAnimationState();

        dirX = Input.GetAxisRaw("Horizontal") * moveSpeed;
	}

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX, rb.velocity.y);
    }

    private void LateUpdate()
    {
        CheckWhereToFace();
    }

    void SetAnimationState()
    {

        if (Input.GetKeyDown(KeyCode.C) && !anim.GetCurrentAnimatorStateInfo(0).IsName("Character_B_Attack") && nextHadouken<Time.time)  //BAttack
        {
            nextHadouken = Time.time + timeBetweenHadouken;
            Invoke("ShootHadouken", 0.3f);
            anim.SetBool("isRunning", false);
            anim.SetTrigger("BAttack");
        }

        if (dirX == 0 && rb.velocity.y ==0) //idle
        {
            anim.SetBool("isRunning", false);
            anim.SetBool("isJumping", false);
            anim.SetBool("isDoubleJump", false);
            anim.SetBool("isFalling", false);
        }

        if(Mathf.Abs(dirX) == moveSpeed && rb.velocity.y == 0) //running
        {
            anim.SetBool("isRunning", true);
        }

        if(rb.velocity.y == 0)
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", false);
        }

        if(rb.velocity.y > 0) //jump
        {
            if (canDoubleJump)
            {
                anim.SetBool("isRunning", false);
                anim.SetBool("isJumping", true);
                anim.SetBool("isDoubleJump", false);
            } else //double jump
            {
                anim.SetBool("isRunning", false);
                anim.SetBool("isDoubleJump", true);
            }
        }

        if (rb.velocity.y < 0) //fall
        {
            anim.SetBool("isRunning", false);
            anim.SetBool("isJumping", false);
            anim.SetBool("isDoubleJump", false);
            anim.SetBool("isFalling", true);
        }
    }

    void CheckWhereToFace()
    {
        if (dirX > 0)
        {
            facingRight = true;
            hadouken.GetComponent<SpriteRenderer>().flipX = false;

        } else if (dirX < 0)
        {
            facingRight = false;
            hadouken.GetComponent<SpriteRenderer>().flipX = true;            
        }

        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
        {
            localScale.x *= -1;
        }

        transform.localScale = localScale;
        hadoukenPoint.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)         //-----------BUGGY WHEN WALKING OFF PLATFORM------------///
    {
        if (collision.collider.tag == "Ground")
        {
            Vector3 contactPoint = collision.contacts[0].point;
            Vector3 center = collision.collider.bounds.center;
            if (contactPoint.y > center.y) //top of ground
            {
                isOnGround = true;
            } 
        }
    }

    void ShootHadouken()
    {
        GameObject clone = (GameObject)Instantiate(hadouken, hadoukenPoint.position, hadoukenPoint.rotation);
        if(facingRight)
            clone.GetComponent<Rigidbody2D>().AddRelativeForce(Vector3.right * hadoukenSpeed, ForceMode2D.Impulse);
        else
            clone.GetComponent<Rigidbody2D>().AddRelativeForce(Vector3.right * -hadoukenSpeed, ForceMode2D.Impulse);
    }
}
