using System;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using Unity.VisualScripting;

public class LocalXRRigCustom : MonoBehaviour,INetworkRunnerCallbacks
{
    #region RigComponents
    [Header("Rig Components")]
    public Transform _characterTransform;

    public Transform _headTransform;

    public Transform _leftHandTransform;
    
    public Transform _rightHandTransform;
    #endregion

    // Start is called before the first frame update
    void OnEnable()
    {
        NetworkManager.Instance.SessionRunner.AddCallbacks(this);
    }
    

    #region RunnerCallbacks
    void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, NetworkInput input)
    {
        XRRigInputData inputData = new XRRigInputData();

        inputData.HeadsetPosition = _headTransform.position;
        inputData.HeadsetRotation = _headTransform.rotation;

        inputData.CharacterPosition = _characterTransform.position;
        inputData.CharacterRotation = _characterTransform.rotation;

        inputData.LeftHandPosition = _leftHandTransform.position;
        inputData.LeftHandRotation = _leftHandTransform.rotation;
        
        inputData.RightHandPosition = _rightHandTransform.position;
        inputData.RightHandRotation = _rightHandTransform.rotation;
        
        input.Set(inputData);
    }
    #endregion

    #region UnusedRunnerCallbacks
    void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner)
    {
        
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        
    }

    void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    void INetworkRunnerCallbacks.OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {

    }

    void INetworkRunnerCallbacks.OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {

    }
    

    void INetworkRunnerCallbacks.OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {

    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        
    }


    void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {

    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    void INetworkRunnerCallbacks.OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {

    }

    void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }


    void INetworkRunnerCallbacks.OnSceneLoadDone(NetworkRunner runner)
    {

    }

    void INetworkRunnerCallbacks.OnSceneLoadStart(NetworkRunner runner)
    {

    }

    void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {

    }

    void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }

    void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {

    }
    #endregion

}

public struct XRRigInputData : INetworkInput
{
    //Head
    public Vector3 HeadsetPosition;
    public Quaternion HeadsetRotation;

    //Body
    public Vector3 CharacterPosition;
    public Quaternion CharacterRotation;
    
    //Hands
    public Vector3 LeftHandPosition;
    public Quaternion LeftHandRotation;

    public Vector3 RightHandPosition;
    public Quaternion RightHandRotation;

}