using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EntreceManager : MonoBehaviour
{

    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    void Start()
    {
        startButton.onClick.AddListener(DoStart);
        quitButton.onClick.AddListener(DoQuit);
    }

    void DoStart()
    {
        SceneManager.LoadScene("MainScene");
    }

    void DoQuit()
    {
        Application.Quit();
    }
}
