using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraFadeManager : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 2.0f;
    public float fadeInWaitTime = 4.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void doVisualFade()
    {
        StartCoroutine(StartFade(fadeImage, fadeDuration, 1, 0));
        StartCoroutine(StartFade(fadeImage, fadeDuration, 0, fadeInWaitTime));
    }

    IEnumerator StartFade(Image fadeImg, float duration, float targetOpacity, float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        float currentTime = 0;
        float start = fadeImg.color.a;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(start, targetOpacity, currentTime / duration);
            fadeImg.color = new Color(0f, 0f, 0f, newAlpha);

            yield return null;
        }

        //
        //StartCoroutine(StartFade(fadeImage, fadeDuration, 0));

    }
}
