using UnityEngine;

public class DefeatManager : MonoBehaviour
{
    public static DefeatManager Instance;

    [SerializeField] private GameObject defeatScreen;

    private void Awake()
    {
        Instance = this;

        Time.timeScale = 1f;

        if (defeatScreen != null)
            defeatScreen.SetActive(false);
    }

    public void ShowDefeat()
    {
        if (defeatScreen != null)
            defeatScreen.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }
}