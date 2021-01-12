using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VoiceActing;

public enum CharacterState
{
    Idle,
    Acting,
    Hit,
    Ejected,
    Dead,
    Dash
}



public class PlayerController : InputControllable
{


    [SerializeField]
    private Transform particlePoint; // Utilisé pour que les particles s'affichent bien au centre
    public Transform ParticlePoint
    {
        get { return particlePoint; }
    }

    [SerializeField]
    protected CharacterCollision characterCollision;
    [SerializeField]
    Animator characterAnimator;
    [SerializeField]
    SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer
    {
        get { return spriteRenderer; }
    }


    [Header("Feedback")]
    [SerializeField]
    private ShakeSprite shakeSprite;
    public ShakeSprite ShakeSprite
    {
        get { return shakeSprite; }
    }

    [SerializeField]
    ParticleSystem smoke;
    [SerializeField]
    protected AfterImageEffect afterImageEffect;


    [Space]
    [Header("Movement")]
    [SerializeField]
    float speedMax = 5;
    [SerializeField]
    float acceleration = 5;
    [SerializeField]
    float decceleration = 5;

    [Space]
    [SerializeField]
    float airFriction = 5;
    [SerializeField]
    float airStop = 0.9f;

    [Space]
    [SerializeField]
    protected float gravityForce = 8;
    [SerializeField]
    protected float gravityForceMax = -10;


    [Space]
    [Header("Jump")]
    [SerializeField]
    float jumpImpulsion;
    [SerializeField]
    int numberOfJumps = 2;
    [SerializeField]
    float crouchTime = 0.1f;

    [Space]
    [Header("Dash")]
    [SerializeField] float dashImpulsion;
    [SerializeField] int numberOfDashes = 1;
    int currentNumberOfDashes;

    [SerializeField] float dashDuration = .2f;
    float dashTimer;

    [Space]
    [Header("Knockback")]
    [SerializeField]
    float baseKnockbackTime;
    [SerializeField]
    float knockbackPowerForWallBounce;

    [SerializeField] float knockbackImpactReductionRate = 0.8f;

    [SerializeField]
    float hitStopOnWall = 0.1f;
    [SerializeField]
    float shakePowerOnWall = 0.2f;

    [Space]
    [Header("Action")]
    [SerializeField]
    AttackController crouchJump;
    [SerializeField]
    AttackController attackLeft;
    [SerializeField]
    AttackController attackUp;
    [SerializeField]
    AttackController attackDown;

    [Header("ActionAerial")]
    [SerializeField]
    AttackController attackLeftAerial;
    [SerializeField]
    AttackController attackUpAerial;
    [SerializeField]
    AttackController attackDownAerial;


    protected Vector2 knockbackPower;
    int knockbackAnimation;
    float knockbackTime;
    float knockbackMaxTime;

    CharacterState state = CharacterState.Idle;

    private int direction;
    public int Direction
    {
        get { return direction; }
        set { direction = value; }
    }

    float currentSpeedX;
    public float CurrentSpeedX
    {
        get { return currentSpeedX; }
    }

    protected float currentSpeedY;
    public float CurrentSpeedY
    {
        get { return currentSpeedY; }
    }

    int currentNumberOfJumps;
    float currentCrouchTime;
    float crouchSaveSpeedX = 0; // Ptet à dégager ?

    bool endAction = false;
    bool canEndAction = false;

    AttackController currentAttack;
    AttackController currentAttackController;

    IEnumerator motionSpeedCoroutine;


    public AudioClip dashSound;

    public event Action OnWallBounce;
    public event Action OnKnockback;
    public event ActionPlayerController OnSuperKnockback;
    public event ActionPlayerController OnFlashMove;



    List<input> buffer;
    bool active = false;
    public bool Active
    {
        get { return active; }
        set { active = value; }
    }

    private int characterIndex;

    public int CharacterIndex
    {
        get { return characterIndex; }
    }


    public void SetMaterial(Material mat, Texture2D tex, int ID)
    {
        spriteRenderer.material = mat;
        afterImageEffect.SetSwapTexture(tex);
        characterIndex = ID;
    }


