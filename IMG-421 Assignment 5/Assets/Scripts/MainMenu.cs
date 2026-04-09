using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Function to play the game scene
    public void PlayGame(int difficultyIndex)
    {
        GameManager.Instance.SetDifficulty(difficultyIndex);
        SceneManager.LoadScene("SampleScene");
    }

    // Function to quit the game
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }
}