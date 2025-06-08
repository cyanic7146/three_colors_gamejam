using System.Data;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float jumpForce = 14f;
    public float airAcceleration = 5f;

    public float groundAcceleration = 12f;
    public float maxAirSpeed = 6f;
    public float jumpBufferTime = 0.15f;
    public bool instantDirectionChange = true;
    private Rigidbody2D rb;
    private Vector2 currentMoveInput;
    private bool isGrounded;
    private bool jumpInputReceived;
    private float lastJumpPressTime;
    private float currentHorizontalSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHorizontalSpeed = 0f;
    }

    void FixedUpdate()
    {
        ProcessX();
        Jump();
    }

    void ProcessX()
    {
        float currentSpeedDirection = Mathf.Sign(currentHorizontalSpeed);

        float inputDirection = Mathf.Sign(currentMoveInput.x);

        bool changingDirection = inputDirection != 0 && inputDirection != currentSpeedDirection && Mathf.Abs(currentHorizontalSpeed) > 0.01f;
        if (isGrounded)
        {
            if (changingDirection && instantDirectionChange)
            {
            currentHorizontalSpeed = 0f;
            }

            float targetSpeed = currentMoveInput.x * moveSpeed;

            if (Mathf.Abs(currentMoveInput.x) > 0.01f)
            {
                currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, targetSpeed, groundAcceleration * Time.fixedDeltaTime);
            }
            else
            {
                currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, 0f, groundAcceleration * Time.fixedDeltaTime);
            }

            rb.linearVelocity = new Vector2(currentHorizontalSpeed, rb.linearVelocity.y);
        }
        else
        {
            float targetAirSpeed = currentMoveInput.x * maxAirSpeed;
            if (Mathf.Abs(currentMoveInput.x) > 0.01f)
            {
                currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, targetAirSpeed, airAcceleration * Time.fixedDeltaTime);
            }
            else
            {
                currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, 0f, airAcceleration * Time.fixedDeltaTime);
            }
            rb.linearVelocity = new Vector2(currentHorizontalSpeed, rb.linearVelocity.y);
        }
    }



    public void OnMovement(InputValue value)
    {
        currentMoveInput = value.Get<Vector2>();
    }
    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            jumpInputReceived = true;
            lastJumpPressTime = Time.time;
        }
        else
        {
            jumpInputReceived = false;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;

        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
    void Jump()
    {
        if (isGrounded && Time.time - lastJumpPressTime <= jumpBufferTime)
        {
            Debug.Log(Time.time - lastJumpPressTime);
            Debug.Log(jumpBufferTime);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            jumpInputReceived = false;
            lastJumpPressTime = 0f;
            isGrounded = false;
        }
    }


}
