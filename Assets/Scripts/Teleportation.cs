using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
    private PlayerController prc;// How long the turbo speed lasts
    
    private void FetchPRC()
    {
        prc = FindObjectOfType<PlayerSpawner>().localPlayer.GetComponent<PlayerController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Teleporter teleporter = other.GetComponent<Teleporter>();
        if (teleporter != null)
        {
            teleporter.teleportationCredits += 1;
            if (prc == null)
                FetchPRC();
            prc.RPC_RequestToDespawn(gameObject.name);
        }
    }
}

