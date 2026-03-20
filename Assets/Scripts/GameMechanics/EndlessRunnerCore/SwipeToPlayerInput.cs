
using UnityEngine;

public class SwipeToPlayerInput : MonoBehaviour
{
    [Header("References")]
    public SwervePlayerController player;
    public RunnerAnimationController animations;

    [Header("Swipe Thresholds")]
    public float horizontalThreshold = 0.4f;
    public float verticalThreshold = 0.2f;

    /// <summary>
    /// Hook this to SwipeFunctions.onSwipeDirectionNormalized
    /// </summary>
    public void OnSwipeDirection(Vector2 dir)
    {
        // Horizontal lane changes
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > horizontalThreshold)
            {
                player.SwervePlayer(new Vector2(+1, 0));   // right
            }
            else if (dir.x < -horizontalThreshold)
            {
                player.SwervePlayer(new Vector2(-1, 0));   // left
            }
            return;
        }

        // Vertical jump
        if (dir.y > verticalThreshold)
        {
            player.SwervePlayer(Vector2.zero); // triggers jump in your controller
            if (animations != null)
                animations.PlayJump();
        }
    }
}
