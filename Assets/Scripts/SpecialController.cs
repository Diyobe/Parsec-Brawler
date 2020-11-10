using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialController : PlayerController
{
    [Header("Specials")]
    [SerializeField]
    AttackController neutralGround;
    [SerializeField]
    AttackController neutralAir;
    [SerializeField]
    AttackController upSpecial;

    [Space]
    [Header("Taunts")]
    [SerializeField]
    AttackController taunt;


    [SerializeField] float upSpecialImpulsion;
    float currentUpSpecial = 0;
    [SerializeField] float upSpecialNumber = 1;

    bool isResetSetup = false;


    private void Awake()
    {
        currentUpSpecial = upSpecialNumber;
    }


    private void ResetUpSpecial()
    {
        currentUpSpecial = upSpecialNumber;
    }

    protected override void CheckAttack(List<input> buffer)
    {
        base.CheckAttack(buffer);

        for (int i = 0; i < buffer.Count; i++)
        {
            if (buffer[i].taunt && characterCollision.IsGrounded == true)
            {
                Action(taunt);
            }

            if (buffer[i].special)
            {
                buffer[i].special = false;
                if (characterCollision.IsGrounded == true)
                {
                    if (Mathf.Abs(buffer[i].vertical) > Mathf.Abs(buffer[i].horizontal))
                    {
                        if (buffer[i].vertical > 0 )
                        {
                            //--currentUpSpecial;
                            Action(upSpecial);
                            currentSpeedY = upSpecialImpulsion;
                            afterImageEffect.StartAfterImage();
                        }
                    }
                    else
                    {
                        Action(neutralGround);
                    }
                }
                else
                {
                    if (Mathf.Abs(buffer[i].vertical) > Mathf.Abs(buffer[i].horizontal))
                    {
                        if (buffer[i].vertical > 0)
                        {
                            Action(upSpecial);
                            currentSpeedY = upSpecialImpulsion;
                            afterImageEffect.StartAfterImage();
                        }
                    }
                    else
                    {
                        Action(neutralAir);
                    }
                }
            }

        }
    }
}
