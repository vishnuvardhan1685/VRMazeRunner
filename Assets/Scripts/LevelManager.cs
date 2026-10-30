
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public GameObject completionPanel;
    public GameObject nextLevelButton;
    public TMP_Text completionTitle;

    private bool levelCompleted;
    public bool IsLevelCompleted => levelCompleted;

    void Start()
    {
        Time.timeScale = 1f;
        levelCompleted = false;

        if (completionPanel != null)
            completionPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void CompleteLevel()
    {
        if (levelCompleted)
            return;

        levelCompleted = true;

        bool isLastLevel =
            SceneManager.GetActiveScene().buildIndex ==
            SceneManager.sceneCountInBuildSettings - 1;

        if (completionPanel != null)
            completionPanel.SetActive(true);

        if (completionTitle != null)
        {
            completionTitle.text = isLastLevel
                ? "GAME COMPLETED!"
                : "LEVEL COMPLETED!";
        }

        if (nextLevelButton != null)
            nextLevelButton.SetActive(!isLastLevel);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;

        int nextIndex =
            SceneManager.GetActiveScene().buildIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
    }
}
