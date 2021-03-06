﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<PlayerController> characters;
    [SerializeField] private GameObject blastExplosion;
    [SerializeField] private Transform respawnPosition;

    [SerializeField] private MultipleTargetCamera targetCamera;

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
            //DontDestroyOnLoad(this);
        }
    }

    public void BlastCharacter(PlayerController blastedCharacter)
    {
        foreach(PlayerController character in characters)
        {
            if (character == blastedCharacter)
            {
                character.ResetToIdle();
                targetCamera.targets.Remove(blastedCharacter.transform);
                character.gameObject.SetActive(false);
                StartCoroutine(RespawnCharacter(character.gameObject, 2f));
            }
        }
    }

    IEnumerator RespawnCharacter(GameObject character, float time)
    {
        yield return new WaitForSeconds(time);

        character.SetActive(true);
        targetCamera.targets.Add(character.transform);
        character.transform.position = respawnPosition.position;
    }
}
