using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CharacterCollision : MonoBehaviour
{
    public delegate void Action();
    public event Action doAction;

    public delegate void ActionCollision(Transform transform);
    public event ActionCollision OnWallCollision;
    public event ActionCollision OnGroundCollision;

    [Header("CharacterController")]
    [SerializeField]
    protected BoxCollider characterCollider;


    [Header("Collision")]
    [SerializeField]
    protected bool collision = true;

    //[HorizontalGroup("RaycastOffset")]
    [SerializeField]
    protected float offsetRaycastX = 0.0001f;
    //[HorizontalGroup("RaycastOffset")]
    [SerializeField]
    protected float offsetRaycastY = 0.0001f;

    //[HorizontalGroup("RaycastNumber")]
    [SerializeField]
    protected int numberRaycastVertical = 2;
    //[HorizontalGroup("RaycastNumber")]
    [SerializeField]
    protected int numberRaycastHorizontal = 2;


    protected float characterMotionSpeed = 1;
    public float CharacterMotionSpeed
    {
        get { return characterMotionSpeed; }
        set { characterMotionSpeed = value; }
    }


    protected float speedX = 0;
    public float SpeedX
    {
        get { return speedX; }
        set { speedX = value; }
    }

    protected float speedY = 0;
    public float SpeedY
    {
        get { return speedY; }
        set { speedY = value; }
    }

    protected float actualSpeedX = 0;
    protected float actualSpeedY = 0;


    int layerMask;
    Vector2 bottomLeft;
    Vector2 upperLeft;
    Vector2 bottomRight;
    Vector2 upperRight;

    Transform collisionInfo;

    private bool isGrounded = false;
    public bool IsGrounded
    {
        get { return isGrounded; }
        set { isGrounded = value; }
    }


    protected void Update()
    {
        UpdateCollision();
    }

    protected void UpdateCollision()
    {
        actualSpeedX = speedX;
        actualSpeedY = speedY;
        actualSpeedX *= characterMotionSpeed;
        actualSpeedY *= characterMotionSpeed;

        if (collision == true)
        {
            layerMask = 1 << 8;

            bottomLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.min.y);
            upperLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.max.y);
            bottomRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.min.y);
            upperRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.max.y);

            UpdatePositionX();
            transform.position = new Vector3(transform.position.x + (actualSpeedX * Time.deltaTime), transform.position.y, 0);

            bottomLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.min.y);
            upperLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.max.y);
            bottomRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.min.y);
            upperRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.max.y);

            UpdatePositionY();
            transform.position = new Vector3(transform.position.x, transform.position.y + (actualSpeedY * Time.deltaTime), 0);
        }

        //transform.position = new Vector3(transform.position.x + (actualSpeedX * Time.deltaTime), transform.position.y + (actualSpeedY * Time.deltaTime), 0);
    }

    private void UpdatePositionX()
    {

        RaycastHit raycastX;
        Vector2 originRaycast;

        if (actualSpeedX < 0)
        {
            // ======================================================================================================
            originRaycast = bottomLeft;// - new Vector2(offsetRaycastX, 0);
            for (int i = 0; i < numberRaycastHorizontal; i++)
            {
                Physics.Raycast(originRaycast, new Vector2(actualSpeedX * Time.deltaTime, 0), out raycastX, Mathf.Abs(actualSpeedX * Time.deltaTime) + offsetRaycastX, layerMask);
                Debug.DrawRay(originRaycast, new Vector2(actualSpeedX * Time.deltaTime, 0), Color.red);
                if (raycastX.collider != null)
                {
                    collisionInfo = raycastX.collider.transform;
                    float distance = raycastX.point.x - bottomLeft.x;
                    distance += offsetRaycastX;
                    actualSpeedX = distance / Time.deltaTime;
                    OnWallCollision.Invoke(collisionInfo);
                    return;
                }
                originRaycast += new Vector2(0, Mathf.Abs(upperLeft.y - bottomLeft.y) / (numberRaycastHorizontal - 1));
            }
            // ======================================================================================================

        }
        else if (actualSpeedX > 0)
        {
            // ======================================================================================================
            originRaycast = bottomRight;// + new Vector2(offsetRaycastX, 0);
            for (int i = 0; i < numberRaycastHorizontal; i++)
            {
                Physics.Raycast(originRaycast, new Vector2(actualSpeedX * Time.deltaTime, 0), out raycastX, Mathf.Abs(actualSpeedX * Time.deltaTime) + offsetRaycastX, layerMask);
                Debug.DrawRay(originRaycast, new Vector2(actualSpeedX * Time.deltaTime, 0), Color.red);
                if (raycastX.collider != null)
                {
                    collisionInfo = raycastX.collider.transform;
                    float distance = raycastX.point.x - bottomRight.x;
                    distance -= offsetRaycastX;
                    actualSpeedX = distance / Time.deltaTime;
                    OnWallCollision.Invoke(collisionInfo);
                    return;
                }
                originRaycast += new Vector2(0, Mathf.Abs(upperRight.y - bottomRight.y) / (numberRaycastHorizontal - 1));
            }
            // ======================================================================================================

        }
    }


    private void UpdatePositionY()
    {
        RaycastHit raycastY;
        Vector2 originRaycast;

        if (actualSpeedY < 0)
        {
            // ======================================================================================================
            originRaycast = bottomLeft;// - new Vector2(0, offsetRaycastY);
            for (int i = 0; i < numberRaycastVertical; i++)
            {
                Physics.Raycast(originRaycast, new Vector2(0, actualSpeedY * Time.deltaTime), out raycastY, Mathf.Abs(actualSpeedY * Time.deltaTime) + offsetRaycastY, layerMask);
                Debug.DrawRay(originRaycast, new Vector2(0, actualSpeedY * Time.deltaTime + offsetRaycastY), Color.red);
                if (raycastY.collider != null)
                {
                    collisionInfo = raycastY.collider.transform;
                    float distance = raycastY.point.y + offsetRaycastY - bottomLeft.y;
                    //distance += offsetRaycastY;
                    actualSpeedY = distance / Time.deltaTime;

                    // A dégager pour plus de clareté
                    if (!isGrounded)
                    {
                        doAction.Invoke();
                    }
                    isGrounded = true;
                    OnGroundCollision.Invoke(collisionInfo);
                    return;
                }
                originRaycast += new Vector2(Mathf.Abs(bottomRight.x - bottomLeft.x) / (numberRaycastVertical - 1), 0);
            }
            isGrounded = false;
            // ======================================================================================================

        }
        else if (actualSpeedY > 0)
        {
            isGrounded = false;
            // ======================================================================================================
            originRaycast = upperLeft;// + new Vector2(0, offsetRaycastY);
            for (int i = 0; i < numberRaycastVertical; i++)
            {
                Physics.Raycast(originRaycast, new Vector2(0, actualSpeedY * Time.deltaTime), out raycastY, Mathf.Abs(actualSpeedY * Time.deltaTime) + offsetRaycastY, layerMask);
                Debug.DrawRay(originRaycast, new Vector2(0, actualSpeedY * Time.deltaTime), Color.yellow);
                if (raycastY.collider != null)
                {
                    collisionInfo = raycastY.collider.transform;
                    float distance = raycastY.point.y - upperLeft.y;
                    distance -= offsetRaycastY;
                    actualSpeedY = distance / Time.deltaTime;
                    OnGroundCollision.Invoke(collisionInfo);
                    return;
                }
                originRaycast += new Vector2(Mathf.Abs(upperRight.x - upperLeft.x) / (numberRaycastVertical - 1), 0);
            }
            // ======================================================================================================
        }
    }


    public void MoveX(float newSpeedX)
    {
        speedX = newSpeedX;
    }
    public void MoveY(float newSpeedY)
    {
        speedY = newSpeedY;
    }
    public void Move(float newSpeedX, float newSpeedY)
    {
        speedX = newSpeedX;
        speedY = newSpeedY;
    }



}
