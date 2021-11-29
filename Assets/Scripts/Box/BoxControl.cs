using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxControl : MonoBehaviour
{
    [SerializeField] float NormalMass = 15f;
    [SerializeField] float BeingPushPullMass = 2f;

    private void FixedUpdate() 
    {
        if (GetComponent<FixedJoint2D>().enabled)
        {
            GetComponent<Rigidbody2D>().mass = BeingPushPullMass;
        }
        else
        {
            GetComponent<Rigidbody2D>().mass = NormalMass;
        }
    }
}
