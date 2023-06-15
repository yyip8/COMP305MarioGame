using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float destroyDelay = 0.5f; // delay in seconds before destroying the coin
    private Animator animator; // Animator for the coin

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("used", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Mario playerController = collision.GetComponent<Mario>();
        if (playerController != null)
        {
            playerController.CollectCoin();
            // Play the animation
            animator.SetBool("used", true);
            // Destroy the coin after the animation has played
            Destroy(gameObject, destroyDelay);
        }
    }
}
