using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;

public class MainMenuController : InputControllable
{

    [SerializeField]
    Animator animatorIntro;
    [SerializeField]
    Animator animatorAppear;
    [SerializeField]
    Animator animatorTransition;

    [SerializeField]
    Animator[] animatorOptions;


    public AudioClip menuMoveSound;
    public AudioClip menuValidateSound;
    public AudioClip menuTheme;
    public AudioClip menuIntro;

    int index = 0;
    bool inputDown = false;
    bool active = false;
    bool trueInactive = false;


    private void Start()
    {
        TengenToppaAudioManager.Instance.PlayMusic(menuIntro, menuTheme);
        StartCoroutine(IntroCoroutine());
    }
    private IEnumerator IntroCoroutine()
    {
        yield return new WaitForSeconds(7f);
        animatorAppear.gameObject.SetActive(true);
        active = true;
        TengenToppaAudioManager.Instance.PlaySound(menuValidateSound, 1f);
        animatorOptions[index].SetBool("Selected", true);
    }



    public override void UpdateBuffer(List<input> inputBuffer, int inputID)
    {
        if (trueInactive == true)
            return;
        if(active == false)
        {
            if (inputBuffer[0].jump)
            {
                //SkipIntro
                animatorIntro.SetTrigger("Skip");
                animatorAppear.gameObject.SetActive(true);
                StopAllCoroutines();
                active = true;
                TengenToppaAudioManager.Instance.PlaySound(menuValidateSound, 1f);
                animatorOptions[index].SetBool("Selected", true);
            }
            return;
        }



        if (inputID != 0)
            return;

        if (inputBuffer[0].vertical < -0.5f && inputDown == false)
        {
            TengenToppaAudioManager.Instance.PlaySound(menuMoveSound, 0.5f);
            inputDown = true;
            animatorOptions[index].SetBool("Selected", false);
            index += 1;
            if (index >= animatorOptions.Length)
                index = 0;
            animatorOptions[index].SetBool("Selected", true);
        }
        else if (inputBuffer[0].vertical > 0.5f && inputDown == false)
        {
            TengenToppaAudioManager.Instance.PlaySound(menuMoveSound, 0.5f);
            inputDown = true;
            animatorOptions[index].SetBool("Selected", false);
            index -= 1;
            if (index < 0)
                index = animatorOptions.Length - 1;
            animatorOptions[index].SetBool("Selected", true);
        }
        else if (Mathf.Abs(inputBuffer[0].vertical) < 0.5f)
        {
            inputDown = false;
        }


        if (inputBuffer[0].jump)
        {
            TengenToppaAudioManager.Instance.PlaySound(menuValidateSound, 0.5f);
            Confirm();
        }
    }


    private void Confirm()
    {
        switch(index)
        {
            case 0:
                animatorTransition.gameObject.SetActive(true);
                StartCoroutine(StartTransition());
                break;
            case 1:
                Application.Quit();
                break;
        }
    }

    private IEnumerator StartTransition()
    {
        trueInactive = true;
        yield return new WaitForSeconds(1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
