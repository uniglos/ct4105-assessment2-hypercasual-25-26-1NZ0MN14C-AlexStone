
using UnityEngine;

public class RunnerAnimationController : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;

    [Header("Animator Parameters")]
    public string runBool = "isRunning";
    public string jumpTrigger = "isJumping";
    public string deathTrigger = "isDead";
    public string jumpDistanceParam = "JumpDistance";
    public string jumpSpeedParam = "JumpSpeed";

    [Header("Jump Animation Controls")]
    public float jumpAnimationDistance = 1f;
    public float jumpAnimationSpeed = 1f;

    private bool isDead = false;

    private void Start()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        animator.SetBool(runBool, true);
        PushJumpAnimParams();
    }

    public void PlayJump()
    {
        if (isDead) return;

        PushJumpAnimParams();
        animator.SetTrigger(jumpTrigger);
        animator.SetBool(runBool, false);
    }

    public void PlayDeath()
    {
        if (isDead) return;

        isDead = true;
        animator.SetBool(runBool, false);
        animator.SetTrigger(deathTrigger);
    }

    public void ReturnToRun()
    {
        if (!isDead)
            animator.SetBool(runBool, true);
    }

    private void PushJumpAnimParams()
    {
        animator.SetFloat(jumpDistanceParam, jumpAnimationDistance);
        animator.SetFloat(jumpSpeedParam, jumpAnimationSpeed);
    }
}
