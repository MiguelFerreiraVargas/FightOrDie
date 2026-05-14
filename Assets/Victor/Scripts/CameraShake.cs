using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private Coroutine currentShake;

    private void Awake()
    {
        Instance = this;
    }

    public void Shake(float duration, float strength)
    {
        if (currentShake != null)
            StopCoroutine(currentShake);

        currentShake =
            StartCoroutine(
                ShakeRoutine(duration, strength)
            );
    }

    IEnumerator ShakeRoutine(float duration, float strength)
    {
        Vector3 startPos = transform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            Vector3 randomOffset =
                Random.insideUnitSphere * strength;

            transform.localPosition =
                startPos + randomOffset;

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = startPos;
    }
}