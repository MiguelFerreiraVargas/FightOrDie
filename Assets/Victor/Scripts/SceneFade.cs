using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeEffect : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1.5f;
    public string sceneName;

    private bool isFading = false;

    void Awake()
    {
        canvasGroup.alpha = 0f;
    }

    public void StartFade()
    {
        if (!isFading)
        {
            StartCoroutine(FadeAndLoad());
        }
    }

    IEnumerator FadeAndLoad()
    {
        isFading = true;

        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = time / fadeDuration;
            yield return null;
        }

        canvasGroup.alpha = 1f;

        SceneManager.LoadScene(sceneName);
    }
}