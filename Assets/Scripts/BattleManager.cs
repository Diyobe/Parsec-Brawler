using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private PlayerData playerData;
    [SerializeField]
    private int debugPlayerNumber;

    [Header("Prefabs")]
    [SerializeField]
    private PlayerController characterPrefab;
    [SerializeField]
    private InputController inputControllerPrefab;

    [Header("Components")]
    [SerializeField] 
    private MultipleTargetCamera cameraController;

    [Header("Spawn")]
    [SerializeField]
    private Transform respawnPosition;
    [SerializeField] 
    private Transform[] spawnPosition;

    [Header("BlastZone")]
    [SerializeField]
    private BlastZone[] blastZones;



    private void Start()
    {
#if UNITY_EDITOR
        playerData.PlayerID.Clear();
        for (int i = 0; i < debugPlayerNumber; i++)
        {

            playerData.PlayerID.Add(1);
        }
#endif
        CreateGame();
    }

    private void CreateGame()
    {
        for (int i = 0; i < playerData.PlayerID.Count; i++)
        {
            PlayerController player = Instantiate(characterPrefab, spawnPosition[i].position, Quaternion.identity);
            player.tag = "Player" + playerData.PlayerID + 1;

            InputController controller = Instantiate(inputControllerPrefab);
            controller.SetPlayerID(playerData.PlayerID[i]);
            //controller.SetController(player);

            cameraController.targets.Add(player.transform);

        }
        SubscribeBlastZone();
        SubscribeFeedback(); // Pas opti on refait une boucle mais nique

        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        yield return new WaitForSeconds(2);
        // Activate player Input
        // FIGHT !
    }





    //===================================================================================//
    //     B L A S T
    //===================================================================================//
    private void SubscribeBlastZone()
    {
        for (int i = 0; i < blastZones.Length; i++)
        {
            blastZones[i].OnBlast += BlastCharacter;
        }
    }

    public void BlastCharacter(PlayerController blastedCharacter)
    {
        blastedCharacter.ResetToIdle();
        cameraController.targets.Remove(blastedCharacter.transform);
        blastedCharacter.gameObject.SetActive(false);
        StartCoroutine(RespawnCharacter(blastedCharacter.gameObject, 2f));

        // Lose health
    }


    IEnumerator RespawnCharacter(GameObject character, float time)
    {
        yield return new WaitForSeconds(time);

        character.SetActive(true);
        cameraController.targets.Add(character.transform);
        character.transform.position = respawnPosition.position;
    }

    private void UnsubscribeBlastZone()
    {
        for (int i = 0; i < blastZones.Length; i++)
        {
            blastZones[i].OnBlast -= BlastCharacter;
        }
    }





    //===================================================================================//
    //     F E E D B A C K S
    //===================================================================================//

    [Header("Feedback")]
    [SerializeField]
    ParticleSystem hitSpeedline;
    [SerializeField]
    Animator backgroundFlash;
    [SerializeField]
    Animator cameraZoom;
    private void SubscribeFeedback()
    {
        for (int i = 0; i < playerData.PlayerID.Count; i++)
        {
            //blastZones[i].
        }
    }
    private void UnsubscribeFeedback()
    {
        for (int i = 0; i < playerData.PlayerID.Count; i++)
        {
            //blastZones[i].
        }
    }



    private void OnDestroy()
    {
        UnsubscribeBlastZone();
        UnsubscribeFeedback();
    }
}
