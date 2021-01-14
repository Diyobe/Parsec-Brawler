using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupermanStageGravity : MonoBehaviour
{
    [SerializeField]
    float gravityModifier;
    [SerializeField]
    float gravityMaxModifier;
    [SerializeField]
    float airFrictionModifier;

    [SerializeField]
    BattleManager battleManager;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GravityCoroutine());
    }

    private IEnumerator GravityCoroutine()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < battleManager.PlayersAlive.Count; i++)
        {
            battleManager.PlayersAlive[i].GravityForce = battleManager.PlayersAlive[i].GravityForce * gravityModifier;
            battleManager.PlayersAlive[i].GravityForceMax = battleManager.PlayersAlive[i].GravityForceMax * gravityMaxModifier;
            battleManager.PlayersAlive[i].AirFriction = battleManager.PlayersAlive[i].AirFriction * airFrictionModifier;
        }
    }

}
