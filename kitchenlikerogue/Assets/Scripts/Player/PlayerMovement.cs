using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private Vector2 moveInput;
    private Rigidbody2D rb;
    private bool isDashing;
    private bool canDash = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input from keyboard or gamepad
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (moveInput.magnitude > 1)
        {
            moveInput.Normalize();
        }

        // Check for dash input
        if (canDash && (Input.GetButtonDown("Dash") || Input.GetKeyDown(KeyCode.Space)))
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            // Move the player
            rb.velocity = moveInput * moveSpeed;
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;

        Vector2 dashDirection = moveInput;

        if (dashDirection == Vector2.zero)
        {
            dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        }

        rb.velocity = dashDirection * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(dashCooldown - dashDuration);

        canDash = true;
    }
}