using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CharacterState
{
    Idle,
    Acting,
    Hit,
    Dead
}



public class PlayerController : MonoBehaviour
{


    [SerializeField]
    CharacterCollision characterCollision;

    [Header("Movement")]
    [SerializeField]
    float speedMax = 5;
    [SerializeField]
    float acceleration = 5;
    [SerializeField]
    float decceleration = 5;

    [Space]
    [SerializeField]
    float airFriction = 5;
    [SerializeField]
    float airStop = 0.9f;

    [Header("Jump")]
    [SerializeField]
    float jumpImpulsion;
    [SerializeField]
    int numberOfJumps = 2;

    [Header("Debug")]
    public int direction;
    public float currentSpeed;
    int currentNumberOfJumps;



    private void Start()
    {
        characterCollision.doAction += ResetJump;
    }

    private void ResetJump()
    {
        currentNumberOfJumps = numberOfJumps;
        direction = (int)Mathf.Sign(currentSpeed);
    }

    public void UpdateBuffer(List<input> buffer)
    {
        CheckJump(buffer);
        CheckHorizontal(buffer);
    }

    void CheckJump(List<input> buffer)
    {
        for (int i = 0; i < buffer.Count; i++)
        {
            if (buffer[i].jump && currentNumberOfJumps > 0)
            {
                buffer[i].jump = false;
                --currentNumberOfJumps;
                characterCollision.Jump(jumpImpulsion);
            }
        }
    }

    void CheckHorizontal(List<input> buffer)
    {
        if(characterCollision.IsGrounded == true)
        {
            if (buffer[0].horizontal != 0)
            {
                currentSpeed += buffer[0].horizontal * acceleration;
                direction = (int) Mathf.Sign(currentSpeed);
            }
            else
            {
                currentSpeed -= (decceleration * direction);
                if (currentSpeed <= decceleration && currentSpeed >= -decceleration)
                    currentSpeed = 0;
            }
        }
        else
        {
            if (buffer[0].horizontal != 0)
            {
                currentSpeed += buffer[0].horizontal * (acceleration - airFriction);
            }
            else
            {
                currentSpeed *= airStop;
                if (currentSpeed <= airFriction && currentSpeed >= -airFriction)
                    currentSpeed = 0;
            }
        }
        currentSpeed = Mathf.Clamp(currentSpeed, -speedMax, speedMax);
        characterCollision.Move(currentSpeed);

    }
}