    private void Start()
    {
        characterCollision.doAction += ResetJump;
        characterCollision.doAction += ResetDash;
        characterCollision.OnWallCollision += WallBounce;
        characterCollision.OnGroundCollision += GroundBounce;
    }
    public override void UpdateBuffer(List<input> inputBuffer, int inputID)
    {
        buffer = inputBuffer;
    }

    public void SetCharacterIndex(int ID)
    {
        characterIndex = ID;
    }

    private void Update()
    {
        // OnTriggerEnter 
        // OnColliderEnter
        // Update
        // Cette ligne est pour empêcher qu'il y ait un bug d'animation au moment où le perso joue une action pile à la frame ou le perso termine son action précédente
        if (canEndAction == false)
            canEndAction = true;
        // Input
        if (knockbackPower != Vector2.zero)
        {
            UpdateKnockback();
        }

        if (state == CharacterState.Idle)
        {
            ApplyGravity();
            UpdateControls();
        }
        else if (state == CharacterState.Acting)
        {
            CheckCrouch();
            ApplyGravity();
        }
        else if (state == CharacterState.Dash)
        {
            UpdateDash();
        }
        characterCollision.Move(currentSpeedX + knockbackPower.x, currentSpeedY + knockbackPower.y);
        SetAnimation();


        // Cette ligne est pour empêcher qu'il y ait un bug d'animation au moment où le perso joue une action pile à la frame ou le perso termine son action précédente
        if (endAction == true)
            EndAction();
    }

    private void UpdateDash()
    {
        if (dashTimer >= dashDuration)
        {
            state = CharacterState.Idle;
            afterImageEffect.EndAfterImage();
            dashTimer = 0;
            if (characterCollision.IsGrounded && currentNumberOfDashes <= 0)
            {
                currentNumberOfDashes = numberOfDashes;
            }
        }
        else
        {
            dashTimer += Time.deltaTime * GetMotionSpeed();
        }
    }

    private void ResetJump()
    {
        currentNumberOfJumps = numberOfJumps;
        if (state == CharacterState.Idle)
        {
            knockbackPower = Vector2.zero;
        }
        else if (state == CharacterState.Acting)
        {
            currentSpeedX = 0;
        }
    }

    private void ResetDash()
    {
        dashTimer = 0;
        currentNumberOfDashes = numberOfDashes;
        afterImageEffect.EndAfterImage();

    }

    public void ResetWhenRespawn()
    {
        ResetJump();
        ResetDash();
    }

    void UpdateControls()
    {
        if (active == false)
            return;
        CheckHorizontal(buffer);
        CheckJump(buffer);
        CheckAttack(buffer);
        CheckDash(buffer);
    }

    void CheckDash(List<input> buffer)
    {
        float vertical;
        float horizontal;
        if (state != CharacterState.Idle)
            return;

        for (int i = 0; i < buffer.Count; i++)
        {
            if (buffer[i].dash && currentNumberOfDashes > 0)
            {
                buffer[i].dash = false;
                --currentNumberOfDashes;

                if (Mathf.Abs(buffer[i].vertical) > 0.35f)
                {
                    vertical = Mathf.Sign(buffer[i].vertical);
                }
                else
                {
                    vertical = 0;
                }

                if (Mathf.Abs(buffer[i].horizontal) > 0.35f)
                {
                    horizontal = Mathf.Sign(buffer[i].horizontal);
                }
                else
                {
                    horizontal = 0;
                }

                Dash(horizontal, vertical);
            }
        }
    }

