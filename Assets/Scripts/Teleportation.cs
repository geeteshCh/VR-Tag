using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        Teleporter teleporter = other.GetComponent<Teleporter>();
        if (teleporter != null)
        {
            teleporter.teleportationCredits = teleporter.teleportationCredits + 1;
            Destroy(gameObject); // Destroy the power-up item
        }
    }
}

