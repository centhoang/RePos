using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllCeilingSpikesControl : MonoBehaviour
{
    /* NOTE: every spikes are found to be synchronized by positioning 3.1 hor far and 0.37 ver upper/lower, 
    with speedPreFall=2, speedFall=10, movementSmoothing=2, movementSmoothingFromUnderGround=0.05 */   

    [Header("Each Spike :")]
    [SerializeField] private float speedPreFall = 0.5f;
    [SerializeField] private float speedFall = 1f;
    [SerializeField] private float movementSmoothing = 0.05f;
    [SerializeField] private float movementSmoothingFromUnderGround = 0.05f;
    [SerializeField] private float freezeTime = 1f;

    GameObject spike;

    void Awake() 
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            spike = transform.GetChild(i).gameObject;
            spike.GetComponent<CeilingSpikeControl>().speedFall = speedFall;
            spike.GetComponent<CeilingSpikeControl>().speedPreFall = speedPreFall;
            spike.GetComponent<CeilingSpikeControl>().movementSmoothing = movementSmoothing;
            spike.GetComponent<CeilingSpikeControl>().movementSmoothingFromUnderGround = movementSmoothingFromUnderGround;
            spike.GetComponent<CeilingSpikeControl>().freezeTime = freezeTime;
        }
    }
}
