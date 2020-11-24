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

    [SerializeField]
    CharacterData[] characterDatas;

    [SerializeField]
    Image[] characterFaces;

    [SerializeField]
    Image[] characterPortraits;

    [SerializeField]
    RectTransform[] characterPositions;

    [SerializeField]
    RectTransform[] cursors;

    bool[] joystickPushed = new bool[4];

    bool[] jumpPressed = new bool[4];

    bool[] actionPressed = new bool[4];

    bool[] isPlayersReady = new bool[4];

    bool[] isPlayerAddedInData = new bool[4];

    int[] inputSelections = new int[4];

    public int numberOfReadyPlayers = 0;

    bool active = true;
    private Player player;

    public void SetStageToLoad(string sceneName)
    {
        stageToLoad = sceneName;
    }

    private void Start()
    {
        playerData.CharacterInfos.Clear();
        numberOfReadyPlayers = 0;
        DrawPlayers();
        player = ReInput.players.GetPlayer(0);

        for (int i = 0; i < characterDatas.Length; i++)
        {
            characterFaces[i].sprite = characterDatas[i].Face;
        }

        for (int i = 0; i < characterPortraits.Length; i++)
        {
            characterPortraits[i].gameObject.SetActive(false);
            cursors[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < textPlayerID.Length; i++)
        {
            textPlayerID[i].gameObject.SetActive(false);
        }
    }


    public override void UpdateBuffer(List<input> inputBuffer, int inputID)
    {
        if (numberOfReadyPlayers <= 0)
            numberOfReadyPlayers = 0;
        for (int i = 0; i < 4; i++)
        {
            //if(ReInput.players.GetPlayer(i).GetButtonDown("Jump") && jumpPressed[i] == false)
            //{
            //    jumpPressed[i] = true;
            //}

            //if (ReInput.players.GetPlayer(i).GetButtonDown("Action"))
            //{
            //    actionPressed[i] = true;
            //}
            //if (ReInput.players.GetPlayer(i).GetButtonUp("Action"))
            //{
            //    actionPressed[i] = false;
            //}

            if (ReInput.players.GetPlayer(i).GetButtonDown("Jump") && !cursors[i].gameObject.activeSelf && jumpPressed[i] == false)
            {
                isPlayersReady[i] = false;
                jumpPressed[i] = true;
                cursors[i].gameObject.SetActive(true);
                cursors[i].position = characterPositions[0].position;
                textPlayerID[i].gameObject.SetActive(true);
                characterPortraits[i].gameObject.SetActive(true);
                characterPortraits[i].sprite = characterDatas[0].CharacterSelectionSprite;
                characterPortraits[i].color = new Color(1f, 1f, 1f, 0.4f);
            }
            else if (ReInput.players.GetPlayer(i).GetButtonUp("Jump") && jumpPressed[i] == true)
            {
                jumpPressed[i] = false;
            }

            if (ReInput.players.GetPlayer(i).GetButtonDown("Action") && cursors[i].gameObject.activeSelf && actionPressed[i] == false)
            {
                isPlayersReady[i] = false;
                actionPressed[i] = true;

                if (numberOfReadyPlayers > 0)
                    numberOfReadyPlayers--;

                cursors[i].gameObject.SetActive(false);
                textPlayerID[i].gameObject.SetActive(false);
                characterPortraits[i].gameObject.SetActive(false);
            }
            else if (ReInput.players.GetPlayer(i).GetButtonUp("Action") && actionPressed[i] == true)
            {
                actionPressed[i] = false;
            }

            //---------------------------------------//

            if (ReInput.players.GetPlayer(i).GetAxis("Horizontal") > 0.5f && cursors[i].gameObject.activeSelf && joystickPushed[i] == false && !isPlayersReady[i])
            {
                joystickPushed[i] = true;
                if (characterPortraits[i].sprite == characterDatas[0].CharacterSelectionSprite)
                {
                    characterPortraits[i].sprite = characterDatas[1].CharacterSelectionSprite;
                    cursors[i].position = characterPositions[1].position;
                }
                else if (characterPortraits[i].sprite == characterDatas[1].CharacterSelectionSprite)
                {
                    characterPortraits[i].sprite = characterDatas[2].CharacterSelectionSprite;
                    cursors[i].position = characterPositions[2].position;
                }
                else
                {
                    characterPortraits[i].sprite = characterDatas[0].CharacterSelectionSprite;
                    cursors[i].position = characterPositions[0].position;
                }
            }
            else if (ReInput.players.GetPlayer(i).GetAxis("Horizontal") < -0.5f && cursors[i].gameObject.activeSelf && joystickPushed[i] == false && !isPlayersReady[i])
            {
                joystickPushed[i] = true;
                if (characterPortraits[i].sprite == characterDatas[0].CharacterSelectionSprite)
                {
                    characterPortraits[i].sprite = characterDatas[2].CharacterSelectionSprite;
                    cursors[i].position = characterPositions[2].position;
                }
                else if (characterPortraits[i].sprite == characterDatas[1].CharacterSelectionSprite)
                {
                    characterPortraits[i].sprite = characterDatas[0].CharacterSelectionSprite;
                    cursors[i].position = characterPositions[0].position;
                }
                else
                {
                    characterPortraits[i].sprite = characterDatas[1].CharacterSelectionSprite;
                    cursors[i].position = characterPositions[1].position;
                }

            }
            else if (Mathf.Abs(ReInput.players.GetPlayer(i).GetAxis("Horizontal")) < 0.5f && cursors[i].gameObject.activeSelf)
            {
                joystickPushed[i] = false;
            }

            //---------------------------------------//

            if (ReInput.players.GetPlayer(i).GetButtonDown("Jump") && cursors[i].gameObject.activeSelf && !isPlayersReady[i] && jumpPressed[i] == false)
            {
                jumpPressed[i] = true;
                isPlayersReady[i] = true;
                if (numberOfReadyPlayers < 4)
                    ++numberOfReadyPlayers;
                if (characterPortraits[i].sprite == characterDatas[0].CharacterSelectionSprite)
                {
                    inputSelections[i] = 0;
                }
                else if (characterPortraits[i].sprite == characterDatas[1].CharacterSelectionSprite)
                {
                    inputSelections[i] = 1;
                }
                else
                {
                    inputSelections[i] = 2;
                }
                characterPortraits[i].color = new Color(1f, 1f, 1f, 1f);
            }
            else if (ReInput.players.GetPlayer(i).GetButtonUp("Jump") && jumpPressed[i] == true)
            {
                jumpPressed[i] = false;
            }

            if (ReInput.players.GetPlayer(i).GetButtonDown("Action") && cursors[i].gameObject.activeSelf && isPlayersReady[i] && actionPressed[i] == false)
            {
                isPlayersReady[i] = false;
                --numberOfReadyPlayers;
                actionPressed[i] = true;

                characterPortraits[i].color = new Color(1f, 1f, 1f, 0.4f);
            }
            else if (ReInput.players.GetPlayer(i).GetButtonUp("Action") && actionPressed[i] == true)
            {
                actionPressed[i] = false;
            }

            //---------------------------------------//

            if (ReInput.players.GetPlayer(i).GetButtonDown("Start") && numberOfReadyPlayers >= 2)
            {
                if (isPlayersReady[0] == true && isPlayerAddedInData[0] == false)
                {
                    isPlayerAddedInData[0] = true;
                    CharacterInfos charInfos = new CharacterInfos();
                    charInfos.PlayerID = 0;
                    charInfos.CharacterColorID = 0;
                    charInfos.CharacterData = characterDatas[inputSelections[0]];
                    playerData.CharacterInfos.Add(charInfos);
                }

                if (isPlayersReady[1] == true && isPlayerAddedInData[1] == false)
                {
                    isPlayerAddedInData[1] = true;
                    CharacterInfos charInfos = new CharacterInfos();
                    charInfos.PlayerID = 1;
                    charInfos.CharacterColorID = 1;
                    charInfos.CharacterData = characterDatas[inputSelections[1]];
                    playerData.CharacterInfos.Add(charInfos);
                }

                if (isPlayersReady[2] == true && isPlayerAddedInData[2] == false)
                {
                    isPlayerAddedInData[2] = true;
                    CharacterInfos charInfos = new CharacterInfos();
                    charInfos.PlayerID = 2;
                    charInfos.CharacterColorID = 2;
                    charInfos.CharacterData = characterDatas[inputSelections[2]];
                    playerData.CharacterInfos.Add(charInfos);
                }

                if (isPlayersReady[3] == true && isPlayerAddedInData[3] == false)
                {
                    isPlayerAddedInData[3] = true;
                    CharacterInfos charInfos = new CharacterInfos();
                    charInfos.PlayerID = 3;
                    charInfos.CharacterColorID = 3;
                    charInfos.CharacterData = characterDatas[inputSelections[3]];
                    playerData.CharacterInfos.Add(charInfos);
                }
                StartBattle();
            }
        }

        //if(player.GetAxis("Horizontal"))
        CheckBattle();
    }



    public void DrawPlayers()
    {
        //for (int i = 0; i < playerData.CharacterInfos.Count; i++)
        //{
        //    imageCharacterFace[i].gameObject.SetActive(true);
        //    textPlayerID[i].text = (playerData.CharacterInfos[i].PlayerID + 1) + "P";
        //}
        //for (int i = playerData.CharacterInfos.Count; i < imageCharacterFace.Length; i++)
        //{
        //    imageCharacterFace[i].gameObject.SetActive(false);
        //    textPlayerID[i].text = "";
        //}
        CheckBattle();
    }


    public void CheckBattle()
    {
        //textBattleStart.gameObject.SetActive((playerData.CharacterInfos.Count >= 2));
        textBattleStart.gameObject.SetActive((numberOfReadyPlayers >= 2));
    }

    public void StartBattle()
    {
        //if (playerData.CharacterInfos.Count >= 2)
        if (numberOfReadyPlayers >= 2 && active)
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
