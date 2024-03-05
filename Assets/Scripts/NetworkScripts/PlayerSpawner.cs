using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerSpawner : MonoBehaviour, INetworkRunnerCallbacks
{

    [SerializeField] private NetworkPrefabRef playerPrefab;
    [SerializeField] private float planetRadius;
    [SerializeField] private float minTheta, maxTheta, minPhi, maxPhi;

    public NetworkObject localPlayer;
    
    public GameObject diedScreen;
    public GameObject logCanvas;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI teleportationCreditText;
    public GameObject[] lifesIcons;
    
    
    void Start()
    {
        NetworkManager.Instance.SessionRunner.AddCallbacks(this);
        SpawnPlayer(NetworkManager.Instance.SessionRunner);
    }


    Vector3 GenerateRandomPositionInSector(float radius)
    {
        float theta = Random.Range(minTheta * Mathf.Deg2Rad, maxTheta * Mathf.Deg2Rad); // Convert degrees to radians
        float phi = Random.Range(minPhi * Mathf.Deg2Rad, maxPhi * Mathf.Deg2Rad); // Convert degrees to radians

        float x = radius * Mathf.Sin(phi) * Mathf.Cos(theta);
        float y = radius * Mathf.Sin(phi) * Mathf.Sin(theta);
        float z = radius * Mathf.Cos(phi);

        return new Vector3(x, y, z);
    }


    private void SpawnPlayer(NetworkRunner runner)
    { 
        //Vector3 spawnPosition = GenerateRandomPositionInSector(planetRadius);
        // Assuming you have a NetworkObject component attached to your player prefab
        Vector3 spawnPosition = Vector3.up;
        localPlayer = runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity);
        print(localPlayer.Id + " LOCAL PLAYER");
    }
    
    
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        print("Player Joined Call");
        if (player == runner.LocalPlayer)
        {
            SpawnPlayer(runner);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }
    public void OnConnectedToServer(NetworkRunner runner)
    {
        
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }
}
