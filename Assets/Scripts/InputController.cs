using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

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

    [SerializeField]
    protected int playerID = 0;

    public List<input> inputBuffer;
    public int bufferLength = 6;

    public PlayerController playerController;
    protected Player player;

    private void Start()
    {
        inputBuffer = new List<input>(bufferLength);
        for (int i = 0; i < bufferLength; i++)
        {
            inputBuffer.Add(new input());
        }
        player = ReInput.players.GetPlayer(playerID);
    }

    private void Update()
    {
        for (int i = bufferLength - 1; i > 0; --i)
        {
            inputBuffer[i] = inputBuffer[i - 1];
        }
        inputBuffer[0] = new input();

        InputJump();
        InputDash();
        InputHit();
        InputHorizontal();
        InputVertical();

        playerController.UpdateBuffer(inputBuffer);
    }

    private void InputHorizontal()
    {
        if (player.GetAxis("Horizontal") != 0)
        {
            inputBuffer[0].horizontal = Mathf.Sign(player.GetAxis("Horizontal"));
        }
        /*if (Input.GetKey(KeyCode.Q))
        {
            inputBuffer[0].horizontal = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            inputBuffer[0].horizontal = 1;
        }*/
        else
        {
            inputBuffer[0].horizontal = 0;
        }
    }

    private void InputVertical()
    {
        if (Input.GetKey(KeyCode.S))
        {
            inputBuffer[0].vertical = -1;
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            inputBuffer[0].vertical = 1;
        }
        else
        {
            inputBuffer[0].vertical = 0;
        }
    }

    void InputJump()
    {
        if(player.GetButtonDown("Jump"))
        {
            inputBuffer[0].jump = true;
        }
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            inputBuffer[0].jump = true;

        }*/
    }

    void InputHit()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            inputBuffer[0].hit = true;

        }
    }

    void InputDash()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            inputBuffer[0].dash = true;

        }
    }
}
