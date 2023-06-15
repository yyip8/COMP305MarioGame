using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    public float sprintMultiplier = 1.5f;
    public Camera mainCamera;
    public float minHeight = 1f;
    public float maxHeight = 5f;
    public float minCameraSize = 5f;
    public float maxCameraSize = 10f;
    public float minFieldOfView = 10f;
    public float maxFieldOfView = 20f;
    public float skidDuration = 0.5f;
    public int coinCount = 0;
    
    private Rigidbody2D rb;
    private bool isJumping = false;
    private bool isSkidding = false;
    private Vector3 cameraOffset;
    private Animator animator;
    private float previousMoveX;
    private SpriteRenderer spriteRenderer;

    public void CollectCoin()
    {
        coinCount++;
        Debug.Log("Coins: " + coinCount);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        cameraOffset = mainCamera.transform.position - transform.position;
        previousMoveX = 0f;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        bool sprint = Input.GetKey(KeyCode.LeftShift);
        float xvel = rb.velocity.x;

        // Move the character horizontally only if it is not jumping
        if (!isJumping)
        {
            rb.velocity = new Vector2(
                moveX * speed * (sprint ? sprintMultiplier : 1),
                rb.velocity.y
            );
            if (xvel > 0)
            {
                spriteRenderer.flipX = isSkidding;
            }
            else if (xvel < 0)
            {
                spriteRenderer.flipX = !isSkidding;
            }
        }

        // Check if the character is skidding
        isSkidding = sprint && (Mathf.Sign(moveX) != Mathf.Sign(previousMoveX));

        previousMoveX = moveX;

        if (isSkidding)
        {
            StopCoroutine("EndSkid");
            StartCoroutine("EndSkid");
        }

        // Update animator parameters
        animator.SetBool("jumping", isJumping);
        animator.SetFloat("xvel", Mathf.Abs(xvel));
        animator.SetBool("isMega", false);

        // Jump if the jump key is pressed
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            isJumping = true;
            float actualJumpForce = jumpForce * (sprint ? sprintMultiplier : 1);
            if (rb.velocity != Vector2.zero)
                actualJumpForce *= sprintMultiplier;
            rb.AddForce(new Vector2(0, actualJumpForce), ForceMode2D.Impulse);
        }

        // Update camera position to follow Mario and adjust size based on Mario's height
        float height = transform.position.y;
        float t = (height - minHeight) / (maxHeight - minHeight);
        if (mainCamera.orthographic)
        {
            mainCamera.orthographicSize = Mathf.Lerp(minCameraSize, maxCameraSize, t);
        }
        else
        {
            mainCamera.fieldOfView = Mathf.Lerp(minFieldOfView, maxFieldOfView, t);
        }
        Vector3 newPosition = transform.position + cameraOffset;
        mainCamera.transform.position = newPosition;
    }

    private IEnumerator EndSkid()
    {
        animator.SetBool("skidding", true);
        yield return new WaitForSeconds(skidDuration);
        animator.SetBool("skidding", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Detect if the character is touching the ground
        if (collision.gameObject.CompareTag("Land"))
        {
            isJumping = false;
        }
    }
}
