using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
       // Start is called before the first frame update
    public float walkSpeed = 3f;
    private Vector3 moveAmount;
    private Vector3 smoothMovevelocity;
    private Vector2 input;
    public Transform OVRRigTransform;
    Rigidbody rigidbody;
    
  
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody> ();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x);
        input = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
       
        Vector3 forwardDirection = OVRRigTransform.forward;
        Vector3 rightDirection = OVRRigTransform.right;
        
        Vector3 moveDir = (forwardDirection * input.y + rightDirection * input.x).normalized;

        
        Vector3 targetMoveAmount = moveDir * walkSpeed;
        targetMoveAmount.y /= walkSpeed;
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMovevelocity, .15f);
    }

    private void FixedUpdate()
    {
        Vector3 localMove =  moveAmount * Time.fixedDeltaTime;;
        rigidbody.MovePosition(rigidbody.position + localMove);
    }
    
    public void ActivateTurboSpeed(float multiplier, float duration)
    {
        StopCoroutine("TurboSpeedRoutine"); // Stop the coroutine if it's already running
        StartCoroutine(TurboSpeedRoutine(multiplier, duration));
    }

    private IEnumerator TurboSpeedRoutine(float multiplier, float duration)
    {
        walkSpeed = walkSpeed * multiplier/1.3f; // Increase the speed
        yield return new WaitForSeconds(duration); // Wait for the duration of the turbo speed
        walkSpeed = walkSpeed/multiplier; // Reset the speed back to normal
    }
    
}
