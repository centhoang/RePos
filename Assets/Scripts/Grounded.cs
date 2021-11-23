using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : MonoBehaviour
{
    //[SerializeField] private LayerMask PlatformLayer;
    GameObject Player;

    private void Awake() 
    {
        Player = gameObject.transform.parent.gameObject;    
    }

    private void OnTriggerStay2D(Collider2D collider) 
    {
        if(collider.IsTouchingLayers(3))
        {
            Player.GetComponent<PlayerMovement>().isGrounded = true;
            //Debug.Log("Setting isGrounded to TRUE: " + Player.GetComponent<PlayerMovement>().isGrounded);
        }
    }

    private void OnTriggerExit2D(Collider2D collider) 
    {
        Player.GetComponent<PlayerMovement>().isGrounded = false;
        //Debug.Log("Setting isGrounded to FALSE: " + Player.GetComponent<PlayerMovement>().isGrounded);
    }
}
