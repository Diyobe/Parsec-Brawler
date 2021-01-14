using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiaStageBackground : MonoBehaviour
{
    [SerializeField]
    BattleManager battleManager;
    [SerializeField]
    BlastZone[] blastZones;

    [SerializeField]
    Animator cameraAnimation;

    void Start()
    {
        StartCoroutine(InitializationCoroutine());
    }

    private IEnumerator InitializationCoroutine()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < battleManager.PlayersAlive.Count; i++)
        {
            battleManager.PlayersAlive[i].OnFlashMove += FlashMoveFeedback;
            battleManager.PlayersAlive[i].OnSuperKnockback += SuperKnockbackFeedback;
        }
        for (int i = 0; i < blastZones.Length; i++)
        {
            blastZones[i].OnBlast += BlastZoneKill;
        }
    }

    public void FlashMoveFeedback(PlayerController p)
    {
        cameraAnimation.SetTrigger("FeedbackFlashMove");
    }
    public void BlastZoneKill(PlayerController p)
    {
        cameraAnimation.SetTrigger("FeedbackBlast");
    }
    public void SuperKnockbackFeedback(PlayerController p)
    {
        cameraAnimation.SetTrigger("FeedbackSuperKnockback");
    }
}
