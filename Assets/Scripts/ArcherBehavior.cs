// ArcherBehavior.cs
// This file was initially generated with Cursor and edited for use in this exercise.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBehavior : MonoBehaviour
{
    // VARIABLES - You shouldn't need to change these
    [SerializeField] private float range = 5f;              // The range at which the archer can shoot the player
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float arrowSpeed = 8f;         // The speed of the arrow
    [SerializeField] private float shootDelay = 0.3f;       // The delay before the arrow is shot
    private Animator animator;                              // The animator component
    private SpriteRenderer spriteRenderer;                  // The sprite renderer component
    private GameObject player;                              // The player game object
    [SerializeField] private float cooldown = 2f;           // The cooldown time between shots
    private float lastShotTime;                             // The time of the last shot

    private Vector3 startPosition; // Store initial position for reset

    // GET REFERENCES TO RELEVANT COMPONENTS AND GAME OBJECTS - You shouldn't need to change this
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        lastShotTime = -cooldown;
        startPosition = transform.position; // Remember starting position
    }

    ////////////////////////////////////////////////////////////
    // TODO: IMPLEMENT THE METHODS BELOW TO COMPLETE THE EXERCISE
    ////////////////////////////////////////////////////////////
    
    // PART 1: We need to check if the player is in range of the archer
    // Return true if the player is in range, false otherwise
    //
    // Hint: We have given you a method `calculateDistanceToPlayer()` to help you out with the distance calculation.
    // It is defined below if you want to understand how it works.
    // Hint: which of the state variables at the top of this file might you need for this function?
    private bool isInRange() {
        // TODO: Implement this method
        return false;
    }

    // PART 2: We need to check if the archer's shooting cooldown is over
    // Return true if the cooldown is over, false otherwise
    //
    // Hint: Use `Time.time` to get the current in-game time
    // Hint: You'll know the cooldown is over when the time since the last shot is greater than the cooldown
    private bool cooldownOver() {
        // TODO: Implement this method
        return false;
    }

    // PART 3: Finally, we need to put it all together and tell the archer how to behave on every frame
    // The follow is pseudo code for the archer's behavior:
    // On every frame...
    // 1. The archer should turn to face the player
    // 2. If the player is in range and the archer's cooldown is over:
    //   a. Make the archer show the shooting animation by triggering a transition into the shooting state. (Hint: animator.SetBool might be helpful)
    //   b. Set lastShotTime to the current in-game time
    //   c. Use `StartCoroutine(ShootArrow())` to start to shoot an arrow at the player
    //
    // Hint: You'll need to use the `isInRange()` and `cooldownOver()` methods you just implemented
    // Hint: One of the helper functions down below migth be useful here.
    //
    // Don't worry too much about what a coroutine is. Essentially, it is a way to run asynchronous code
    // that doesn't block the update function from finishing. We are using it here because the ShootArrow
    // method has a delay at the start that we don't want to wait on.
    // If you're curious, it may be useful to read about coroutines for your final project.
    // https://docs.unity3d.com/6000.3/Documentation/Manual/Coroutines.html
    void Update()
    {
        // TODO: Implement this method
    }


    ////////////////////////////////////////////////////////////
    // HELPER METHODS: USE THESE TO IMPLEMENT YOUR SOLUTION
    ////////////////////////////////////////////////////////////

    // Calculate the distance between the archer and the player
    private float calculateDistanceToPlayer() {
        return Vector3.Distance(transform.position, player.transform.position);
    }

    private void facePlayer()
    {
        // Track player and flip sprite based on position
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = player.transform.position.x < transform.position.x;
        }
    }

    ////////////////////////////////////////////////////////////
    // OTHER METHODS: NO NEED TO SCROLL PAST THIS POINT (Unless you're interested in the code)
    ////////////////////////////////////////////////////////////

    // Public method called by the player when attacking
    // Returns true if the archer was hit (within attack range), false otherwise
    public bool TryTakeDamage(Vector3 attackerPosition, float attackRange = 1.0f)
    {
        float distance = Vector3.Distance(transform.position, attackerPosition);
        if (distance <= attackRange)
        {
            // Play death effect (smoke animation) if DeathEffect component exists
            DeathEffect deathEffect = GetComponent<DeathEffect>();
            if (deathEffect != null)
            {
                deathEffect.PlayDeathEffect();
            }
            else
            {
                // Fallback: destroy immediately if no death effect component
                Destroy(gameObject);
            }
            return true;
        }
        return false;
    }

    private IEnumerator ShootArrow()
    {
        // Wait for the animation timing
        yield return new WaitForSeconds(shootDelay);

        // Re-check if player is still in range after delay (prevents firing at respawned player)
        if (arrowPrefab != null && player != null && isInRange())
        {
            // Spawn the arrow at the Archer's position
            Vector3 spawnPosition = transform.position;

            // Calculate direction to player
            Vector3 direction = (player.transform.position - spawnPosition).normalized;

            // Calculate rotation to point at the player
            Quaternion arrowRotation = CalculateRotationFromDirection(direction);

            // Instantiate the arrow with rotation pointing at the player
            GameObject arrow = Instantiate(arrowPrefab, spawnPosition, arrowRotation);

            // Set the arrow's direction and speed
            Arrow arrowScript = arrow.GetComponent<Arrow>();
            if (arrowScript != null)
            {
                arrowScript.SetDirection(direction, arrowSpeed);
            }
        }
    }

    private Quaternion CalculateRotationFromDirection(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }

    /// <summary>
    /// Resets the archer to its initial state for game reset.
    /// Called by GameManager when the game restarts.
    /// </summary>
    public void ResetArcher()
    {
        // Re-enable components via DeathEffect if it exists
        DeathEffect deathEffect = GetComponent<DeathEffect>();
        if (deathEffect != null)
        {
            deathEffect.ReenableComponents();
        }
        
        // Reset position to starting position
        transform.position = startPosition;
        
        // Reset cooldown so archer doesn't immediately shoot
        lastShotTime = -cooldown;
    }
}
