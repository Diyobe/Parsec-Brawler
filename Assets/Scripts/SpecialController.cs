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
    [SerializeField]
    AttackController taunt2;
    [SerializeField]
    bool canUse2Taunt;


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
                buffer[i].taunt = false;
                if (!canUse2Taunt)
                    Action(taunt);
                else
                {
                    int random = UnityEngine.Random.Range(1, 3);
                    switch (random)
                    {
                        case 1:
                            Action(taunt);
                            break;
                        case 2:
                            Action(taunt2);
                            break;
                    }
                }
                if (i + 1 < buffer.Count)
                {
                    if (buffer[i + 1].taunt)
                        buffer[i + 1].taunt = false;
                }
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
