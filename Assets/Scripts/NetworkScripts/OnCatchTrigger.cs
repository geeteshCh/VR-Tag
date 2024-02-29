using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class OnCatchTrigger : MonoBehaviour
{

    public LayerMask layerToTrigger;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == layerToTrigger)
        {
            if (other.transform.root.GetComponent<NetworkObject>()?.HasStateAuthority == false)
            {
               
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
