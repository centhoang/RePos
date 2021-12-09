using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikyControl : MonoBehaviour
{
    [SerializeField] private bool startBattle = false;

    [Header("Spiky's :")]
    [SerializeField] private float speed = 30f;
    [Space]
    [SerializeField] private Transform positionUsingWave;
    [SerializeField] private float timeWaitForWave = 3f;
    [SerializeField] private Transform positionUsingCeiling;
    [SerializeField] private float timeWaitForCeiling = 10f;
    [SerializeField] private Transform positionUsingFence;
    [SerializeField] private float timeWaitForFence = 7f;
    [SerializeField] private Transform positionUsingForest;
    [SerializeField] private float timeWaitForForest = 12f;

    [Header("Skill's prefabs :")]
    [SerializeField] private GameObject spikeWave;
    [SerializeField] private GameObject ceilingSpike;
    [SerializeField] private GameObject spikeFence;
    [SerializeField] private GameObject theForest;


    private Animator animator; 
    private Rigidbody2D rb;
    private Vector2 newPos;
    private Vector2 target;
    private float waitTimer;
    private int stageOrder = 1;
    private bool allowToMove = true;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = gameObject.transform.parent.gameObject.transform.parent.GetComponent<Rigidbody2D>();

        waitTimer = timeWaitForWave;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() 
    {
        if (startBattle)
        {
            while (true)
            {
                if (stageOrder == 1)
                {
                    if (allowToMove)
                    {
                        MoveToPosition(positionUsingWave);
                    }
                    
                    if (rb.position == target)
                    {
                        allowToMove = false;

                        waitTimer -= Time.fixedDeltaTime;
                        if (waitTimer <= 0)
                        {
                            stageOrder++;
                            waitTimer = timeWaitForCeiling;
                            allowToMove = true;
                        }
                    }
                }
                else if (stageOrder == 2)
                {
                    if (allowToMove)
                    {
                        MoveToPosition(positionUsingCeiling);
                    }
                    
                    if (rb.position == target)
                    {
                        allowToMove = false;

                        waitTimer -= Time.fixedDeltaTime;
                        if (waitTimer <= 0)
                        {
                            stageOrder++;
                            waitTimer = timeWaitForFence;
                            allowToMove = true;
                        }
                    }
                }
                else if (stageOrder == 3)
                {
                    if (allowToMove)
                    {
                        MoveToPosition(positionUsingFence);
                    }
                    
                    if (rb.position == target)
                    {
                        allowToMove = false;

                        waitTimer -= Time.fixedDeltaTime;
                        if (waitTimer <= 0)
                        {
                            stageOrder++;
                            waitTimer = timeWaitForForest;
                            allowToMove = true;
                        }
                    }
                }
                else if (stageOrder == 4)
                {
                    if (allowToMove)
                    {
                        MoveToPosition(positionUsingFence);
                    }
                    
                    if (rb.position == target)
                    {
                        allowToMove = false;

                        waitTimer -= Time.fixedDeltaTime;
                        if (waitTimer <= 0)
                        {
                            stageOrder = 1;
                            waitTimer = timeWaitForWave;
                            allowToMove = true;
                        }
                    }
                }
            }
        }
    }

    private void MoveToPosition (Transform newTransform)
    {
        target = new Vector2 (newTransform.position.x, newTransform.position.y);
        newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }
}
