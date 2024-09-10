using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class plrMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private BoxCollider2D boxcollider;
    private Animator anim;
    private bool holdingJump = false;
    private float jumpMultiplier;
    private bool canStomp = true;
    private bool walking = false;
    private bool stomping = false;
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float stompPower;
    [SerializeField] private GameObject run_fx;


    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxcollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //flip
        if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(10, 10, 1);
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-10, 10, 1);
        }

        anim.SetFloat("moveY", body.velocity.y);

        //jump
        if (Input.GetKeyDown("space") && isGrounded())
        {
            anim.SetBool("holdingJump", true);
            holdingJump = true;
        }

        if (Input.GetKeyUp("space") && isGrounded())
        {
            StartCoroutine(Jump());
        }

        //movement left-right

        if(horizontalInput == 0)
        {
            walking = false;
        }
        else
        {
            if (!walking)
            {
                walking = true;

                GameObject fx = Instantiate(
                run_fx, gameObject.transform.position + new Vector3(-Mathf.Sign(gameObject.transform.localScale.x) * 15, 2)
                ,Quaternion.Euler(0, 0, 0));
                fx.transform.localScale = new Vector3(Mathf.Sign(gameObject.transform.localScale.x)*7, 7, 1);
                Destroy(fx, .5f);
            }
        }

        if (!holdingJump && !stomping)
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
            jumpMultiplier = jumpPower;
        }
        else
        {
            body.velocity = new Vector2(horizontalInput * speed / 3, body.velocity.y);

            if(jumpMultiplier > 2*jumpPower)
            {
                jumpMultiplier = 2*jumpPower;
            }
            else
            {
                jumpMultiplier += 100*Time.deltaTime;
            }
        }

        //wall climb
        if (onWall())
        {
            body.gravityScale = 30;
            transform.rotation = Quaternion.AngleAxis(Mathf.Sign(transform.localScale.x) * 90, Vector3.forward);

            body.velocity = new Vector2(body.velocity.x, verticalInput * speed);
            anim.SetBool("grounded", true);
        }
        else
        {
            body.gravityScale = 50;
            transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
        }

        //stomp
        if(isGrounded())
        {
            canStomp = true;
            stomping = false;
        }
        if(Input.GetKeyDown(KeyCode.S) && !isGrounded() && canStomp)
        {
            StartCoroutine(stomp());
        }
    }

    private IEnumerator stomp()
    {
        body.velocity = Vector2.zero;

        yield return new WaitForSeconds(.3f);
        body.velocity = new Vector2(body.velocity.x, -stompPower);

        canStomp = false;
        stomping = true;
    }

    private IEnumerator Jump()
    {
        anim.SetBool("holdingJump", false);
        yield return new WaitForSeconds(.2f);
        anim.SetBool("grounded", false);
        holdingJump = false;
        body.velocity = new Vector2(body.velocity.x, jumpMultiplier);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Wall")
        {
            anim.SetBool("grounded", true);
        }
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxcollider.bounds.center, boxcollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxcollider.bounds.center, boxcollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

}
