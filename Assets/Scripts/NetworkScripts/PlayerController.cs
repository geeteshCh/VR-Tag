using System;
using Fusion;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : NetworkBehaviour {
    [Networked, OnChangedRender(nameof(UpdatePlayerAppearance))] public NetworkBool isChaser { get; set; }

    public Material chaserMaterial;
    public Material runnerMaterial;
    public Renderer playerRenderer;
    
    private NetworkObject currentChaser;
    private NetworkObject newChaser;
    
    [Networked]
    public int lifeCount { get; set; }

    public NetworkObject noToDestroy;
    public float lastTime = -1;
    private LocalXRRigCustom lrc;

    private void Start()
    {
        lifeCount = 3;
    }

    public bool GotCaughtAndDied()
    {
        if (Time.time - lastTime < 1)
            return false;

        lastTime = Time.time;
        
        Debug.Log("Got Caught Called to Remove One Life");
        lifeCount -= 1;

        if (lifeCount < 0)
        {
            string playerName = GetComponent<NetworkRigCustom>().FetchPlayerName();
            Debug.Log(" Player :: " + playerName + " Out of the Game Now");
            RPC_RequestToDespawnNO(GetComponent<NetworkObject>());
            FindObjectOfType<PlayerSpawner>().diedScreen.SetActive(true);
            return true;
        }

        return false;
    }
    
    // Method to change player appearance based on their role
    public void UpdatePlayerAppearance() {
        playerRenderer.material = isChaser ? chaserMaterial : runnerMaterial;
    }

    // RPC to request a role change for the caught player
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_RequestRoleChange(NetworkId caughtPlayerId) {
        // This RPC is called by the chaser to change the role of the caught player
        NetworkObject.NetworkUnwrap(NetworkManager.Instance.SessionRunner, caughtPlayerId, ref newChaser);
        if (newChaser) {
            var caughtPlayerController = newChaser.GetComponent<PlayerController>();
            if (caughtPlayerController != null) {
                bool died = caughtPlayerController.GotCaughtAndDied();
                
                if (died)
                {
                    return;
                }
                else
                {
                    // The previous chaser becomes a runner
                    this.isChaser = false;
                    // The caught player becomes the chaser
                    caughtPlayerController.isChaser = true;
                    caughtPlayerController.MoveItRandom();
                }   
            }
        }
    }

    private void MoveItRandom()
    {
        if (!lrc)
            lrc = FindObjectOfType<LocalXRRigCustom>();

        lrc = FindObjectOfType<LocalXRRigCustom>();
        Vector3 v = new Vector3(Random.Range(3, 12), 27, Random.Range(2, 10));
        lrc.gameObject.transform.SetPositionAndRotation(v, Quaternion.identity);
    }

    // Local method to handle catch detection and initiate role change
    public void HandleCatch(NetworkId caughtPlayerId) {
        
        Debug.Log("Handle Catch Called");
        if (Object.HasStateAuthority) {
            // The chaser initiates the RPC to change roles
            RPC_RequestRoleChange(caughtPlayerId);
        }
    }
    
    
    // RPC to request a role change for the caught player
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_RequestToDespawn(string name) {
        
        // This RPC is called by the chaser to change the role of the caught player
        Debug.Log("RPC Called for Local GO " +name);
        GameObject.Find(name)?.SetActive(false);

    }
    
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_RequestToDespawnNO(NetworkId objectId) {
        
        // This RPC is called by the chaser to change the role of the caught player
        Debug.Log("RPC Called To Disable RPC" +objectId);
        if (Object.Id != objectId)
        {
            NetworkObject.NetworkUnwrap(NetworkManager.Instance.SessionRunner, objectId, ref noToDestroy);
        }
        else
        {
            noToDestroy = Object;
        }
        
        NetworkManager.Instance.SessionRunner.Despawn(noToDestroy);
    }
}