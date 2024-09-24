using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{
    public ParticleSystem collisionParticleSystemPrefab;
    //public bool once = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object has the "Enemy" tag
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Ensure the particle system is assigned and exists
            if (collisionParticleSystemPrefab != null)
            {
                // Play the particle system
                //collisionParticleSystemPrefab.transform.position = other.transform.position; // Set particles to spawn at the collision point
                ParticleSystem collisionParticleSystem = Instantiate(collisionParticleSystemPrefab, other.transform.position, Quaternion.identity);
                collisionParticleSystem.Play();
                
                //once = false;  // Ensure this only happens once

                Debug.Log("Particle System in effect.");

                // Optionally destroy the hole object after particles finish playing
                Destroy(collisionParticleSystem, collisionParticleSystemPrefab.main.duration + collisionParticleSystemPrefab.main.startLifetime.constant);
            }
            else
            {
                Debug.LogError("No particle system assigned to the script!");
            }
        }
    }
}


