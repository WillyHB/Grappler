using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float Speed = 100;

    public float JumpForce = 200;

    public float StopAccelerant = 0.1f;

    public float StartAccelerant;

    private float accelerant;

    [Header("Ground Check")]
    public LayerMask GroundLayerMask;
    public float GroundedCheckRadius = 0.04f;

    private bool isGrounded = false;

    private Rigidbody2D rb;

    public bool IsGrappling;
    public float GrapplePower = 50;

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();

        GetComponent<PlayerInput>().actions["Jump"].started += Jump;

    }

    private void Jump(InputAction.CallbackContext cc)
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y < 0 ? 0 : rb.velocity.y);
            rb.AddForce(Vector2.up * JumpForce);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider2D hit
            = Physics2D.OverlapCircle(
                new Vector2(transform.position.x, transform.position.y - GetComponent<CapsuleCollider2D>().size.y / 2),
                GroundedCheckRadius, GroundLayerMask);

        isGrounded = hit != null;

        float val = GetComponent<PlayerInput>().actions["Move"].ReadValue<float>();

        if (val == 0)
        {
            if (accelerant < 0)
            {
                accelerant += Mathf.Min(StopAccelerant * Time.deltaTime, -accelerant);
            }

            else if (accelerant > 0)
            {
                accelerant -= Mathf.Min(StopAccelerant * Time.deltaTime, accelerant);
            }

        }

        else
        {
            float speed = val * (Speed * Time.deltaTime);

            if (val > 0)
            {
                if (accelerant < speed)
                {
                    accelerant += StartAccelerant * Time.deltaTime;
                }

                else
                {
                    accelerant = speed;
                }


            }

            else if (val < 0)
            {
                if (accelerant > speed)
                {
                    accelerant -= StartAccelerant * Time.deltaTime;
                }

                else
                {
                    accelerant = speed;
                }

            }
        }

        if (!IsGrappling)
        {
            rb.velocity = new Vector2(accelerant == 0 ? rb.velocity.x : accelerant, rb.velocity.y);
        }

        else
        {
            rb.AddForce(new Vector2(val * (GrapplePower * Time.deltaTime), 0));
        }

    }
}
