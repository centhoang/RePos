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
    [SerializeField] private int numberOfWave = 5;
    [SerializeField] private float delayBetweenEachWave = 2f;
    [SerializeField] private float timeWaitForWave = 3f;
    [SerializeField] private Transform positionUsingCeiling;
    [SerializeField] private float timeWaitForCeiling = 10f;
    [SerializeField] private Transform positionUsingFence;
    [SerializeField] private int numberOfFence = 2;
    [SerializeField] private float delayBetweenEachFence = 5f;
    [SerializeField] private Transform posSpawnFenceUnder;
    [SerializeField] private Transform posSpawnFenceUpper;
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
    private bool usedSkill = false;

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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            startBattle = true;
        }
    }

    private void FixedUpdate() 
    {
        if (startBattle)
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

                    if (!usedSkill)
                    {
                        StartCoroutine(SpawnSpikeWave());
                        usedSkill = true;
                    }

                    waitTimer -= Time.fixedDeltaTime;
                    if (waitTimer <= 0)
                    {
                        stageOrder++;
                        waitTimer = timeWaitForCeiling;
                        allowToMove = true;
                        usedSkill = false;
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

                    if (!usedSkill)
                    {
                        Instantiate(ceilingSpike);
                        usedSkill = true;
                    }

                    waitTimer -= Time.fixedDeltaTime;
                    if (waitTimer <= 0)
                    {
                        stageOrder++;
                        waitTimer = timeWaitForFence;
                        allowToMove = true;
                        usedSkill = false;
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

                    if (!usedSkill)
                    {
                        StartCoroutine(SpawnSpikeFence());
                        usedSkill = true;
                    }

                    waitTimer -= Time.fixedDeltaTime;
                    if (waitTimer <= 0)
                    {
                        stageOrder++;
                        waitTimer = timeWaitForForest;
                        allowToMove = true;
                        usedSkill = false;
                    }
                }
            }
            else if (stageOrder == 4)
            {
                if (allowToMove)
                {
                    MoveToPosition(positionUsingForest);
                }
                
                if (rb.position == target)
                {
                    allowToMove = false;

                    if (!usedSkill)
                    {
                        Instantiate(theForest);
                        usedSkill = true;
                    }

                    waitTimer -= Time.fixedDeltaTime;
                    if (waitTimer <= 0)
                    {
                        stageOrder = 1;
                        waitTimer = timeWaitForWave;
                        allowToMove = true;
                        usedSkill = false;
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

    IEnumerator SpawnSpikeWave()
    {
        for (int i = 0; i < numberOfWave; i++)
        {
            Instantiate(spikeWave);
            yield return new WaitForSeconds(delayBetweenEachWave);
        }
    }

    IEnumerator SpawnSpikeFence()
    {
        for (int i = 0; i < numberOfFence; i++)
        {
            if (i%2 != 0)
            {
                Vector2 posSpawn = new Vector2(posSpawnFenceUpper.position.x, posSpawnFenceUpper.position.y);
                Instantiate(spikeFence, posSpawn, Quaternion.identity);
            }
            else
            {
                Vector2 posSpawn = new Vector2(posSpawnFenceUnder.position.x, posSpawnFenceUnder.position.y);
                Instantiate(spikeFence, posSpawn, Quaternion.identity);
            }

            yield return new WaitForSeconds(delayBetweenEachFence);
        }
    }
}
