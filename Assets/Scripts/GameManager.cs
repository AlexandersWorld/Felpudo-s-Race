using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private Button tryAgainButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private GameObject gameOverScreen;

    [SerializeField] private PlayerController2D felpudo;

    [SerializeField]
    private float timeRemaining = 120f;

    private bool timerRunning = true;

    void Start()
    {
        timerText.text = "3:00";

        tryAgainButton.onClick.AddListener(DoTryAgain);
        quitButton.onClick.AddListener(DoQuit);

        gameOverScreen.SetActive(false);
        Time.timeScale = 1f;
    }
    void Update()
    {
        if (timeRemaining <= 0)
        {
            GameOver();
        }

        if (felpudo.GetHealth() <= 0)
        {
            GameOver();
        }

        if (timerRunning)
        {
            if (timeRemaining >= 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay(timeRemaining);
            }
            else
            {
                 
                timeRemaining = 0;
                timerRunning = false;

                Debug.Log("Time's up!");
                timerText.text = "0:00";
            }
        }
    }

    void GameOver()
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    void UpdateTimerDisplay(float timeToDisplay)
    {
        timeToDisplay = Mathf.Max(timeToDisplay, 0);
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);
        int milliseconds = Mathf.FloorToInt((timeToDisplay * 100) % 100);

        timerText.text = $"{minutes}:{seconds:00}.{milliseconds:00}";
    }

    void DoTryAgain()
    {
        SceneManager.LoadScene("MainScene");
    }

    void DoQuit()
    {
        Application.Quit();
    }
}
