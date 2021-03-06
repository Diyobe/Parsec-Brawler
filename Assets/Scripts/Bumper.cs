﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;


public class Bumper : MonoBehaviour
{
    [SerializeField]
    float bumperForce;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2" || other.gameObject.tag == "Player3" || other.gameObject.tag == "Player4")
        {
            GetComponent<Animator>().SetTrigger("Feedback");
            PlayerController player = other.GetComponent<PlayerController>();
            Vector2 direction = player.ParticlePoint.position - this.transform.position;
            direction *= bumperForce;
            player.AddForce(direction.x, direction.y);
        }
    }
}
