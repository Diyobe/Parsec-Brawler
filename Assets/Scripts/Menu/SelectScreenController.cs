using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Rewired;
using VoiceActing;

public class SelectScreenController : InputControllable
{
    public AudioClip menuMoveSound;
    public AudioClip menuValidateSound;
    public AudioClip startFightSound;


    [SerializeField]
    PlayerData playerData;


    [SerializeField]
    Image[] imageCharacterFace;
    [SerializeField]
    TextMeshProUGUI[] textPlayerID;

    [SerializeField]
    TextMeshProUGUI textBattleStart;
    [SerializeField]
    Animator animatorStart;

    [SerializeField]
    GameObject previousScreen;

    [SerializeField]
    string stageToLoad;

    bool active = true;
    private Player player;

    public void SetStageToLoad(string sceneName)
    {
        stageToLoad = sceneName;
    }

    private void Start()
    {
        playerData.CharacterInfos.Clear();
        DrawPlayers();
        player = ReInput.players.GetPlayer(0);
    }


    public override void UpdateBuffer(List<input> inputBuffer, int inputID)
    {

    }



    public void DrawPlayers()
    {
        for (int i = 0; i < playerData.CharacterInfos.Count; i++)
        {
            imageCharacterFace[i].gameObject.SetActive(true);
            textPlayerID[i].text = (playerData.CharacterInfos[i].PlayerID + 1) + "P";
        }
        for (int i = playerData.CharacterInfos.Count; i < imageCharacterFace.Length; i++)
        {
            imageCharacterFace[i].gameObject.SetActive(false);
            textPlayerID[i].text = "";
        }
        CheckBattle();
    }


    public void CheckBattle()
    {
        textBattleStart.gameObject.SetActive((playerData.CharacterInfos.Count >= 2));
    }

    public void StartBattle()
    {
        if (playerData.CharacterInfos.Count >= 2)
        {
            TengenToppaAudioManager.Instance.PlaySound(startFightSound, 0.5f);
            TengenToppaAudioManager.Instance.StopMusic(2f);
            active = false;
            animatorStart.gameObject.SetActive(true);
            StartCoroutine(StartBattleCoroutine());
        }
    }
    private IEnumerator StartBattleCoroutine()
    {
        yield return new WaitForSeconds(2f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(stageToLoad);
    }
}
