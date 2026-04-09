using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    public Difficulty currentDifficulty;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetDifficulty(int difficultyIndex)
    {
        currentDifficulty = (Difficulty)difficultyIndex;
    }
}
