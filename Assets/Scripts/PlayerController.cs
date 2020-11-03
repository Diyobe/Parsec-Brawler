using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CharacterState
{
    Idle,
    Acting,
    Hit,
    Ejected,
    Dead
}



public class PlayerController : MonoBehaviour
{


    [SerializeField]
    private Transform particlePoint; // Utilisé pour que les particles s'affichent bien au centre
    public Transform ParticlePoint
    {
        get { return particlePoint; }
    }

    [SerializeField]
    CharacterCollision characterCollision;
    [SerializeField]
    Animator characterAnimator;


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


    [Space]
    [Header("Knockback")]
    [SerializeField]
    float knockbackPowerReduce;
    [SerializeField]
    float knockbackPowerForWallBounce;
    [SerializeField]
    float knockbackWallBounce;



    protected Vector2 knockbackPower;

    CharacterState state;

    private int direction;
    public int Direction
    {
        get { return direction; }
    }

    float currentSpeedX;

    int currentNumberOfJumps;

    bool endAction = false;
    bool canEndAction = false;

    AttackController currentAttack;
    AttackController currentAttackController;

    IEnumerator motionSpeedCoroutine;


    List<input> buffer;

    private void Start()
    {
        characterCollision.doAction += ResetJump;
        characterCollision.OnWallCollision += WallBounce;
        characterCollision.OnGroundCollision += GroundBounce;
    }
    public void UpdateBuffer(List<input> inputBuffer)
    {
        buffer = inputBuffer;
    }

    private void Update()

    {   // OnTriggerEnter 
        // OnColliderEnter
        // Update
        // Cette ligne est pour empêcher qu'il y ait un bug d'animation au moment où le perso joue une action pile à la frame ou le perso termine son action précédente
        if (canEndAction == false)
            canEndAction = true;

        // Input
        if (state == CharacterState.Hit)
        {
            UpdateKnockback();
        }
        else
        {
            UpdateControls();
            ApplyGravity();
        }

        // Cette ligne est pour empêcher qu'il y ait un bug d'animation au moment où le perso joue une action pile à la frame ou le perso termine son action précédente
        if (endAction == true)
            EndAction();
    }

    private void ResetJump()
    {
        currentNumberOfJumps = numberOfJumps;
        direction = (int)Mathf.Sign(currentSpeedX);
    }

    void UpdateControls()
    {
        CheckJump(buffer);
        CheckHorizontal(buffer);
    }

    void CheckJump(List<input> buffer)
    {
        for (int i = 0; i < buffer.Count; i++)
        {
            if (buffer[i].jump && currentNumberOfJumps > 0)
            {
                buffer[i].jump = false;
                --currentNumberOfJumps;
                characterCollision.Jump(jumpImpulsion);
            }
        }
    }

    void CheckHorizontal(List<input> buffer)
    {
        if(characterCollision.IsGrounded == true)
        {
            if (buffer[0].horizontal != 0)
            {
                currentSpeedX += buffer[0].horizontal * acceleration;
                direction = (int) Mathf.Sign(currentSpeedX);
            }
            else
            {
                currentSpeedX -= (decceleration * direction);
                if (currentSpeedX <= decceleration && currentSpeedX >= -decceleration)
                    currentSpeedX = 0;
            }
        }
        else
        {
            if (buffer[0].horizontal != 0)
            {
                currentSpeedX += buffer[0].horizontal * (acceleration - airFriction);
            }
            else
            {
                currentSpeedX *= airStop;
                if (currentSpeedX <= airFriction && currentSpeedX >= -airFriction)
                    currentSpeedX = 0;
            }
        }
        currentSpeedX = Mathf.Clamp(currentSpeedX, -speedMax, speedMax);
        characterCollision.MoveX(currentSpeedX);

    }










    private void ApplyGravity()
    {
        if (characterCollision.IsGrounded == true)
        {
            //currentSpeedY = 0;
            return;
        }
        characterCollision.ApplyGravity(gravityForce, gravityForceMax);
    }




    // ==========================================================================================================
    //    A C T I O N
    // ==========================================================================================================


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
    // Uniquement utilisable pour dire d'arrêter l'action
    public void CanEndAction()
    {
        if(canEndAction == true)
        {
            endAction = true;
        }
    }

    private void EndAction()
    {
        if (currentAttackController != null)
            currentAttackController.ActionEnd();
    }



    // ==========================================================================================================
    //    A N I M A T I O N
    // ==========================================================================================================

    public void SetCharacterMotionSpeed(float newSpeed, float time = 0)
    {
        characterCollision.CharacterMotionSpeed = newSpeed;
        characterAnimator.speed = newSpeed;
        /*if (currentAttackController != null)
            currentAttackController.AttackMotionSpeed(newSpeed);*/
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
        /*if (currentAttackController != null)
            currentAttackController.AttackMotionSpeed(characterMotionSpeed);*/
    }


    public float GetMotionSpeed()
    {
        return characterAnimator.speed;
    }













    // ==========================================================================================================
    //    K N O C K B A C K
    // ==========================================================================================================

    private void OnTriggerEnter(Collider other)
    {
        //Attaque ennemi détecté
        if(other.tag != this.transform.tag)
        {
            Knockback(other.GetComponent<AttackController>());
        }

    }

    private void Knockback(AttackController attack)
    {

        state = CharacterState.Hit;
        currentSpeedX = 0;
        Vector2 direction = this.transform.position - attack.transform.position;
        direction *= attack.KnockbackPower;
        knockbackPower = direction;

        attack.HasHit(this);

    }

    protected void UpdateKnockback()
    {
        if (GetMotionSpeed() == 0)
            return;
        characterCollision.Move(knockbackPower.x, knockbackPower.y);
        float reduce = knockbackPowerReduce * Time.deltaTime * GetMotionSpeed();
        knockbackPower -= new Vector2(reduce * Mathf.Sign(knockbackPower.x), reduce * Mathf.Sign(knockbackPower.y));
        if(knockbackPower.magnitude < knockbackPowerForWallBounce )
        {
            state = CharacterState.Idle;
        }
    }





    public void WallBounce(Transform collider)
    {
        if(state == CharacterState.Hit)
        {
            knockbackPower.x = -knockbackPower.x;
            //SetCharacterMotionSpeed(0, 10);
        }
    }

    public void GroundBounce(Transform collider)
    {
        if (state == CharacterState.Hit)
        {
            knockbackPower.y = -knockbackPower.y;
            //SetCharacterMotionSpeed(0, 10);
        }
    }

}
