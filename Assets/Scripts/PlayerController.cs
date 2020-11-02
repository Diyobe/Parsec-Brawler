using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    CharacterCollision characterCollision;

    [SerializeField]
    float jumpImpulsion;

    public void UpdateBuffer(List<input> buffer)
    {
        CheckJump(buffer);
    }

    void CheckJump(List<input> buffer)
    {
        for (int i = 0; i < buffer.Count; i++)
        {
            if (buffer[i].jump && characterCollision.IsGrounded)
            {
                buffer[i].jump = false;
                characterCollision.Jump(jumpImpulsion);
            }
        }
    }

}
