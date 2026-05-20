using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    public Transform target;
    public GameObject kunai;
    public Transform throwPoint;

    [Header("UI")]
    public TextMeshProUGUI stateText;
    public TextMeshProUGUI scoreText;

    [Header("Game Settings")]
    public int kunaiNeededPerState = 8;

    private int stuckCount = 0;
    private int score = 0;
    private int currentState = 1;

    private float throwCooldown = 0f;
    private float throwDelay = 0.35f;

    private void Awake()
    {
        Instance = this;
    }

    public void ThrowKunai()
    {
        if (Time.time < throwCooldown) return;
        if (kunai == null || throwPoint == null || target == null) return;

        GameObject newKunai = Instantiate(kunai, throwPoint.position, throwPoint.rotation);
        
        Rigidbody rb = newKunai.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = (target.position - throwPoint.position).normalized;
            rb.AddForce(direction * 38f, ForceMode.Impulse);
        }

        throwCooldown = Time.time + throwDelay;
    }

    public void KunaiStuck()
    {
        stuckCount++;
        score += 100;

        if (Random.value < 0.3f)
            score += 200;

        UpdateUI();

        if (stuckCount >= kunaiNeededPerState)
        {
            NextState();
        }
    }

    void NextState()
    {
        currentState++;
        stuckCount = 0;

        TargetRotator rotator = target.GetComponent<TargetRotator>();
        if (rotator != null)
            rotator.IncreaseDifficulty();

        UpdateUI();
    }

    void UpdateUI()
    {
        if (stateText) stateText.text = "State: " + currentState;
        if (scoreText) scoreText.text = score.ToString();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || 
            (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            ThrowKunai();
        }
    }

    public void GameOver()
    {
        Debug.Log("💀 GAME OVER - State: " + currentState + " | Score: " + score);
        Time.timeScale = 0f;
    }
}