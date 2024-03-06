using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
    private PlayerController prc; // How long the turbo speed lasts
    private PlayerSpawner ps;
    public AudioSource asc;
    private void FetchPRC()
    {
        ps = FindObjectOfType<PlayerSpawner>();
        prc = ps.localPlayer.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Teleporter teleporter = other.GetComponent<Teleporter>();
        if (teleporter != null)
        {
            teleporter.teleportationCredits += 1;
            if (prc == null)
                FetchPRC();
            prc.RPC_RequestToDespawn(gameObject.name);
            ps.teleportationCreditText.text = teleporter.teleportationCredits.ToString();
        }
    }
}
