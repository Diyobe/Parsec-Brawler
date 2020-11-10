using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textLive;
    [SerializeField]
    Shake faceShake;

    Image faceImage;
    int currentLive;

    public void DrawFace(Sprite face)
    {
        faceImage = faceShake.GetComponent<Image>();
        faceImage.sprite = face;
    }

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
