using System.Collections;
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

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        localScale = transform.localScale;
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
        if (dirX == 0 && isOnGround) //idle
        {
            anim.SetBool("isRunning", false);
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
            } else
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
        } else if (dirX < 0)
        {
            facingRight = false;
        }

        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
        {
            localScale.x *= -1;
        }

        transform.localScale = localScale;
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
}
