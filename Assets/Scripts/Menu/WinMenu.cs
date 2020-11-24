using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VoiceActing;

public class WinMenu : InputControllable
{
    [Header("Data")]
    [SerializeField]
    GameObject[] winnerColor;

    [Header("Parameters")]
    [SerializeField]
    TextMeshProUGUI textWinner;

    [SerializeField]
    GameObject[] playerPositions;
    [SerializeField]
    TextMeshProUGUI[] textPositions;
    [SerializeField]
    Image[] facePositions;

    [SerializeField]
    GameObject fade;

    bool active = false;
    public List<int> playerID;

    public AudioClip resultSong;
    public AudioClip drumValidate;

    [SerializeField]
    PlayerData playerData;

    [SerializeField]
    Image winnerCharacter;

    public void CreateMenu(List<int> id)
    {
        playerID = id;
        this.gameObject.SetActive(true);
        playerID.Reverse();
        //textWinner.text = playerNames[playerID[0]] + " WIN";
        textWinner.text = playerData.CharacterInfos[playerID[0]].CharacterData.CharName + " WIN";
        //winnerColor[playerID[0]].gameObject.SetActive(true);

        winnerCharacter.gameObject.SetActive(true);
        winnerCharacter.sprite = playerData.CharacterInfos[playerID[0]].CharacterData.CharacterSelectionSprite;

        for (int i = 1; i < playerID.Count; i++)
        {
            playerPositions[i - 1].gameObject.SetActive(true);
            textPositions[i - 1].text = (i+1).ToString();
            facePositions[i - 1].sprite = playerData.CharacterInfos[playerID[i]].CharacterData.Face;
        }
        StartCoroutine(WaitCoroutine());
    }

    private IEnumerator WaitCoroutine()
    {
        TengenToppaAudioManager.Instance.PlaySound(resultSong, 1f);
        yield return new WaitForSecondsRealtime(1f);
        active = true;
    }

    public override void UpdateBuffer(List<input> inputBuffer, int inputID)
    {
        if (active == false)
            return;
        if(inputID == playerID[0] && inputBuffer[0].jump)
        {
            TengenToppaAudioManager.Instance.PlaySound(drumValidate, 0.5f);
            active = false;
            //TengenToppaAudioManager.Instance.StopMusic(1.5f);
            fade.gameObject.SetActive(true);
            StartCoroutine(WaitCoroutine2());
        }
    }

    private IEnumerator WaitCoroutine2()
    {
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
