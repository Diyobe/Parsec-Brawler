using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/CharacterData", order = 1)]
public class CharacterData : ScriptableObject
{
    [SerializeField]
    private string charName;

    public string CharName
    {
        get { return charName; }
        set { charName = value; }
    }

    [SerializeField]
    private Sprite face;

    public Sprite Face
    {
        get { return face; }
    }

    [SerializeField]
    private PlayerController playerController;

    public PlayerController PlayerController
    {
        get { return playerController; }
        set { playerController = value; }
    }

    [SerializeField]
    private Material[] swapColors;

    public Material[] SwapColors
    {
        get { return swapColors; }
        set { swapColors = value; }
    }
}
