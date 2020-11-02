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
    float speed = 5;

    [SerializeField]
    CharacterCollision characterCollision;

    [SerializeField]
    float jumpImpulsion;

    [SerializeField]
    int numberOfJumps = 2;

    int currentNumberOfJumps;

    private void Start()
    {
        characterCollision.doAction += ResetJump;
    }

    private void ResetJump()
    {
        currentNumberOfJumps = numberOfJumps;
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
        characterCollision.Move(buffer[0].horizontal * speed);
    }
}
