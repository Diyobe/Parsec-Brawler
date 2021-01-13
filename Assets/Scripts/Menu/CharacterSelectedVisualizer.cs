using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectedVisualizer : MonoBehaviour
{
    [SerializeField] Transform[] characterPosition = new Transform[4];
    [SerializeField] [ReadOnly] GameObject[] characterInstance = new GameObject[4];
    [SerializeField] GameObject[] characterPrefab = new GameObject[5];



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCharacterInstance(int playerID, CharacterData characterData)
    {
        switch(characterData.CharName)
        {
            case "Lia":
                if (characterInstance[playerID] != null)
                    RemoveCharacterInstance(playerID);
                CharacterSpawn(playerID, 0);
                break;

            case "Ninjamurai":
                if (characterInstance[playerID] != null)
                    RemoveCharacterInstance(playerID);
                CharacterSpawn(playerID, 1);
                break;

            case "Shizuru Fujina":
                if (characterInstance[playerID] != null)
                    RemoveCharacterInstance(playerID);
                CharacterSpawn(playerID, 2);
                break;

            case "Superman":
                if (characterInstance[playerID] != null)
                    RemoveCharacterInstance(playerID);
                CharacterSpawn(playerID, 3);
                break;

            case "Valentine":
                if (characterInstance[playerID] != null)
                    RemoveCharacterInstance(playerID);
                CharacterSpawn(playerID, 4);
                break;
        }
    }

    private void CharacterSpawn(int playerID, int characterPrebabValue)
    {
        characterInstance[playerID] = Instantiate(characterPrefab[characterPrebabValue], characterPosition[playerID].parent);
        characterInstance[playerID].transform.localScale = characterPosition[playerID].localScale;
        characterInstance[playerID].transform.position = characterPosition[playerID].position;
    }

    public void SetActiveInstance(bool isActive)
    {
        for(int i = 0; i < 4; i++)
        {
            if (characterInstance[i] != null)
                characterInstance[i].SetActive(isActive);
        }
    }
    public void SetActiveUniqueInstance(bool isActive, int playerID)
    {
        if (characterInstance[playerID] != null)
            characterInstance[playerID].SetActive(isActive);
    }

    public void RemoveCharacterInstance(int playerID)
    {
        if (characterInstance[playerID] != null)
            Destroy(characterInstance[playerID]);
    }
}
