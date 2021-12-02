using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateControl : MonoBehaviour
{
    public bool buttonPressed = false;
    private Animator gateAnimator;
    
    private void Start() {
        gateAnimator = GetComponent<Animator>();
    }
    
    private void FixedUpdate() 
    {
        if (buttonPressed)
        {
            gateAnimator.SetBool("buttonPressed", true);
        }

        if (!buttonPressed)
        {
           gateAnimator.SetBool("buttonPressed", false); 
        }
    }
}
