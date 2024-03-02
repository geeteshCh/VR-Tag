using Fusion;
using UnityEngine;

public class PlayerController : NetworkBehaviour {
    [Networked, OnChangedRender(nameof(UpdatePlayerAppearance))] public NetworkBool isChaser { get; set; }

    public Material chaserMaterial;
    public Material runnerMaterial;
    public Renderer playerRenderer;
    
    private NetworkObject currentChaser;
    private NetworkObject newChaser;
    
    private int lifeCount = 3;


    public bool GotCaughtAndDied()
    {
        Debug.Log("Got Caught Called to Remove One Life");
        lifeCount -= 1;

        if (lifeCount < 0)
        {
            string playerName = GetComponent<NetworkRigCustom>().FetchPlayerName();
            Debug.Log(" Player :: " + playerName + " Out of the Game Now");
            RPC_RequestToDespawn(GetComponent<NetworkObject>());
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
                }   
            }
        }
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
    public void RPC_RequestToDespawn(NetworkObject no) {
        // This RPC is called by the chaser to change the role of the caught player
        NetworkManager.Instance.SessionRunner.Despawn(no);
    }
}