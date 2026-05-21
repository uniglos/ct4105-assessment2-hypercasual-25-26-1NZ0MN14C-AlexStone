using UnityEngine;

public class Kunai : MonoBehaviour
{
    private Rigidbody rb;
    private bool isStuck = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isStuck) return;

        if (collision.gameObject.CompareTag("targetlog"))
        {
            StickToTarget(collision.transform);
        }
        else if (collision.gameObject.CompareTag("Kunai"))
        {
           // Debug.Log(" Hit another kunai!");
            if (GameManager.Instance != null)
                GameManager.Instance.GameOver();
        }
    }



    void StickToTarget(Transform targetParent)
    {
        isStuck = true;
        
        // Stop all movement
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        // Stick to the log
        transform.SetParent(targetParent, true);

        Vector3 dirToCenter = (transform.position - targetParent.position).normalized;
        
        transform.position = targetParent.position + dirToCenter * 1.6f;

        // Point blade into the center
        transform.LookAt(targetParent.position);
        transform.Rotate(0, 180, 0);

        // Fix scale and weird rotation cause damn it sucks
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);


        if (GameManager.Instance != null)
            GameManager.Instance.KunaiStuck();

        //Debug.Log("Kunai stuck!");
    }
}