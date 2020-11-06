using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Rewired;

public class SelectScreenController: InputControllable
{
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

    public void SetStageToLoad(string sceneName)
    {
        stageToLoad = sceneName;
    }

    private void Start()
    {
        playerData.PlayerID.Clear();
        DrawPlayers();
    }


    public override void UpdateBuffer(List<input> inputBuffer, int inputID)
    {
        if (active == false)
            return;

        if(inputBuffer[0].jump == true)
        {
            if (playerData.PlayerID.Contains(inputID) == false)
            {
                playerData.PlayerID.Add(inputID);
                DrawPlayers();
            }
            else
            {
                StartBattle();
            }
        }

        if (inputBuffer[0].hit == true)
        {
            if (playerData.PlayerID.Contains(inputID) == true)
            {
                playerData.PlayerID.Remove(inputID);
                DrawPlayers();
            }
            else if (inputID == 0)
            {
                this.gameObject.SetActive(false);
                previousScreen.SetActive(true);
                //StartBattle();
            }
        }

        /*if (inputBuffer[0].dash == true)
        {
            StartBattle();
        }*/
    }



    public void DrawPlayers()
    {
        for (int i = 0; i < playerData.PlayerID.Count; i++)
        {
            imageCharacterFace[i].gameObject.SetActive(true);
            textPlayerID[i].text = (playerData.PlayerID[i]+1)+"P";
        }
        for (int i = playerData.PlayerID.Count; i < imageCharacterFace.Length; i++)
        {
            imageCharacterFace[i].gameObject.SetActive(false);
            textPlayerID[i].text = "";
        }
        CheckBattle();
    }


    public void CheckBattle()
    {
        textBattleStart.gameObject.SetActive((playerData.PlayerID.Count >= 2));
    }

    public void StartBattle()
    {
        if(playerData.PlayerID.Count >= 2)
        {
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
