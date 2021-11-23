using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayerCollision : MonoBehaviour
{
    [SerializeField] float m_MassTouching = 10f;
    [SerializeField] float m_MassUnTouched = 1.5f;

    void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("touched Player layer");
            this.GetComponent<Rigidbody2D>().mass = m_MassTouching;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) 
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Exit touching Player layer");
            this.GetComponent<Rigidbody2D>().mass = m_MassUnTouched;
        }   
    }
}
