using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActionProxy : MonoBehaviour
{
    [SerializeField]
    PlayerController characterController;
    
    public void ActionActive()
    {
        characterController.ActionActive();
    }
    public void ActionInactive()
    {
        characterController.ActionUnactive();
    }
    public void ActionEnd()
    {
        characterController.EndAction();
    }

    public void CanCancel()
    {
        characterController.CanCancel();
    }
}
