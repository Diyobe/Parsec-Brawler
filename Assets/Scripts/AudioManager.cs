using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource battleTheme;
    [SerializeField] private AudioSource menuMusic;
    [SerializeField] private AudioSource introMenu;

    public static AudioManager Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(this);
        }
    }

    public void PlayBattleTheme()
    {
        battleTheme.Play();
    }

    public void StopBattleTheme()
    {
        battleTheme.Stop();
    }

    public void PlayMenuMusic()
    {
        introMenu.Play();
        StartCoroutine(LaunchMenuMusic());
    }

    IEnumerator LaunchMenuMusic()
    {
        yield return new WaitForSeconds(introMenu.clip.length);
        menuMusic.Play();
    }

    public void StopMenuMusic()
    {
        menuMusic.Stop();
    }

    public void PlayIntroMenu()
    {
        introMenu.Play();
    }

    public void StopIntroMenu()
    {
        introMenu.Stop();
    }
}
