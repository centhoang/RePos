using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingSpikeControl : MonoBehaviour
{
    /* NOTE: every spikes are found to be synchronized by positioning 3.1 hor far and 0.37 ver upper/lower, 
    with speedPreFall=2, speedFall=10, movementSmoothing=2, movementSmoothingFromUnderGround=0.05 */   

    [SerializeField] public float speedPreFall = 2f;
    [SerializeField] public float speedFall = 10f;
    [SerializeField] public float movementSmoothing = 2f;
    [SerializeField] public float movementSmoothingFromUnderGround = 0.05f;
    [SerializeField] public float freezeTime = 1f;

    private Rigidbody2D rb;
    private Vector3 targetVelocity;
    private Vector3 currentVelocity = Vector3.zero;
    private bool outOfGroundFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() 
    {
        if (!outOfGroundFlag)
        {
            targetVelocity = new Vector2(rb.velocity.x, speedPreFall * Time.fixedDeltaTime * (-10f));
        }
        else
        {
            targetVelocity = new Vector2(rb.velocity.x, speedFall * Time.fixedDeltaTime * (-10f));
        }

        if (outOfGroundFlag)
        {
            freezeTime -= Time.fixedDeltaTime;
            if (freezeTime <= 0)
            {
                rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, movementSmoothing);
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
        else
        {
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, movementSmoothingFromUnderGround);
        }
    }

    private void OnTriggerExit2D(Collider2D collider) 
    {
        if (collider.CompareTag("Ground"))
        {
            outOfGroundFlag = true;
        }
    } 
}