    void CheckJump(List<input> buffer)
    {
        for (int i = 0; i < buffer.Count; i++)
        {
            if (buffer[i].jump && currentNumberOfJumps > 0)
            {
                buffer[i].jump = false;
                --currentNumberOfJumps;
                if (characterCollision.IsGrounded == true)
                {
                    // Crouch
                    Action(crouchJump);
                    crouchSaveSpeedX = currentSpeedX;
                    currentCrouchTime = crouchTime;
                    return;
                }
                else
                    Jump(jumpImpulsion);
            }
        }

    }
    protected virtual void CheckAttack(List<input> buffer)
    {
        for (int i = 0; i < buffer.Count; i++)
        {
            if (buffer[i].hit)
            {
                buffer[i].hit = false;
                if (characterCollision.IsGrounded == true)
                {
                    if (Mathf.Abs(buffer[i].vertical) > Mathf.Abs(buffer[i].horizontal))
                    {
                        if (buffer[i].vertical > 0)
                            Action(attackUp);
                        else
                            Action(attackDown);
                    }
                    else
                    {
                        Action(attackLeft);
                    }
                }
                else
                {
                    if (Mathf.Abs(buffer[i].vertical) > Mathf.Abs(buffer[i].horizontal))
                    {
                        if (buffer[i].vertical > 0)
                            Action(attackUpAerial);
                        else
                            Action(attackDownAerial);
                    }
                    else
                    {
                        if (currentSpeedX != 0)
                            direction = (int)Mathf.Sign(currentSpeedX);
                        Action(attackLeftAerial);
                    }
                }
            }
        }
    }


    void CheckHorizontal(List<input> buffer)
    {
        if (characterCollision.IsGrounded == true)
        {
            if (buffer[0].horizontal != 0)
            {
                currentSpeedX += Mathf.Sign(buffer[0].horizontal) * acceleration;
                direction = (int)Mathf.Sign(currentSpeedX);
            }
            else
            {
                currentSpeedX = 0;
                /*currentSpeedX -= (decceleration * Mathf.Sign(currentSpeedX)) * Time.deltaTime;
                if (Mathf.Abs(currentSpeedX) <= decceleration * Time.deltaTime)
                    currentSpeedX = 0;*/
            }
        }
        else
        {
            if (buffer[0].horizontal != 0)
            {
                currentSpeedX += Mathf.Sign(buffer[0].horizontal) * (acceleration - airFriction);
            }
            else
            {
                currentSpeedX *= airStop;    // Pas FPS indépendant à refaire
                if (currentSpeedX <= airFriction && currentSpeedX >= -airFriction)
                    currentSpeedX = 0;
            }
        }
        currentSpeedX = Mathf.Clamp(currentSpeedX, -speedMax, speedMax);
    }






    public void MoveForward(float value)
    {
        currentSpeedX = value * direction;
    }

    public void AddForce(float forceX, float forceY)
    {
        knockbackMaxTime += new Vector2(forceX, forceY).magnitude;
        knockbackPower += new Vector2(forceX, forceY);
    }

    private void CheckCrouch()
    {
        if (currentCrouchTime > 0)
        {
            currentCrouchTime -= Time.deltaTime * GetMotionSpeed();
            if (currentCrouchTime <= 0)
            {
                state = CharacterState.Idle;
                characterAnimator.SetTrigger("Idle");
                characterCollision.IsGrounded = false;
                currentSpeedX = crouchSaveSpeedX;
                Jump(jumpImpulsion);
            }
        }
    }

    public void Jump(float jumpImpulse)
    {
        currentSpeedY = jumpImpulse;
        characterCollision.IsGrounded = false;
        if (currentSpeedX != 0)
            direction = (int)Mathf.Sign(currentSpeedX);
    }

    public void Dash(float horizontal, float vertical)
    {
        state = CharacterState.Dash;

        characterCollision.IsGrounded = false;

        afterImageEffect.StartAfterImage();
        Vector2 normalizeDirection = new Vector2(horizontal, vertical).normalized;
        currentSpeedX = dashImpulsion * normalizeDirection.x;
        currentSpeedY = dashImpulsion * normalizeDirection.y;

        TengenToppaAudioManager.Instance.PlaySound(dashSound, 0.4f);
    }

    private void ApplyGravity()
    {
        if (characterCollision.IsGrounded == true)
        {
            currentSpeedY = 0;
        }
        currentSpeedY -= gravityForce * Time.deltaTime * GetMotionSpeed();
        if (currentSpeedY < gravityForceMax)
            currentSpeedY = gravityForceMax;
    }

