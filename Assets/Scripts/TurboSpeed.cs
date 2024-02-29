using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurboSpeed : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public float speedMultiplier = 2.0f; // How much to multiply the speed by
    public float duration = 5.0f; // How long the turbo speed lasts

    private void OnTriggerEnter(Collider other)
    {
        FirstPersonController firstPersonController = other.GetComponent<FirstPersonController>();
        if (firstPersonController != null)
        {
            firstPersonController.ActivateTurboSpeed(speedMultiplier, duration);
            Destroy(gameObject); // Destroy the power-up object
        }
    }
}
