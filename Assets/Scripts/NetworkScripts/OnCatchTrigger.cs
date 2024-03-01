using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class OnCatchTrigger : MonoBehaviour
{
    public PlayerController playerRoleController;
    public LayerMask layerToTrigger;

    private void OnEnable()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter Outside " +other.gameObject.layer + "  "+layerToTrigger  );
        if (other.gameObject.layer == 7)
        {   
            Debug.Log("Trigger Enter Inside");
            NetworkObject n = other.transform.root.GetComponent<NetworkObject>();
            if (n?.HasStateAuthority == false)
            {
               print(n.gameObject.GetComponent<NetworkRigCustom>().userName + " Got Caught ");
               
               if(playerRoleController == null)
                   playerRoleController = FindObjectOfType<PlayerSpawner>().localPlayer.GetComponent<PlayerController>();
               playerRoleController.HandleCatch(n.Id);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