    public void ResetToIdle()
    {
        currentCrouchTime = 0;
        currentSpeedX = 0;
        currentSpeedY = 0;
        afterImageEffect.EndAfterImage();
        knockbackTime = 0;

        knockbackPower = Vector2.zero;
        canEndAction = false;
        endAction = false;

        state = CharacterState.Idle;
        characterAnimator.SetTrigger("Idle");
        smoke.Stop();

        if (currentAttackController != null)
            currentAttackController.ActionEnd();
    }


    // ==========================================================================================================
    //    A C T I O N
    // ==========================================================================================================
    #region Action 

    public void Action(AttackController action)
    {
        if (currentAttackController != null)
            currentAttackController.ActionEnd();
        currentAttackController = null;

        currentAttack = action;
        characterAnimator.ResetTrigger("Idle");
        characterAnimator.Play(currentAttack.AttackAnimation.name, 0, 0f);

        endAction = false;
        canEndAction = false;

        if (action.KeepMomentum == false)
        {
            currentSpeedX = 0;
            currentSpeedY = 0;
        }

        state = CharacterState.Acting;
    }

    // Appelé par les anims
    // Uniquement utilisable pour l'action principale
    public void ActionActive()
    {
        if (currentAttack != null)
        {
            currentAttackController = (Instantiate(currentAttack, this.transform.position, Quaternion.identity));
            currentAttackController.CreateAttack(this);
        }
    }

    // Appelé par les anims
    // Uniquement utilisable pour l'action principale
    public void ActionUnactive()
    {
        if (currentAttackController != null)
        {
            currentAttackController.ActionUnactive();
        }
    }

    // Appelé par les anims
    // Uniquement utilisable pour dire d'arrêter l'action
    public void CanEndAction()
    {
        if (canEndAction == true)
        {
            endAction = true;
        }
    }

    private void EndAction()
    {
        if (currentAttackController != null)
            currentAttackController.ActionEnd();
        state = CharacterState.Idle;
        characterAnimator.SetTrigger("Idle");
    }
    #endregion


    // ==========================================================================================================
    //    A N I M A T I O N
    // ==========================================================================================================
    #region Animation 

    protected void SetAnimation()
    {
        if (direction == 1)
            spriteRenderer.flipX = false;
        else if (direction == -1)
            spriteRenderer.flipX = true;

        characterAnimator.SetBool("AerialUp", !characterCollision.IsGrounded);

        if (characterCollision.IsGrounded == false && (characterCollision.SpeedY) <= 0)
            characterAnimator.SetBool("AerialDown", true);
        else
            characterAnimator.SetBool("AerialDown", false);

        if (state == CharacterState.Idle)
            characterAnimator.SetBool("Moving", characterCollision.SpeedX != 0);
        characterAnimator.SetBool("Dash", state == CharacterState.Dash);

    }


    public void SetCharacterMotionSpeed(float newSpeed, float time = 0)
    {
        characterCollision.CharacterMotionSpeed = newSpeed;
        characterAnimator.speed = newSpeed;
        if (time > 0)
        {
            if (motionSpeedCoroutine != null)
                StopCoroutine(motionSpeedCoroutine);
            motionSpeedCoroutine = MotionSpeedCoroutine(time);
            StartCoroutine(motionSpeedCoroutine);
        }
    }


    private IEnumerator MotionSpeedCoroutine(float time)
    {
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        characterCollision.CharacterMotionSpeed = 1;
        characterAnimator.speed = 1;
    }


    public float GetMotionSpeed()
    {
        return characterAnimator.speed;
    }

    #endregion











    // ==========================================================================================================
    //    K N O C K B A C K
    // ==========================================================================================================

    private void OnTriggerEnter(Collider other)
    {
        //Attaque ennemi détecté
        if (other.tag != this.transform.tag && state != CharacterState.Dash)
        {
            Knockback(other.GetComponent<AttackController>());
        }
        else if (other.tag != this.transform.tag && state == CharacterState.Dash)
        {
            AttackController a = other.GetComponent<AttackController>();
            if (a != null) { a.DoSomething(this); OnFlashMove.Invoke(this); }

        }

    }

