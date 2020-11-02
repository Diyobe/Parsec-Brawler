using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class input
{
    public bool jump;
    public bool hit;
    public bool dash;

    public float horizontal;
    public float vertical;

    public input()
    {
        jump = false;
        hit = false;
        dash = false;

        horizontal = 0;
        vertical = 0;
    }
}


public class InputController : MonoBehaviour
{

    public List<input> inputBuffer;
    public int bufferLength = 6;

    public PlayerController playerController;

    private void Start()
    {
        inputBuffer = new List<input>(bufferLength);
        for (int i = 0; i < bufferLength; i++)
        {
            inputBuffer.Add(new input());
        }
    }

    private void Update()
    {
        for (int i = bufferLength - 1; i > 0; --i)
        {
            inputBuffer[i] = inputBuffer[i - 1];
        }
        inputBuffer[0] = new input();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            inputBuffer[0].jump = true;

        }

        playerController.UpdateBuffer(inputBuffer);
    }
}
