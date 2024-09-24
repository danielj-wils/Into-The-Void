using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameObject arrow;              

    public float maxMoveSpeed = 5f;         // Maximum movement speed
    public float acceleration = 10f;        // How quickly the player accelerates
    public float deceleration = 8f;         // How quickly the player decelerates
    public float rotationSpeed = 200f;      // Speed at which the player rotates toward the mouse
    public float inertia = 3f;              // How quickly velocity diminishes after stopping
    public float deadZone = 0.1f;           // Dead zone to prevent small, jittery movements

    private Vector2 velocity = Vector2.zero;  // Current movement velocity

    public int health;
    int score;
    public TMP_Text healthDisplay;
    public TMP_Text scoreDisplay;

    void Update()
    {
        // Handle movement with inertia and dead zone
        SmoothMovePlayerWithInertia();

        // Handle smooth rotation toward the mouse
        RotatePlayerToMouse();

        // Move the arrow in a circular path and rotate it toward the mouse
        MoveArrowAroundPlayer();
    }

    void SmoothMovePlayerWithInertia()
    {
        // Get input for movement
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 inputDirection = new(moveX, moveY);

        // Apply dead zone: ignore small input values
        if (inputDirection.magnitude < deadZone)
        {
            inputDirection = Vector2.zero;
        }

        // Normalize input if it's above the dead zone
        inputDirection = inputDirection.normalized;

        // Accelerate if there's input, otherwise apply inertia (deceleration)
        if (inputDirection.magnitude > 0)
        {
            // Accelerate toward the desired direction
            velocity = Vector2.MoveTowards(velocity, inputDirection * maxMoveSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            // Apply inertia: gradually reduce velocity if there's no input
            velocity = Vector2.MoveTowards(velocity, Vector2.zero, inertia * Time.deltaTime);
        }

        // Move the player using the current velocity
        transform.Translate(velocity * Time.deltaTime, Space.World);
    }

    void RotatePlayerToMouse()
{
    // Get mouse position in world coordinates
    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    mousePosition.z = 0f; // Ensure we're in 2D

    // Calculate the direction from the player to the mouse
    Vector2 direction = (mousePosition - transform.position).normalized;
    float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

    // Smoothly rotate toward the mouse position
    Quaternion currentRotation = transform.rotation;
    Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);

    // Calculate the angle difference
    float angleDifference = Quaternion.Angle(currentRotation, targetRotation);

    // Only rotate if the angle difference is significant
    if (angleDifference > 0.1f) // You can adjust this threshold
    {
        // Smooth rotation with rotationSpeed
        transform.rotation = Quaternion.RotateTowards(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}

    void MoveArrowAroundPlayer()
    {
        // Get mouse position in world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Ensure we're in 2D

        // Calculate the direction from the player to the mouse
        Vector2 direction = (mousePosition - transform.position).normalized;

        // Calculate the new position for the arrow (on a circle around the player)
        Vector3 arrowPosition = transform.position + (Vector3)direction;

        // Set the arrow's position to be on the circle
        arrow.transform.position = arrowPosition;

        // Calculate the target angle to point towards the mouse
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Add 90 degrees to the target angle
        float rotationAngle = targetAngle - 90f; // Adjust 90f if needed based on your sprite's orientation

        // Set the arrow's rotation
        arrow.transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);
    }

    public void TakeDamage()
    {
        health--;
        healthDisplay.text = "Health: " + health;
        if (health <= 0)
        {
            SceneManager.LoadScene("Game");
        }
    }

    public void AddScore()
    {
        score++;
        scoreDisplay.text = "Score: " + score;
    }
}
