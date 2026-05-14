using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Kills")]
    public int currentKills;
    public int killsToWin = 20;

    [Header("UI")]
    public TextMeshProUGUI killText;
    public GameObject victoryScreen;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();

        if (victoryScreen != null)
            victoryScreen.SetActive(false);
    }

    public void AddKill()
    {
        currentKills++;

        UpdateUI();

        if (currentKills >= killsToWin)
        {
            Victory();
        }
    }

    void UpdateUI()
    {
        killText.text =
            "Mortes:" + currentKills + " / " + killsToWin;
    }

    void Victory()
    {
        if (victoryScreen != null)
            victoryScreen.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void GoMenu(string sceneName)
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(sceneName);
    }
}