using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    
    public static NetworkManager Instance { get; set; }
    
    public NetworkRunner SessionRunner { get; set; }

    [SerializeField] private GameObject _runnerPrefab;
    
    
    [Header("Session List")] 
    public List<SessionInfo> _sessions = new List<SessionInfo>();
    public Transform _sessionsListContent;
    public GameObject sessionEntryPrefab;


   

    private void Awake()
    {
        if (Instance != null)
        {
            /*this.SessionRunner = Instance.SessionRunner;
            Destroy(Instance);
            Instance = this;*/
            
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

    }

    void Start()
    {
        if (SessionRunner == null)
        {
            CreateRunner();
            SessionRunner.JoinSessionLobby(SessionLobby.Shared); 
        }
        //StartSharedSession();
    }

    void Update()
    {
    }
    
    void PlayerAdded()
    {
        print("Player Added in the List");
    }
    
    public async void StartSharedSession(string SessionName="dummy")
    {
        print("SessionName :: "+SessionName);
        await Connect(SessionName.ToUpper());
       
    }
    
    public void RefereshSessionListUI()
    {
        print("Refresh List");
        foreach (Transform child in _sessionsListContent)
        {
            Destroy(child.gameObject);   
        }
        
        foreach (var sessionInfo in _sessions)
        {
           
            if (sessionInfo.IsVisible)
            {
                GameObject entry = Instantiate(sessionEntryPrefab, _sessionsListContent);
                SessionListPrefab script = entry.GetComponent<SessionListPrefab>();
                script.sessionName.text = sessionInfo.Name;
                script.playerCount.text = sessionInfo.PlayerCount + "/" + sessionInfo.MaxPlayers;

                if (!sessionInfo.IsOpen || sessionInfo.PlayerCount >= sessionInfo.MaxPlayers)
                {
                    script.joinButton.interactable = false;
                }
                else
                {
                    script.joinButton.interactable = true;
                }
            }
        }
    }
    
    
    public async Task LoadScene(int sceneIdx = 1)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIdx);

        while (!asyncLoad.isDone)
        {
            await Task.Yield();
        }
    }
    
    public void RegisterNetworkObjects(NetworkObject[] networkObjectArray)
    {
        SceneRef sceneRef = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        SessionRunner.RegisterSceneObjects(sceneRef, networkObjectArray);
    }

    private async Task Connect(string SessionName , int playerCount = 10)
    {
        
        var result = await SessionRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = SessionName,
            PlayerCount = playerCount,
        });
        
        if (result.Ok)
        {
            print("Start Game Successful");
            LoadScene();
        }
        else
        {
            Debug.LogError((result.ErrorMessage));
        }
    }
    
    private void CreateRunner()
    {
        SessionRunner = Instantiate(_runnerPrefab, transform).GetComponent<NetworkRunner>();
        SessionRunner.AddCallbacks(this);
    }
    
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("Player Joined :: " + player.PlayerId);
         
    }
    
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("Player Left :: " + player.PlayerId);
        
    }
    
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        
    }
    
    
    
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
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
        print("SessionList Updated");

        foreach (var VARIABLE in sessionList)
        {
            print("Session Name ::: "+VARIABLE.Name);
        }

        _sessions = sessionList;
        
        RefereshSessionListUI();
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