    private void Knockback(AttackController attack)
    {
        if (attack == null)
            return;

        state = CharacterState.Hit;
        currentSpeedX = 0;
        currentSpeedY = 0;
        currentCrouchTime = 0;
        Vector2 direction = this.transform.position - (attack.transform.position + attack.KnockbackAngle);
        direction *= attack.KnockbackPower;
        knockbackPower = direction;
        shakeSprite.Shake(attack.TargetShakePower, attack.HitStop);
        attack.HasHit(this);
        smoke.Play();

        knockbackTime = 0;
        knockbackMaxTime = baseKnockbackTime + (direction.magnitude);

        afterImageEffect.StartAfterImage();
        KnockbackAnimation();

        OnKnockback.Invoke();
        CheckSuperKnockback(attack);
    }

    protected void UpdateKnockback()
    {
        if (GetMotionSpeed() == 0)
        {
            return;
        }
        knockbackTime += Time.deltaTime * GetMotionSpeed();
        knockbackPower = Vector2.Lerp(knockbackPower, Vector2.zero, knockbackTime / knockbackMaxTime);

        if (knockbackPower.magnitude < knockbackPowerForWallBounce && state == CharacterState.Hit)
        {
            state = CharacterState.Idle;
            characterAnimator.SetTrigger("Idle");
            afterImageEffect.EndAfterImage();
            smoke.Stop();
            currentSpeedX = knockbackPower.x;
            currentSpeedY = knockbackPower.y;
            knockbackPower = Vector2.zero;
        }

        if (knockbackPower.magnitude < 1f)
        {
            knockbackTime = 0;
            knockbackMaxTime = 0;
            knockbackPower = Vector2.zero;
        }
    }


    private void KnockbackAnimation()
    {
        knockbackAnimation += 1;
        if (knockbackAnimation == 2)
            knockbackAnimation = 0;
        characterAnimator.SetTrigger("Hit");
        characterAnimator.SetInteger("HitAnimation", knockbackAnimation);
    }


    private void CheckSuperKnockback(AttackController attack)
    {
        int layerMask = 1 << 8;
        RaycastHit hit;
        Physics.Raycast(this.transform.position, knockbackPower, out hit, 50, layerMask);
        if (hit.collider == null)
        {
            OnSuperKnockback.Invoke(this);
        }
    }


    public void WallBounce(Transform collider)
    {
        if (state == CharacterState.Hit)
        {
            knockbackPower.x = -knockbackPower.x;
            shakeSprite.Shake(shakePowerOnWall, hitStopOnWall);
            SetCharacterMotionSpeed(0, hitStopOnWall);
            KnockbackAnimation();
            direction = -direction;
            CheckCollisionComponent(collider);
            currentSpeedY = 0;
            knockbackPower *= knockbackImpactReductionRate;

            OnWallBounce.Invoke();
        }
        else if (characterCollision.IsGrounded == false)
        {
            currentSpeedX = 0;
            knockbackPower.x = 0;
        }
    }

    public void GroundBounce(Transform collider)
    {
        if (state == CharacterState.Hit)
        {
            knockbackPower.y = -knockbackPower.y;
            shakeSprite.Shake(shakePowerOnWall, hitStopOnWall);
            SetCharacterMotionSpeed(0, hitStopOnWall);
            KnockbackAnimation();
            CheckCollisionComponent(collider);
            currentSpeedY = 0;
            knockbackPower *= knockbackImpactReductionRate;

            OnWallBounce.Invoke();
        }
        else if (characterCollision.IsGrounded == false)
        {
            currentSpeedY = 0;
            knockbackPower.y = 0;
        }
    }

    // a refactor éventuellement
    private void CheckCollisionComponent(Transform collider)
    {
        WallDestructible a = collider.GetComponent<WallDestructible>();
        if (a != null)
            a.Damage(particlePoint.position);
    }

}
