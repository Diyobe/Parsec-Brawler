﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;

public class SelectStageController : InputControllable
{
    [SerializeField]
    string[] sceneNames;

    [SerializeField]
    RectTransform[] rectTransforms;

    [SerializeField]
    RectTransform selection;

    [SerializeField]
    SelectScreenController selectScreenController;

    int index = 0;
    bool inputDown = false;

    public AudioClip menuMoveSound;
    public AudioClip menuValidateSound;
    public AudioClip menuTheme;
    public AudioClip menuIntro;

    private void Start()
    {
        TengenToppaAudioManager.Instance.PlayMusic(menuIntro, menuTheme);
    }


    public override void UpdateBuffer(List<input> inputBuffer, int inputID)
    {
        if (inputID != 0)
            return;

        if (inputBuffer[0].vertical < -0.5f && inputDown == false)
        {
            TengenToppaAudioManager.Instance.PlaySound(menuMoveSound, 0.5f);
            inputDown = true;
            index += 1;
            if (index >= sceneNames.Length)
                index = 0;
            selection.anchoredPosition = rectTransforms[index].anchoredPosition;
        }
        else if (inputBuffer[0].vertical > 0.5f && inputDown == false)
        {
            TengenToppaAudioManager.Instance.PlaySound(menuMoveSound, 0.5f);
            inputDown = true;
            index -= 1;
            if (index < 0)
                index = sceneNames.Length-1;
            selection.anchoredPosition = rectTransforms[index].anchoredPosition;
        }
        else if (Mathf.Abs(inputBuffer[0].vertical) < 0.5f)
        {
            inputDown = false;
        }


        if (inputBuffer[0].jump)
        {
            TengenToppaAudioManager.Instance.PlaySound(menuValidateSound, 0.5f);
            selectScreenController.gameObject.SetActive(true);
            selectScreenController.SetStageToLoad(sceneNames[index]);
            this.gameObject.SetActive(false);
        }
        else if (inputBuffer[0].dash)
        {
            
        }
    }
}
