﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

[System.Serializable]
public class input
{
    public bool jump;
    public bool hit;
    public bool special;
    public bool dash;
    public bool taunt;

    public float horizontal;
    public float vertical;

    public input()
    {
        jump = false;
        special = false;
        hit = false;
        dash = false;
        taunt = false;

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

    public InputControllable playerController;
    protected Player player;


    public void SetPlayerID(int newID)
    {
        playerID = newID;
    }
    public void SetPlayerController(InputControllable controller)
    {
        playerController = controller;
    }

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
        InputSpecial();
        InputTaunt();
        InputHorizontal();
        InputVertical();
        InputStart();

        playerController.UpdateBuffer(inputBuffer, playerID);
    }

    private void InputTaunt()
    {
        if (player.GetButtonDown("Taunt"))
        {
            inputBuffer[0].taunt = true;
        }
    }

    private void InputSpecial()
    {
        if (player.GetButtonDown("Special"))
        {
            inputBuffer[0].special = true;
        }
    }

    public bool InputStart()
    {
        return player.GetButtonDown("Start");
    }

    private void InputHorizontal()
    {
        if (Mathf.Abs(player.GetAxis("Horizontal")) > .35)
        {
            inputBuffer[0].horizontal = player.GetAxis("Horizontal");
        }
        else
        {
            inputBuffer[0].horizontal = 0;
        }
    }

    private void InputVertical()
    {
        if (Mathf.Abs(player.GetAxis("Vertical")) > .35)
        {
            inputBuffer[0].vertical = player.GetAxis("Vertical");
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
    }

    void InputHit()
    {
        if (player.GetButtonDown("Action"))
        {
            inputBuffer[0].hit = true;
        }
    }

    void InputDash()
    {
        if (player.GetButtonDown("Dash"))
        {
            inputBuffer[0].dash = true;

        }
    }
}
