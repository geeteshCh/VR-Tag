using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;

public class ForceAttractor : MonoBehaviour
{
    private PlayerController prc;// How long the turbo speed lasts
    public AudioSource asc;
    private void FetchPRC()
    {
        prc = FindObjectOfType<PlayerSpawner>().localPlayer.GetComponent<PlayerController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        //This is for local Player so they have all the state authority for their NetworkBehaviours
        if (other.gameObject.layer == 8)
        {
            if (prc == null)
                FetchPRC();
            prc.gameObject.GetComponent<NetworkForceAttractor>().RPC_RequestToApplyForce();
            prc.RPC_RequestToDespawn(gameObject.name);
            asc.Play();
        }
    }
    
    
}

