using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private PlayerData playerData;
    [SerializeField]
    private int debugPlayerNumber;
    [SerializeField]
    private int debugPlayerLives = 1;

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

    List<PlayerController> playersAlive = new List<PlayerController>();
    List<int> playersLives = new List<int>();


    private void Start()
    {
#if UNITY_EDITOR
        playerData.PlayerID.Clear();
        for (int i = 0; i < debugPlayerNumber; i++)
        {

            playerData.PlayerID.Add(i);
        }
#endif
        CreateGame();
    }

    private void CreateGame()
    {
        for (int i = 0; i < playerData.PlayerID.Count; i++)
        {
            PlayerController player = Instantiate(characterPrefab, spawnPosition[i].position, Quaternion.identity);
            player.gameObject.tag = "Player" + (playerData.PlayerID[i]+1);
            playersAlive.Add(player);
            playersLives.Add(debugPlayerLives);

            InputController controller = Instantiate(inputControllerPrefab);
            controller.SetPlayerID(playerData.PlayerID[i]);
            controller.SetPlayerController(player);
            controller.gameObject.SetActive(true);

            cameraController.targets.Add(player.transform);

        }
        SubscribeBlastZone();
        SubscribeFeedback(); // Pas opti on refait une boucle mais nique

        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        yield return new WaitForSeconds(2);
        for (int i = 0; i < playersAlive.Count; i++)
        {
            playersAlive[i].Active = true;
        }
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

        for (int i = 0; i < playersAlive.Count; i++)
        {
            if(playersAlive[i] == blastedCharacter)
            {
                playersLives[i] -= 1;
                if(playersLives[i] <= 0) // Si le perso n'a plus de vie = DED
                {
                    playersAlive.RemoveAt(i);
                    playersLives.RemoveAt(i);
                    if(playersAlive.Count <= 1) // Si il n'y a plus qu'un combattant
                    {
                        StartCoroutine(WinGameCoroutine());
                    }
                }
                else
                {
                    StartCoroutine(RespawnCharacter(blastedCharacter.gameObject, 2f));
                }
            }
        }
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





    private IEnumerator WinGameCoroutine()
    {
        for (int i = 0; i < playersAlive.Count; i++)
        {
            playersAlive[i].SetCharacterMotionSpeed(0.2f, 1);
        }
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 0.2f;
        yield return new WaitForSecondsRealtime(2f);
        winAnimator.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
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
    [SerializeField]
    private GameObject winAnimator;


    private void SubscribeFeedback()
    {
        for (int i = 0; i < playersAlive.Count; i++)
        {
            playersAlive[i].OnWallBounce += HitSpeedline;

            playersAlive[i].OnKnockback += HitSpeedline;
            playersAlive[i].OnKnockback += BackgroundFlash;
            playersAlive[i].OnKnockback += CameraZoomDeSesMorts;

            playersAlive[i].OnSuperKnockback += HitSpeedline;
        }
    }

    private void UnsubscribeFeedback()
    {

    }


    public void BackgroundFlash()
    {
        backgroundFlash.SetTrigger("Feedback");
    }

    public void HitSpeedline()
    {
        hitSpeedline.Play();
    }

    public void CameraZoomDeSesMorts()
    {
        cameraZoom.SetTrigger("Feedback");
    }


    public void FinalFeedback(GameObject hitAnimation, Vector3 position)
    {
        cameraZoom.SetTrigger("FinalFeedback");
        StartCoroutine(FinalFeedbackCoroutine(hitAnimation, position));
    }

    private IEnumerator FinalFeedbackCoroutine(GameObject hitAnimation, Vector3 position)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(Instantiate(hitAnimation, position, Quaternion.identity), 5f);
    }



    //===================================================================================//
    //     D E S T R O Y
    //===================================================================================//

    private void OnDestroy()
    {
        UnsubscribeBlastZone();
        UnsubscribeFeedback();
    }

}
