﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;

public delegate void ActionPlayerController(PlayerController playerController);
public class BlastZone : MonoBehaviour
{

    public event ActionPlayerController OnBlast;

    [SerializeField] private GameObject BlastParticle;
    public enum BlastzoneState
    {
        Up,
        Down,
        Left,
        Right
    }

    public BlastzoneState state;

    public AudioClip explosion;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2" || other.gameObject.tag == "Player3" || other.gameObject.tag == "Player4")
        {
            InstantiateBlast(other.transform.position);
            OnBlast.Invoke(other.GetComponent<PlayerController>());
        }
    }

    void InstantiateBlast(Vector3 position)
    {
        float particleRotation;

        TengenToppaAudioManager.Instance.PlaySound(explosion, 0.5f);

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
