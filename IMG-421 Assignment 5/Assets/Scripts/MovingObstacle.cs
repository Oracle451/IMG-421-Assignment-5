using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    public float speed = 1f;
    public float range = 3f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;

        // Scale speed with difficulty
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