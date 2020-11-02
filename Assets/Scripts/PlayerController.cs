using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public void UpdateBuffer(List<input> buffer)
    {

    }

    void CheckJump(List<input> buffer)
    {
        foreach(input input in buffer)
        {
            if (input.jump)
            {
                buffer.Remove(input);
                Jump();
            }
        }
    }

    private void Jump()
    {

    }
}
