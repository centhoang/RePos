using UnityEngine;

public class GroundedForBox : MonoBehaviour
{
    GameObject Box;
    public static GroundedForBox instance;
    public bool isGrounded = false;

    private void Awake() 
    {
        Box = gameObject.transform.parent.gameObject;
        instance = this;  
    }

    private void OnTriggerStay2D(Collider2D collider) 
    {
        if (collider.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider) 
    {    
        if (collider.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            Box.GetComponent<FixedJoint2D>().enabled = false;
        }
    }
}
