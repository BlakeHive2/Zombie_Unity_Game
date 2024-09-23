using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class FadeAudioSource
{
    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}

public class MusicManager : MonoBehaviour
{
    public float fadeMusicTime = 2;
    public bool DoFade = false;
    
    public GameObject fadeOutSection;
    public GameObject fadeInSection;

    AudioSource[] fadeInSrc;
    AudioSource[] fadeOutSrc;
    

    void Start()
    {
        
    }

    public void FadeFromTo(GameObject fromObj, GameObject toObj)
    {
        fadeOutSection = fromObj;
        fadeInSection = toObj;

        fadeInSrc = fadeInSection.GetComponentsInChildren<AudioSource>();
        fadeOutSrc = fadeOutSection.GetComponentsInChildren<AudioSource>();

        DoFade = true;
    }
    void Update()
    {
        if (DoFade == true)
        {
            
            StartCoroutine(ChangeFadeClips());
            gameObject.GetComponent<CameraFadeManager>().doVisualFade();
            DoFade = false;
        }
    }

    //TODO: fade out current, swap clips, then fade in new
    IEnumerator ChangeFadeClips()
    {
        
        //Fade Out
        foreach (AudioSource src in fadeOutSrc)
        {
            StartCoroutine(FadeAudioSource.StartFade(src, fadeMusicTime, 0));
        }

        //Fade In
        yield return new WaitForSeconds(fadeMusicTime);
                
        fadeInSection.SetActive(true);

        foreach (AudioSource src in fadeInSrc)
        {
            src.volume = 0;
        }

        yield return new WaitForSeconds(fadeMusicTime + 0.2f);

        foreach (AudioSource src in fadeInSrc)
        {
            StartCoroutine(FadeAudioSource.StartFade(src, fadeMusicTime, 1));
        }

        fadeOutSection.SetActive(false);
    }

}

