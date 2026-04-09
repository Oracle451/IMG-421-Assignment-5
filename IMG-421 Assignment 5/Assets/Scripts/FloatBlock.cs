using UnityEngine;

public class FloatBlock : MonoBehaviour
{
    void Start()
    {
        // Only drift the blocks if on medium or easy
        if (GameManager.Instance.currentDifficulty != GameManager.Difficulty.Hard)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            // Turn off gravity
            rb.useGravity = false;
            // Gentle random drift so they don't just hang in place
            rb.velocity = new Vector3(
                Random.Range(-0.5f, 0.5f),
                Random.Range(0.2f, 0.8f),
                0
            );
        }
    }

    // When hit by a projectile
    void OnCollisionEnter(Collision col)
    {
        // Once again, only activate if not on hard mode
        if (GameManager.Instance.currentDifficulty != GameManager.Difficulty.Hard)
        {
            if (col.gameObject.GetComponent<Projectile>() != null)
            {
                // Fly off in a random direction on hit
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<Rigidbody>().AddForce(
                    new Vector3(Random.Range(-3f, 3f), Random.Range(2f, 5f), 0),
                    ForceMode.Impulse
                );
            }
        }
    }
}