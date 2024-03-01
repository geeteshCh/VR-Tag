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

        /*if (Application.isEditor)
        {
            if(Input.GetMouseButton(1))
                transform.Rotate(Vector3.up * Input.GetAxis("Mouse X"));
            input = new Vector2( Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }*/
        
        Vector3 moveDir = new Vector3(input.x, 0, input.y).normalized;
        // Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        Vector3 targetMoveAmount = moveDir * walkSpeed;
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMovevelocity, .15f);
    }

    private void FixedUpdate()
    {
        Vector3 localMove = transform.TransformDirection(moveAmount) * Time.fixedDeltaTime;
        rigidbody.MovePosition(rigidbody.position + localMove);
    }
    
    public void ActivateTurboSpeed(float multiplier, float duration)
    {
        StopCoroutine("TurboSpeedRoutine"); // Stop the coroutine if it's already running
        StartCoroutine(TurboSpeedRoutine(multiplier, duration));
    }

    private IEnumerator TurboSpeedRoutine(float multiplier, float duration)
    {
        walkSpeed = walkSpeed * multiplier; // Increase the speed
        yield return new WaitForSeconds(duration); // Wait for the duration of the turbo speed
        walkSpeed = walkSpeed/multiplier; // Reset the speed back to normal
    }
}
