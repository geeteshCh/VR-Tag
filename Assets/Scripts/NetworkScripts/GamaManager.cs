using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {
    
    public NetworkObject canvasToDespawn;
    private NetworkRigCustom nrc;
    private void Update()
    {
        if ((OVRInput.GetDown(OVRInput.Button.Two) || Input.GetKeyDown(KeyCode.X)))
        {
            AssignChaserRandomly();
        }
    }

    private void AssignChaserRandomly()
    {
        if(nrc == null)
            nrc = FindObjectOfType<PlayerSpawner>().localPlayer.GetComponent<NetworkRigCustom>();

        if (!nrc.HasStateAuthority)
            return;
        
        print("Assigning Chaser "+ nrc.playerNetworkObjects.Count);
        // Generate a random index on the server/master client
        int chaserIndex = Random.Range(0, nrc.playerNetworkObjects.Count);
        print("chaser index " +chaserIndex);
        // Use an RPC to assign the chaser role based on the random index
        nrc.gameObject.GetComponent<PlayerRoleController>().HandleCatch(nrc.playerNetworkObjects[chaserIndex].Id);
        NetworkManager.Instance.SessionRunner.Despawn(canvasToDespawn);
        print("chaser index " +chaserIndex);
    }

    
}