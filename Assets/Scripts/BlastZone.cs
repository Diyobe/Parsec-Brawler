using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastZone : MonoBehaviour
{
    [SerializeField] private GameObject BlastParticle;

    //[Serializable]
    public enum BlastzoneState
    {
        Up,
        Down,
        Left,
        Right
    }

    public BlastzoneState state;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2" || other.gameObject.tag == "Player3" || other.gameObject.tag == "Player4")
        {
            InstantiateBlast(other.transform.position);
            GameManager.Instance.BlastCharacter(other.gameObject);
        }
    }

    void InstantiateBlast(Vector3 position)
    {
        float particleRotation;

        if (state == BlastzoneState.Up)
        {
            particleRotation = -90f;
        }
        else if (state == BlastzoneState.Down)
        {
            particleRotation = 90f;
        }
        else if (state == BlastzoneState.Left)
        {
            particleRotation = 0f;
        }
        else
        {
            particleRotation = 180f;
        }

        GameObject blast = 
        Instantiate(BlastParticle, position, Quaternion.Euler(new Vector3(BlastParticle.transform.rotation.x, BlastParticle.transform.rotation.y, particleRotation)));

        Destroy(blast, 5.0f);
    }
}
