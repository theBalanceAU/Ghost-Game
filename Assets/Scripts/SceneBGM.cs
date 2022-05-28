using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBGM : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] float fadeInDuration = 2f;
    [SerializeField] float fadeOutDuration = 2f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start() {
        audioSource.volume = 0f;
        FadeIn();
    }

    public void FadeIn()
    {
        StartCoroutine(StartFade(fadeInDuration, 1f));
    }

    public void FadeOut()
    {
        StartCoroutine(StartFade(fadeOutDuration, 0f));
    }

    IEnumerator StartFade(float duration, float targetVolume)
    {
        float currentTime = 0;
        float startVolume = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}
