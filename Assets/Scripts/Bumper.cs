using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bumper : MonoBehaviour
{

    [SerializeField]
    Vector2 bumperForce;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2" || other.gameObject.tag == "Player3" || other.gameObject.tag == "Player4")
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.AddForce(bumperForce.x, bumperForce.y);
        }
    }
}
