using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1Movement : MonoBehaviour

{
    public float moveSpeed = 3f;
    public float jumpForce = 5f;
    private bool isGrounded;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

        float moveInput = 0;
        if (Input.GetKey(KeyCode.A))
            moveInput = -1;
        else if (Input.GetKey(KeyCode.D))
            moveInput = 1;

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (isGrounded && Input.GetKeyDown(KeyCode.W))
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            isGrounded = false; 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
