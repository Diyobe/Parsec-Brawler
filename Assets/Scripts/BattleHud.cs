using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleHud : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textLive;
    [SerializeField]
    Shake faceShake;

    int currentLive;

    public void ShakeFace()
    {
        faceShake.ShakeRectEffect();
    }

    public void DrawLives(int livesNumber)
    {
        currentLive = livesNumber;
        textLive.text = livesNumber.ToString();

    }

    public void DrawLivesFeedback(int livesNumber)
    {
        if(currentLive != livesNumber)
            ShakeFace();
        if (livesNumber == 0)
            GetComponent<Animator>().SetTrigger("Dead");
        DrawLives(livesNumber);
        
    }

}
