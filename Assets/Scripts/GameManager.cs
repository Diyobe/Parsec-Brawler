using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> characters;
    [SerializeField] private GameObject blastExplosion;
    [SerializeField] private Transform respawnPosition;

    [SerializeField] private MultipleTargetCamera camera;

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
                camera.targets.Remove(blastedCharacter.transform);
                character.SetActive(false);
                StartCoroutine(RespawnCharacter(character, 2f));
            }
        }
    }

    IEnumerator RespawnCharacter(GameObject character, float time)
    {
        yield return new WaitForSeconds(time);

        character.SetActive(true);
        camera.targets.Add(character.transform);
        character.transform.position = respawnPosition.position;
    }
}
