using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        Teleporter teleporter = other.GetComponent<Teleporter>();
        if (teleporter != null)
        {
            teleporter.teleportationCredits = teleporter.teleportationCredits + 1;
            NetworkManager.Instance.SessionRunner.Despawn(GetComponent<NetworkObject>());
            //Destroy(gameObject);
        }
    }
}

