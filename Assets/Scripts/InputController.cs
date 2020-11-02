using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
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

    public List<input> inputBuffer;

    public int bufferLength = 6;

    private void Start()
    {
        inputBuffer = new List<input>(bufferLength);
    }

    private void Update()
    {
        for (int i = bufferLength; i < 0; --i)
        {
            if (i == inputBuffer.Count)
            {
                inputBuffer.RemoveAt(i);
            }
            else if (i != inputBuffer.Count && i > 0)
            {
                inputBuffer[i] = inputBuffer[i - 1];
            }
            else
            {
                inputBuffer[i] = new input();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            inputBuffer[0].jump = true;
        }
    }
}
