using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{

    [SerializeField]
    private AnimationClip attackAnimation;
    public AnimationClip AttackAnimation
    {
        get { return attackAnimation; }
    }

    [SerializeField]
    private float lifetime;
    public float Lifetime
    {
        get { return lifetime; }
    }


    [SerializeField]
    private bool keepMomentum;
    public bool KeepMomentum
    {
        get { return keepMomentum; }
    }

    [SerializeField]
    private GameObject onHitAnimation;
    public GameObject OnHitAnimation
    {
        get { return onHitAnimation; }
    }


    [Space]
    [SerializeField]
    float knockbackPower = 1;
    public float KnockbackPower
    {
        get { return knockbackPower; }
    }

    [SerializeField]
    private Vector3 knockbackAngle;
    public Vector3 KnockbackAngle
    {
        get { return knockbackAngle; }
    }
    

    [SerializeField]
    private bool hitStopUser = true;
    public bool HitStopUser
    {
        get { return hitStopUser; }
    }
    [SerializeField]
    private float hitStop = 0.15f;
    public float HitStop
    {
        get { return hitStop; }
    }





    [Space]
    [SerializeField]
    private float targetShakePower;
    public float TargetShakePower
    {
        get { return targetShakePower; }
    }

    [SerializeField]
    private float zoom;

    [Space]
    [SerializeField]
    private float shakeScreen;
    [SerializeField]
    private float shakeScreenTime;



    [Header("Components")]
    [SerializeField]
    private BoxCollider hitbox;

    PlayerController user;
    IEnumerator attackCoroutine;


    public void CreateAttack(PlayerController cUser)
    {
        user = cUser;
        this.transform.localScale = new Vector3(this.transform.localScale.x * user.SpriteRenderer.transform.localScale.x * user.Direction, 
                                                this.transform.localScale.y * user.SpriteRenderer.transform.localScale.y, 
                                                user.transform.localScale.z);

        // hitbox
        hitbox.enabled = true;

        // Link to user
        this.transform.SetParent(cUser.transform);

        // Tag
        this.tag = cUser.tag;

        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);
        attackCoroutine = AttackBehaviorCoroutine();
        StartCoroutine(attackCoroutine);
    }

    private IEnumerator AttackBehaviorCoroutine()
    {
        int currentFrame = 0;
        float lifetime = Mathf.Max(0, (Lifetime * 60) - currentFrame) / 60f;
        while (lifetime > 0)
        {
            lifetime -= Time.deltaTime * user.GetMotionSpeed();
            yield return null;
        }
        ActionEnd();
    }

    public void HasHit(PlayerController target)
    {
        //target.PlaySound(attackBehavior.OnHitSound);

        //Feedback
        if (OnHitAnimation != null)
        {
            Destroy(Instantiate(OnHitAnimation, target.ParticlePoint.position, Quaternion.identity), 5f);
        }

        if (HitStop > 0)
        {
            user.SetCharacterMotionSpeed(0, HitStop);
            target.SetCharacterMotionSpeed(0, HitStop);
        }
        ActionEnd();
    }

    public void DoSomething(PlayerController target)
    {
        user.SetCharacterMotionSpeed(0.35f, HitStop);
        target.SetCharacterMotionSpeed(0.35f, HitStop);
    }

    public void StopUser(float motionSpeed, float time)
    {
        user.SetCharacterMotionSpeed(motionSpeed, time);
    }

    public void ActionActive()
    {
        hitbox.enabled = true;
    }

    public void ActionUnactive()
    {
        hitbox.enabled = false;
    }

    public void ActionEnd()
    {
        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);
        Destroy(this.gameObject);
    }








}
