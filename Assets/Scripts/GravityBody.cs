using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    GravityAttractor _planet;
    private Rigidbody rigidbody;
    private void Awake()
    {
        _planet = GameObject.FindGameObjectWithTag("Planet").GetComponent<GravityAttractor>();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        _planet.Attract(rigidbody);
    }
}
