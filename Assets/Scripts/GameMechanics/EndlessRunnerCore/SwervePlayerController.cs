
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SwervePlayerController : MonoBehaviour
{
    public float moveDistance = 1;
    public float maxSwerveAmount = 1;

    Rigidbody rb;
    Vector2 currentSwerve = Vector2.zero;

    bool isGrounded;
    public float JumpPower = 1;
    public bool AllowJumping = true;

    public float groundedOffset = -0.14f;
    public LayerMask GroundLayerMask;

    Vector3 SpherePos = Vector3.zero;

    [Header("Death Settings")]
    public LayerMask deathLayer;

    [Header("Animation Controller")]
    public RunnerAnimationController animationController;   // <<< NEW

    bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Debug.Log(isGrounded);
    }

    private void FixedUpdate()
    {
        // Ground check
        SpherePos = new Vector3(transform.position.x, transform.position.y + groundedOffset, transform.position.z);
        isGrounded = Physics.CheckSphere(SpherePos, 0.5f, GroundLayerMask);

        // Auto return to Run animation when grounded
        if (!isDead && isGrounded && animationController != null)
        {
            animationController.ReturnToRun();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(SpherePos, 0.5f);
    }

    /// <summary>
    /// Called from swipe/touch input. 
    /// Dir.x != 0 => swerve. Dir.x == 0 => jump.
    /// </summary>
    public void SwervePlayer(Vector2 Dir)
    {
        if (isDead) return;

        // SWERVE left/right
        if (Dir.x != 0)
        {
            if (currentSwerve.x + Dir.x <= Vector2.right.x * maxSwerveAmount &&
                currentSwerve.x + Dir.x >= Vector2.left.x * maxSwerveAmount)
            {
                transform.position = new Vector3(
                    transform.position.x + (Dir.x * moveDistance),
                    transform.position.y,
                    transform.position.z
                );

                currentSwerve += Dir;
            }
        }
        else
        {
            HandleJump();
        }
    }

    private void HandleJump()
    {
        if (AllowJumping && isGrounded)
        {
            rb.AddForce(Vector3.up * JumpPower, ForceMode.VelocityChange);

            if (animationController != null)
                animationController.PlayJump();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        // Death layer check
        if (((1 << other.gameObject.layer) & deathLayer) != 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        if (animationController != null)
            animationController.PlayDeath();
    }
}
