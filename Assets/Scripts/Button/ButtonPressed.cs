using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressed : MonoBehaviour
{
    Animator buttonAnimator; 
    public bool pressed = false;

    void Start()
    {
        buttonAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.gameObject.CompareTag("Box"))
        {
            buttonAnimator.SetBool("pressed", true);
            pressed = true;
        }        
    }

    private void OnTriggerExit2D(Collider2D collider) 
    {
        if (collider.gameObject.CompareTag("Box"))
        {
            buttonAnimator.SetBool("pressed", false);
            pressed = false;
        }  
    }
}
