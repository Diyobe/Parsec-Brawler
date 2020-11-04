using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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



    private void Start()
    {
        DrawPlayers();
    }


    public override void UpdateBuffer(List<input> inputBuffer, int inputID = 0)
    {
        if(inputBuffer[0].jump == true)
        {
            playerData.PlayerID.Add(inputID);
            DrawPlayers();
        }

        if (inputBuffer[0].hit == true)
        {
            playerData.PlayerID.Remove(inputID);
            DrawPlayers();
        }
    }



    public void DrawPlayers()
    {
        for (int i = 0; i < playerData.PlayerID.Count; i++)
        {
            imageCharacterFace[i].gameObject.SetActive(true);
            textPlayerID[i].text = playerData.PlayerID[i].ToString();
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
            // Battle
        }
    }
}
