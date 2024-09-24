using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    Player player;

    bool inactive = false;

    public float shakeStrength = 0.03f;
    public float shakeDuration = 0.2f;

    // Reference to the TrailRenderer component
    public TrailRenderer trail;

    // Start is called before the first frame update
    void Start()
    {
        // Find the player in the scene
        player = FindObjectOfType<Player>();

        // Optionally, find the TrailRenderer if not assigned via Inspector
        if (trail == null)
        {
            trail = GetComponent<TrailRenderer>(); // Assumes TrailRenderer is on the same GameObject
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move toward the player
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!inactive)  // Check if not inactive
        {
            if (other.CompareTag("Hole"))
            {
                player.AddScore();
                Destroy(gameObject);
            }

            if (other.CompareTag("Player") || other.CompareTag("Enemy"))
            {
                CameraShake.Shake(shakeDuration, shakeStrength);
                player.TakeDamage();
                speed = 0;
                transform.parent = player.transform;
            }

            inactive = true;

            // Safely destroy the TrailRenderer component
            if (trail != null)
            {
                Destroy(trail); // Proper way to remove a component during runtime
            }
        }
    }
}
