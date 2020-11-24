using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private PlayerData playerData;
    [SerializeField]
    private bool debug;
    [SerializeField]
    private int debugPlayerNumber;
    [SerializeField]
    private int debugPlayerLives = 1;

    [Header("Prefabs")]
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

    [Header("HUD")]
    [SerializeField]
    private Transform hudParent;
    [SerializeField]
    List<BattleHud> battleHuds = new List<BattleHud>();

    [SerializeField]
    WinMenu winMenu;

    List<PlayerController> playersAlive = new List<PlayerController>();
    List<int> playersLives = new List<int>();

    List<int> listLosers = new List<int>();

    public AudioClip battleTheme;
    public AudioClip bumpSound;
    public AudioClip flashMoveClip;
    private void Start()
    {
#if UNITY_EDITOR
        if (debug == true)
        {
            //playerData.PlayerID.Clear();
            //for (int i = 0; i < debugPlayerNumber; i++)
            //{

            //    playerData.PlayerID.Add(i);
            //}
        }
#endif
        CreateGame();
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
    }

    private void CreateGame()
    {
        for (int i = 0; i < playerData.CharacterInfos.Count; i++)
        {
            CharacterData data = playerData.CharacterInfos[i].CharacterData;
            int colorID = playerData.CharacterInfos[i].CharacterColorID;
            int playerID = playerData.CharacterInfos[i].PlayerID;

            PlayerController player = Instantiate(data.PlayerController, spawnPosition[i].position, Quaternion.identity);
            player.SpriteRenderer.material = data.SwapColors[colorID];
            player.Direction = (int)Mathf.Sign(spawnPosition[i].localScale.x);
            player.gameObject.tag = "Player" + (playerID + 1);

            playersAlive.Add(player);
            playersLives.Add(playerData.NumberOfLives);

            InputController controller = Instantiate(inputControllerPrefab);
            controller.SetPlayerID(playerID);
            controller.SetPlayerController(player);
            controller.gameObject.SetActive(true);

            cameraController.targets.Add(player.transform);

        }
        SubscribeBlastZone();
        SubscribeFeedback(); // Pas opti on refait une boucle mais nique

        StartCoroutine(StartGameCoroutine());
        TengenToppaAudioManager.Instance.PlayMusic(battleTheme, battleTheme);
    }

    private IEnumerator StartGameCoroutine()
    {
        for (int i = 0; i < 4; i++) // 4 = max number of players
        {
            if (i < playersAlive.Count)
            {
                playersAlive[i].OnKnockback += battleHuds[i].ShakeFace;
                battleHuds[i].gameObject.SetActive(true);
                battleHuds[i].DrawFace(playerData.CharacterInfos[i].CharacterData.Face, playerData.CharacterInfos[i].CharacterData.SwapColorsUI[i]);
                battleHuds[i].DrawLives(playerData.NumberOfLives);
            }
        }

        yield return new WaitForSeconds(5f);
        for (int i = 0; i < playersAlive.Count; i++)
        {
            playersAlive[i].Active = true;
        }
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
        for (int i = 0; i < playersAlive.Count; i++)
        {
            if(playersAlive[i] == blastedCharacter && blastedCharacter.enabled == true)
            {
                playersLives[i] -= 1;
                blastedCharacter.ResetToIdle();
                cameraController.targets.Remove(blastedCharacter.transform);
                blastedCharacter.SpriteRenderer.enabled = false;
                blastedCharacter.enabled = false;

                if (playersLives[i] <= 0) // Si le perso n'a plus de vie = DED
                {
                    listLosers.Add(blastedCharacter.CharacterIndex);

                    playersAlive[i].OnKnockback -= battleHuds[i].ShakeFace; // histoire d'être sur
                    playersAlive.RemoveAt(i);
                    playersLives.RemoveAt(i);
                    battleHuds[i].DrawLivesFeedback(0);
                    battleHuds.RemoveAt(i);
                    if(playersAlive.Count <= 1) // Si il n'y a plus qu'un combattant
                    {
                        StartCoroutine(WinGameCoroutine());
                    }
                }
                else
                {
                    StartCoroutine(RespawnCharacter(blastedCharacter, 2f));
                }
                break;
            }
        }

        // On update le hud mais c'est une boucle de trop, pas opti mais au moins c'est un peu clair
        for (int i = 0; i < battleHuds.Count; i++)
        {
            if(battleHuds[i].isActiveAndEnabled)
                battleHuds[i].DrawLivesFeedback(playersLives[i]);
        }
    }


    IEnumerator RespawnCharacter(PlayerController character, float time)
    {
        yield return new WaitForSeconds(time);

        character.SpriteRenderer.gameObject.SetActive(true);
        character.SpriteRenderer.enabled = true;
        character.enabled = true;

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


    private void SoundWallBounce()
    {
        TengenToppaAudioManager.Instance.PlaySound(bumpSound, 0.8f, 0.5f, 2f);
    }


    private IEnumerator WinGameCoroutine()
    {
        for (int i = 0; i < blastZones.Length; i++)
        {
            blastZones[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < playersAlive.Count; i++)
        {
            playersAlive[i].SetCharacterMotionSpeed(0.2f, 1);
        }
        TengenToppaAudioManager.Instance.StopMusic(4f);
        yield return new WaitForSecondsRealtime(0.6f);
        Time.timeScale = 0.2f;
        yield return new WaitForSecondsRealtime(2.4f);
        winAnimator.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1.1f);
        Time.timeScale = 0f;
        hudParent.gameObject.SetActive(false);
        listLosers.Add(playersAlive[0].CharacterIndex);
        winMenu.CreateMenu(listLosers);
        
        /*yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);*/
    }














    //===================================================================================//
    //     F E E D B A C K S
    //===================================================================================//

    [Header("Feedback")]
    [SerializeField]
    ParticleSystem hitSpeedline;
    [SerializeField]
    GameObject finalHitParticle;

    [SerializeField]
    ParticleSystem flashMoveParticle;
    [SerializeField]
    Animator animatorFlashMove;

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
            playersAlive[i].OnWallBounce += SoundWallBounce;

            playersAlive[i].OnKnockback += HitSpeedline;
            playersAlive[i].OnKnockback += BackgroundFlash;
            playersAlive[i].OnKnockback += CameraZoomDeSesMorts;

            playersAlive[i].OnFlashMove += FlashMove;

            playersAlive[i].OnSuperKnockback += FinalFeedback;
        }
    }

    private void UnsubscribeFeedback()
    {
        for (int i = 0; i < playersAlive.Count; i++)
        {
            playersAlive[i].OnWallBounce -= HitSpeedline;

            playersAlive[i].OnKnockback -= HitSpeedline;
            playersAlive[i].OnKnockback -= BackgroundFlash;
            playersAlive[i].OnKnockback -= CameraZoomDeSesMorts;

            playersAlive[i].OnFlashMove -= FlashMove;

            playersAlive[i].OnSuperKnockback -= FinalFeedback;
        }
    }


    public void BackgroundFlash()
    {
        backgroundFlash.SetTrigger("Feedback");
    }

    public void HitSpeedline()
    {
        hitSpeedline.Play();
    }

    public void FlashMove(PlayerController player)
    {
        hitSpeedline.Play();
        flashMoveParticle.Play();
        flashMoveParticle.transform.position = player.ParticlePoint.position;
        animatorFlashMove.SetTrigger("Feedback");

        float angle = Vector2.Angle(new Vector2(player.CurrentSpeedX, player.CurrentSpeedY), Vector2.up);
        flashMoveParticle.transform.eulerAngles = new Vector3(angle, -90, 90);
        TengenToppaAudioManager.Instance.PlaySound(flashMoveClip, 1);
    }

    public void CameraZoomDeSesMorts()
    {
        if (playersAlive.Count <= 2)
        {
            cameraZoom.SetTrigger("Feedback");
        }
    }


    public void FinalFeedback(PlayerController player)
    {
        if (playersAlive.Count <= 2 && playersLives[playersAlive.IndexOf(player)] == 1)
        {
            cameraZoom.SetTrigger("FinalFeedback");
            for (int i = 0; i < playersAlive.Count; i++)
            {
                playersAlive[i].SetCharacterMotionSpeed(0, 1.6f);
            }
            player.ShakeSprite.Shake(0.2f, 1.6f);
            StartCoroutine(FinalFeedbackCoroutine(finalHitParticle, player.transform.position));
        }

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
        for (int i = 0; i < playersAlive.Count; i++)
        {
            playersAlive[i].OnKnockback -= battleHuds[i].ShakeFace; // histoire d'être sur
        }
    }

}
