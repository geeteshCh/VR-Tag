using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {
    
    public GameObject canvasToDespawn;
    private NetworkRigCustom nrc;
    public bool isCalledInStart = false;

    public GameObject uiCanvas;
    private void OnEnable()
    {
       
    }

    private void Update()
    {
        if ((OVRInput.GetDown(OVRInput.Button.Three) || Input.GetKeyDown(KeyCode.X)) && !isCalledInStart)
        {
            isCalledInStart = true;
            AssignChaserRandomly();
        }
        if(OVRInput.GetDown(OVRInput.Button.Two))
        {
            uiCanvas.SetActive(!uiCanvas.activeInHierarchy);
        }
    }

    private void AssignChaserRandomly()
    {
        if(nrc == null)
            nrc = FindObjectOfType<PlayerSpawner>().localPlayer.GetComponent<NetworkRigCustom>();

        print("Assigning Chaser "+ nrc.playerNetworkObjects.Count);
        // Generate a random index on the server/master client
        int chaserIndex = Random.Range(0, nrc.playerNetworkObjects.Count);
        print("chaser index " +chaserIndex);
        // Use an RPC to assign the chaser role based on the random index
        PlayerController pc = nrc.gameObject.GetComponent<PlayerController>();
        pc.lifeCount += 1;
        pc.HandleCatch(nrc.playerNetworkObjects[chaserIndex].Id);
        pc.RPC_RequestToDespawn(canvasToDespawn.name);
        print("chaser index " +chaserIndex);
    }

    
}