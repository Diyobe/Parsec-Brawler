using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : InputControllable
{
    [SerializeField]
    RectTransform[] rectTransforms;
    [SerializeField]
    GameObject screenSelect;
    [SerializeField]
    RectTransform selection;

    int index = 0;
    bool inputDown = false;

    public override void UpdateBuffer(List<input> inputBuffer, int inputID)
    {
        if (inputID != 0)
            return;

        if (inputBuffer[0].vertical < -0.5f && inputDown == false)
        {
            inputDown = true;
            index += 1;
            if (index >= rectTransforms.Length)
                index = 0;
            selection.anchoredPosition = rectTransforms[index].anchoredPosition;
        }
        else if (inputBuffer[0].vertical > 0.5f && inputDown == false)
        {
            inputDown = true;
            index -= 1;
            if (index < 0)
                index = rectTransforms.Length - 1;
            selection.anchoredPosition = rectTransforms[index].anchoredPosition;
        }
        else if (Mathf.Abs(inputBuffer[0].vertical) < 0.5f)
        {
            inputDown = false;
        }


        if (inputBuffer[0].jump)
        {
            Confirm();
        }
    }


    private void Confirm()
    {
        switch(index)
        {
            case 0:
                screenSelect.gameObject.SetActive(true);
                this.gameObject.SetActive(false);
                break;
            case 1:
                
                break;
            case 2:
                Application.Quit();
                break;
        }
    }
}
