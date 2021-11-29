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
        if(collider.gameObject.CompareTag("Ground") || collider.gameObject.CompareTag("Box"))
        {
            Player.GetComponent<PlayerMovement>().isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider) 
    {
        if(collider.gameObject.CompareTag("Ground") || collider.gameObject.CompareTag("Box"))
        {
            Player.GetComponent<PlayerMovement>().isGrounded = false;
        }
    }
}
