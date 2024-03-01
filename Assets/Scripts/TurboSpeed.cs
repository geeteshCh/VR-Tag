using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class TurboSpeed : MonoBehaviour
{
    
    public float speedMultiplier = 2.0f; // How much to multiply the speed by
    public float duration = 5.0f;
    private PlayerRoleController prc;// How long the turbo speed lasts

    private void Start()
    {
        prc = FindObjectOfType<PlayerSpawner>().localPlayer.GetComponent<PlayerRoleController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        FirstPersonController firstPersonController = other.GetComponent<FirstPersonController>();
        if (firstPersonController != null)
        {
            firstPersonController.ActivateTurboSpeed(speedMultiplier, duration);
            prc.RPC_RequestToDespawn(GetComponent<NetworkObject>());
           
        }
    }
}
