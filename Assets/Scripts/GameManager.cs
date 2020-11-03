using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> characters;
    [SerializeField] private GameObject blastExplosion;
    [SerializeField] private Transform respawnPosition;

    public static GameManager Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void BlastCharacter(GameObject blastedCharacter)
    {
        foreach(GameObject character in characters)
        {
            if (character == blastedCharacter)
            {
                Instantiate(blastExplosion, character.transform.position, Quaternion.identity);
                character.SetActive(false);
                RespawnCharacter(character);
            }
        }
    }

    private void RespawnCharacter(GameObject character)
    {
        character.SetActive(true);
        character.transform.position = respawnPosition.position;
    }
}
