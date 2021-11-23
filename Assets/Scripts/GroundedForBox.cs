using UnityEngine;

public class GroundedForBox : MonoBehaviour
{
    [SerializeField] float m_MassUntouched = 10f;
    [SerializeField] float m_MassTouched = 1.5f;
    GameObject Box;
    private bool FixedJointedFlag = false;

    private void Awake() 
    {
        Box = gameObject.transform.parent.gameObject;    
    }

    private void Update() 
    {
        if (Box.GetComponent<FixedJoint2D>().enabled == false)
        {
            Box.GetComponent<Rigidbody2D>().mass = m_MassUntouched;
        }
        else
        {
            Box.GetComponent<Rigidbody2D>().mass = m_MassTouched;
        }
    }

    private void OnTriggerStay2D(Collider2D collider) 
    {
        if (!FixedJointedFlag)
        {
            if(collider.IsTouchingLayers(3) && Box.GetComponent<FixedJoint2D>().enabled == true)
            {
                FixedJointedFlag = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider) 
    {
        if (FixedJointedFlag)
        {
            Box.GetComponent<FixedJoint2D>().enabled = false;
            FixedJointedFlag = false;
        }
    }
}
