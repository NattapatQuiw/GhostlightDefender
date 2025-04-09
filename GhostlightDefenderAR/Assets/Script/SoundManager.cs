using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public enum SoundCategory { Background, SFX }

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource backgroundSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip clickSound;
    public AudioClip defaultBGM;

    [Header("Game Result Sounds")]
    public AudioClip winSound;
    public AudioClip loseSound;

    private float fadeDuration = 1f;
    private AudioSource oneShotSource;

    private Dictionary<SoundCategory, float> volumeLevels = new()
    {
        { SoundCategory.Background, 1f },
        { SoundCategory.SFX, 1f }
    };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadVolumeSettings();

            oneShotSource = gameObject.AddComponent<AudioSource>();
            oneShotSource.playOnAwake = false;
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        PlayBGM(defaultBGM);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeAndPlayNewBGM(defaultBGM));
    }

    public void PlayClickSound()
    {
        if (clickSound != null)
            sfxSource.PlayOneShot(clickSound, volumeLevels[SoundCategory.SFX]);
    }

    public void PlayWinSound()
    {
        if (winSound != null)
            oneShotSource.PlayOneShot(winSound, volumeLevels[SoundCategory.SFX]);
    }

    public void PlayLoseSound()
    {
        if (loseSound != null)
            oneShotSource.PlayOneShot(loseSound, volumeLevels[SoundCategory.SFX]);
    }

    public void PlayBGM(AudioClip clip)
    {
        backgroundSource.clip = clip;
        backgroundSource.loop = true;
        backgroundSource.Play();
    }

    private IEnumerator FadeAndPlayNewBGM(AudioClip newClip)
    {
        float currentVolume = backgroundSource.volume;
        // Fade out current BGM
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            backgroundSource.volume = Mathf.Lerp(currentVolume, 0, t / fadeDuration);
            yield return null;
        }

        backgroundSource.Stop();
        backgroundSource.clip = newClip;
        backgroundSource.Play();

        // Fade in new BGM
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            backgroundSource.volume = Mathf.Lerp(0, volumeLevels[SoundCategory.Background], t / fadeDuration);
            yield return null;
        }

        backgroundSource.volume = volumeLevels[SoundCategory.Background];
    }

    public void SetVolume(SoundCategory category, float value)
    {
        value = Mathf.Clamp01(value);
        volumeLevels[category] = value;

        if (category == SoundCategory.Background)
            backgroundSource.volume = value;
        else if (category == SoundCategory.SFX)
            sfxSource.volume = value;

        PlayerPrefs.SetFloat(category.ToString(), value);
    }

    public float GetVolume(SoundCategory category)
    {
        return volumeLevels[category];
    }

    private void LoadVolumeSettings()
    {
        foreach (SoundCategory cat in Enum.GetValues(typeof(SoundCategory)))
        {
            float val = PlayerPrefs.GetFloat(cat.ToString(), 1f);
            volumeLevels[cat] = val;

            if (cat == SoundCategory.Background && backgroundSource != null)
                backgroundSource.volume = val;
            if (cat == SoundCategory.SFX && sfxSource != null)
                sfxSource.volume = val;
        }
    }

    public void ToggleMute(SoundCategory category, bool isMute)
    {
        SetVolume(category, isMute ? 0f : 1f);
    }
}
