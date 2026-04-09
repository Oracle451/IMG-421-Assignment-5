using UnityEngine;

public class FloatBlock : MonoBehaviour
{
    void Start()
    {
        if (GameManager.Instance.currentDifficulty != GameManager.Difficulty.Hard)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            // Gentle random drift so they don't just hang there
            rb.velocity = new Vector3(
                Random.Range(-0.5f, 0.5f),
                Random.Range(0.2f, 0.8f),
                0
            );
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (GameManager.Instance.currentDifficulty != GameManager.Difficulty.Hard)
        {
            if (col.gameObject.GetComponent<Projectile>() != null)
            {
                // Pop off in a random direction on hit
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<Rigidbody>().AddForce(
                    new Vector3(Random.Range(-3f, 3f), Random.Range(2f, 5f), 0),
                    ForceMode.Impulse
                );
            }
        }
    }
}