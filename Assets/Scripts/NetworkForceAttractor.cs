using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class NetworkForceAttractor : NetworkBehaviour
{
    public float forceMagnitude = 1;
    public float forceDuration = 5;

    public PlayerController pc;

    
    // RPC to request a role change for the caught player
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_RequestToApplyForce()
    {
        // This RPC is called by the chaser to change the role of the caught player
        Debug.Log("RPC To APPLY FORCE");
        ApplyForceFromPowerUp(pc.isChaser);
    }
    
    public void ApplyForceFromPowerUp(bool isAttracting)
    {
        GameObject g = FindObjectOfType<GravityBody>().gameObject;
        StartCoroutine(ApplyForceOverTime(g, isAttracting));
    }

    public IEnumerator ApplyForceOverTime(GameObject forceVictim, bool isAttracting)
    {
        float elapsed = 0;
        while (elapsed < forceDuration)
        {
            if (pc.isChaser != isAttracting)
                break;
            
            Vector3 forceDirection;
            if (isAttracting)
            {
                // Attract towards the chaser
                forceDirection = (transform.position - forceVictim.transform.position).normalized;
            }
            else
            {
                // Repulse away from the runner
                forceDirection = (forceVictim.transform.position-transform.position).normalized;
            }
            forceVictim.GetComponent<Rigidbody>().AddForce(forceDirection * forceMagnitude);
            elapsed += Time.deltaTime;
            Debug.Log("adding force");
            yield return null;
        }
        
    }
}
