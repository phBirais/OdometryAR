using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AudioController : MonoBehaviour
{
     #region VARIABLES    
    public float maxMusicVolume = .6f;

    public Button   soundButton;
    public Sprite[] btnImage;

    AudioSource  source;

    bool soundStatus = true;
    #endregion

    void Awake ()
    {
        source = GetComponent<AudioSource>();
    }

    void Start ()
    {
        //------------------recuperando preferencias de audio---------------------------------      
        if (PlayerPrefs.HasKey("AudioStatus"))
        {
            soundStatus = (PlayerPrefs.GetInt("AudioStatus") != 0);
        }
        else
        {
            PlayerPrefs.SetInt("AudioStatus", 1);
            PlayerPrefs.Save();
        }
        ChangeAudioStatus(soundStatus);
        StartCoroutine(StartFade(source, 1f, maxMusicVolume));
    }

    public void Mute ()
    {
        soundStatus = !soundStatus;
        ChangeAudioStatus(soundStatus);

        PlayerPrefs.SetInt("AudioStatus", soundStatus ? 1 : 0);
        PlayerPrefs.Save();
    }

    void ChangeAudioStatus (bool status)
    {
        float volume = status ? 1 : 0;

        AudioListener.volume     = volume;
        soundButton.image.sprite = btnImage[status ? 0 : 1];
    }

    public void FadeInMusic (AudioSource audioSource)
    {
        StartCoroutine(StartFade(source, 1f, 0f));
        StartCoroutine(StartFade(audioSource, 1f, maxMusicVolume));
    }

    public void FadeOutMusic (AudioSource audioSource)
    {
        StartCoroutine(StartFade(audioSource, 1f, 0f));
        StartCoroutine(StartFade(source, 1f, maxMusicVolume));
    }

    public void FadeBackgroundMusic ()
    {
        StartCoroutine(StartFade(source, 1f, 0f));
    }

    IEnumerator StartFade (AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start       = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime       += Time.fixedDeltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }

    void OnApplicationQuit ()
    {
        PlayerPrefs.SetInt("AudioStatus", soundStatus ? 1 : 0);
        PlayerPrefs.Save();
    }

    void OnApplicationPause (bool pauseStatus)
    {
        if (pauseStatus)
        {
            PlayerPrefs.SetInt("AudioStatus", soundStatus ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    void OnApplicationFocus (bool hasFocus)
    {
        if (!hasFocus)
        {
            PlayerPrefs.SetInt("AudioStatus", soundStatus ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}