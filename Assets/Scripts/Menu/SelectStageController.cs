using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;
using TMPro;

public class SelectStageController : InputControllable
{
    [SerializeField]
    PlayerData playerData;
    [SerializeField]
    TextMeshProUGUI textLife;
    [SerializeField]
    TextMeshProUGUI textMode;

    [SerializeField]
    string[] sceneNames;

    [SerializeField]
    RectTransform[] rectTransforms;

    [SerializeField]
    RectTransform slider;
    [SerializeField]
    float stageElementHeight;

    [SerializeField]
    RectTransform selection;
    Animator animatorSelection;

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
        animatorSelection = selection.GetComponent<Animator>();
        TengenToppaAudioManager.Instance.PlayMusic(menuIntro, menuTheme);
        textLife.text = playerData.NumberOfLives.ToString();
        textMode.text = playerData.GameMode.ToString();
        AdjusteSlider();
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
            animatorSelection.SetTrigger("Feedback");
            AdjusteSlider();
        }
        else if (inputBuffer[0].vertical > 0.5f && inputDown == false)
        {
            TengenToppaAudioManager.Instance.PlaySound(menuMoveSound, 0.5f);
            inputDown = true;
            index -= 1;
            if (index < 0)
                index = sceneNames.Length-1;
            selection.anchoredPosition = rectTransforms[index].anchoredPosition;
            animatorSelection.SetTrigger("Feedback");
            AdjusteSlider();
        }
        else if (Mathf.Abs(inputBuffer[0].vertical) < 0.5f)
        {
            inputDown = false;
        }


        if (inputBuffer[0].jump)
        {
            TengenToppaAudioManager.Instance.PlaySound(menuValidateSound, 0.5f);
            selectScreenController.gameObject.SetActive(true);
            selectScreenController.Init();
            selectScreenController.SetStageToLoad(sceneNames[index]);
            this.gameObject.SetActive(false);
        }
        else if (inputBuffer[0].hit)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        else if (inputBuffer[0].dash)
        {
            playerData.NumberOfLives += 1;
            if( playerData.NumberOfLives > 5)
            {
                playerData.NumberOfLives = 1;
            }
            textLife.text = playerData.NumberOfLives.ToString();
        }
        else if(inputBuffer[0].taunt)
        {
            if (playerData.GameMode == TypeOfGameMode.FreeForAll)
                playerData.GameMode = TypeOfGameMode.TeamVsTeam;
            else if (playerData.GameMode == TypeOfGameMode.TeamVsTeam)
                playerData.GameMode = TypeOfGameMode.FreeForAll;
            textMode.text = playerData.GameMode.ToString();
        }
    }


    private void AdjusteSlider()
    {
        StopAllCoroutines();
        StartCoroutine(SliderTweenCoroutine(0.5f, index * stageElementHeight));
    }
    private IEnumerator SliderTweenCoroutine(float duration, float amount)
    {
        float t = 0f;
        float speed = 1f / duration;
        float y = slider.anchoredPosition.y;
        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            y = Mathf.Lerp(y, amount, t);
            slider.anchoredPosition = new Vector2(0, y);
            yield return null;
        }
    }

}
