using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tempest : MonoBehaviour
{
    [SerializeField]
    Material newSkybox;

    [SerializeField]
    AudioClip thunderFX;

    public void EventTempest()
    {
        RenderSettings.skybox = newSkybox;
    }

    public void PlayThunderFX()
    {
        VoiceActing.TengenToppaAudioManager.Instance.PlaySound(thunderFX, 0.7f);
    }
}
