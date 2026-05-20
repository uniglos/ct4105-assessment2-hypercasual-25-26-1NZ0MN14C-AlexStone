using UnityEngine;

public class TargetRotator : MonoBehaviour
{
    [Header("Rotation")]
    public float baseSpeed = 90f;        // degrees per second, remeber that idiot
    
    private float currentSpeed;

    void Start()
    {
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        // Rotate around its own Y axis make sure its spins correct
        transform.Rotate(0, currentSpeed * Time.deltaTime, 0);
    }

    // use this to make it harder for game play loop
    public void IncreaseDifficulty()
    {
        currentSpeed += 18f;
    }
}