using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinMenu : InputControllable
{
    [Header("Data")]
    [SerializeField]
    string[] playerNames;
    [SerializeField]
    GameObject[] winnerColor;
    [SerializeField]
    Sprite[] playerHead;

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
    List<int> playerID;

    public void CreateMenu(List<int> id)
    {
        playerID = id;
        this.gameObject.SetActive(true);
        playerID.Reverse();
        textWinner.text = playerNames[playerID[0]] + " WIN";
        winnerColor[playerID[0]].gameObject.SetActive(true);

        for (int i = 1; i < playerID.Count; i++)
        {
            playerPositions[i - 1].gameObject.SetActive(true);
            textPositions[i - 1].text = (i+1).ToString();
            facePositions[i - 1].sprite = playerHead[playerID[i]];
        }
        StartCoroutine(WaitCoroutine());
    }

    private IEnumerator WaitCoroutine()
    {
        yield return new WaitForSecondsRealtime(1f);
        active = true;
    }

    public override void UpdateBuffer(List<input> inputBuffer, int inputID)
    {
        if (active == false)
            return;
        if(inputID == playerID[0] && inputBuffer[0].jump)
        {
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
