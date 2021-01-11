using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    private void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, 0f);
        transform.rotation = Quaternion.Euler(transform.parent.rotation.x, Quaternion.identity.y, 0);
    }
}
