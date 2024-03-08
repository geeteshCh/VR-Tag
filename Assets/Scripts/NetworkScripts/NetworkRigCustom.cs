using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Fusion;
using TMPro;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class NetworkRigCustom : NetworkBehaviour
{
    public bool IsLocalNetworkRig => Object.HasStateAuthority;

    [Header("RigVisuals")]
    [SerializeField]
    private GameObject[] _localVisuals;

    [SerializeField] private TextMeshProUGUI playerNameText;
    #region RigComponents
    [Header("RigComponents")]
    [SerializeField]
    private NetworkTransform _characterTransform;

    [SerializeField]
    private NetworkTransform _headTransform;

    [SerializeField]
    private NetworkTransform _leftHandTransform;
    [SerializeField]
    private NetworkTransform _rightHandTransform;
    
  
    #endregion

    LocalXRRigCustom _localXRRig;
    
        
   
    [Networked, OnChangedRender(nameof(OnUserNameChanged)),Capacity(20)]
    public NetworkString<_32> userName { get; set; }
    public List<NetworkObject> playerNetworkObjects { get; } = new();

    

    void OnUserNameChanged()
    {
        playerNameText.text = userName.ToString();
    }

    public string FetchPlayerName()
    {
        return playerNameText.text;
    }
    
    public override void Spawned()
    {
        if (IsLocalNetworkRig)
        {
            _localXRRig = FindObjectOfType<LocalXRRigCustom>();
            if(_localXRRig == null)
            {
                Debug.LogError("Missing Hardware Rig in the Scene");
            }

            foreach (var v in _localVisuals)
            {
                v.SetActive(false);
            }
            Debug.Log("Rotation ::: " + transform.rotation.eulerAngles);
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.zero - transform.position);
            transform.rotation = targetRotation;
            //_localXRRig.transform.SetPositionAndRotation(transform.position,targetRotation);
            userName = PlayerPrefs.GetString("username");
            playerNetworkObjects.Add(Object);
        }
        else
        {
            Debug.Log("This is a client object");
            OnUserNameChanged();
        }
        

        if (IsLocalNetworkRig)
        {
            print("Namesss Added "+ NetworkManager.Instance.SessionRunner.SessionInfo.PlayerCount.ToString());
            //GetComponent<ColorSync>().SetRandomColor();
        }
        playerNetworkObjects.Add(Object);
       
    }

    private void OnDestroy()
    {
        Debug.Log("//Destroy");

    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        Debug.Log("Despawned");
    }


    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

       
        if(GetInput<XRRigInputData>(out var inputData))
        {
            _characterTransform.transform.SetPositionAndRotation(inputData.CharacterPosition, inputData.CharacterRotation);
            _headTransform.transform.SetPositionAndRotation(inputData.HeadsetPosition, inputData.HeadsetRotation);
            _leftHandTransform.transform.SetPositionAndRotation(inputData.LeftHandPosition, inputData.LeftHandRotation);
            _rightHandTransform.transform.SetPositionAndRotation(inputData.RightHandPosition, inputData.RightHandRotation);
        }
        
       
    }
    public override void Render()
    {
        base.Render();
        if (IsLocalNetworkRig)
        {
            _headTransform.transform.SetPositionAndRotation(_localXRRig._headTransform.position, _localXRRig._headTransform.rotation);

            _characterTransform.transform.SetPositionAndRotation(_localXRRig._characterTransform.position, _localXRRig._characterTransform.rotation);

            _leftHandTransform.transform.SetPositionAndRotation(_localXRRig._leftHandTransform.position, _localXRRig._leftHandTransform.rotation);

            _rightHandTransform.transform.SetPositionAndRotation(_localXRRig._rightHandTransform.position, _localXRRig._rightHandTransform.rotation);

        }
    }
    
}
