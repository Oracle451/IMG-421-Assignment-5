using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    // How fast it moves
    public float speed = 1f;
    // Distance it travels
    public float range = 3f;

    // Where it starts
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;

        // Change speed and range with difficulty
        switch (GameManager.Instance.currentDifficulty)
        {
            case GameManager.Difficulty.Medium: 
                speed = 2f; 
                range = 8f;
                break;
            case GameManager.Difficulty.Hard: 
                speed = 4f; 
                range = 5f;
                break;
        }
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * speed) * range;
        transform.position = startPos + new Vector3(0, offset, 0);
    }
}