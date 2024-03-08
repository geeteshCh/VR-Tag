using System;
using System.Collections;
using Fusion;
using TMPro;
using UnityEditor;
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
    
    
    [Header("For Stopwatch")]
    public float timeLimit = 120f; // Countdown time limit in seconds
    
    [Networked]
    private float timeRemaining{ get; set; } // Time remaining in the countdown
    private bool isStopwatchRunning = false;
    
    
    private PlayerSpawner ps;
    
    private void Start()
    {
        lifeCount = 3;
        timeRemaining = timeLimit;
        ps = FindObjectOfType<PlayerSpawner>();

    }
    
    public bool GotCaughtAndDied()
    {
       
        StartCountdown();
        
        if (!Object.HasStateAuthority)
            return false;
        
        if (Time.time - lastTime < 1)
            return false;

        lastTime = Time.time;
        
        Debug.Log("Got Caught Called to Remove One Life");
        lifeCount -= 1;
        if (lifeCount < 3 && lifeCount >=0)
        {
            ps.lifesIcons[lifeCount].SetActive(false);
        }
        if (lifeCount < 0)
        {
            string playerName = GetComponent<NetworkRigCustom>().FetchPlayerName();
            string msgLog = " Player :: " + playerName + " Out of the Game Now";
            Debug.Log(msgLog);
            RPC_RequestToShowMessage(msgLog);
            RPC_RequestToDespawnNO(GetComponent<NetworkObject>());
            ps.diedScreen.SetActive(true);
            return true;
        }

        return false;
    }
    
    
    void FixedUpdate()
    {
        if (isStopwatchRunning && timeRemaining > 0)
        {
            // Subtract the fixed delta time from remaining time
            timeRemaining -= Time.fixedDeltaTime;
            Debug.Log($"Time Remaining: {timeRemaining} seconds");
            ps.timerText.text = ((int)timeRemaining).ToString();

            if (timeRemaining <= 0)
            {
                // Optionally, perform an action when the countdown reaches zero
                Debug.Log("Countdown finished!");
                StopCountdown();
                if (!GotCaughtAndDied())
                {
                    StartCountdown();
                }
            }
        }
    }

    // Call this method to start the countdown
    public void StartCountdown()
    {
        isStopwatchRunning = true;
        ps.timerText.transform.parent.gameObject.SetActive(true);

        timeRemaining = timeLimit;
        ps.timerText.text = timeRemaining.ToString();
    }

    // Call this method to stop the countdown
    public void StopCountdown()
    {
        isStopwatchRunning = false;
        ps.timerText.transform.parent.gameObject.SetActive(false);
    }
    
    
    // Method to change player appearance based on their role
    public void UpdatePlayerAppearance() {
        playerRenderer.material = isChaser ? chaserMaterial : runnerMaterial;
    }

    // RPC to request a role change for the caught player
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_RequestRoleChange(NetworkId caughtPlayerId) {
        // This RPC is called by the chaser to change the role of the caught player
        StopCountdown();
        NetworkObject.NetworkUnwrap(NetworkManager.Instance.SessionRunner, caughtPlayerId, ref newChaser);
        if (!ps)
            ps = FindObjectOfType<PlayerSpawner>();
        ps.ascOutSound.Play();
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
        Vector3 v = new Vector3(Random.Range(2, 12), 31, -7.6f);
        lrc.gameObject.transform.SetPositionAndRotation(v, Quaternion.identity);
    }

    // Local method to handle catch detection and initiate role change
    public void HandleCatch(NetworkId caughtPlayerId) {
        
        Debug.Log("Handle Catch Called");
        if (Object.HasStateAuthority)
        {
            StopCountdown();
            // The chaser initiates the RPC to change roles
            RPC_RequestRoleChange(caughtPlayerId);
        }
    }
    
    
    // RPC to request a role change for the caught player
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_RequestToDespawn(string name) {
        
        // This RPC is called by the chaser to change the role of the caught player
        Debug.Log("RPC Called for Local GO " +name);
        GameObject go = GameObject.Find(name);
        go?.SetActive(false);
        if (go.tag == "IntroCanvas")
            FindObjectOfType<FirstPersonController>().enabled = true;

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
    
    
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_RequestToShowMessage(string msg)
    {
        Debug.Log("RPC SHOW Msg");
        if (!ps)
            ps = FindObjectOfType<PlayerSpawner>();
        ps.logCanvas.SetActive(true);
        ps.logCanvas.GetComponent<TextMeshProUGUI>().text = msg;
        StartCoroutine(DisableGO(ps.logCanvas, 20));
    }

    IEnumerator DisableGO(GameObject g, float duration)
    {
        yield return new WaitForSeconds(duration);
        g.SetActive(false);
    }
}