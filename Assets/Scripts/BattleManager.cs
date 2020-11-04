using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private PlayerData playerData;

    [SerializeField]
    private PlayerController characterPrefab;
    [SerializeField]
    private InputController inputControllerPrefab;
    [SerializeField] 
    private MultipleTargetCamera cameraController;

    [SerializeField]
    private Transform characterRespawnPosition;
    [SerializeField] 
    private Transform[] characterSpawnPosition;


    private void Start()
    {
        CreateGame();
    }

    private void CreateGame()
    {
        for (int i = 0; i < playerData.PlayerID.Count; i++)
        {
            InputController controller = Instantiate(inputControllerPrefab);
            //controller.SetPlayerID(playerData.PlayerID[i]);
            Instantiate(characterPrefab, characterSpawnPosition[i].position, Quaternion.identity);
        }
    }

}
