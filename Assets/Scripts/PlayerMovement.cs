using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[Header("-- Horizontal Movement --")]
    [SerializeField] private float runSpeed = 40f;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement

	[Header("-- Vertical Movement --")]
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, .5f)] [SerializeField] private float m_StillGroundedDelay = 0.1f; // Delay time counts as grounded since last touch.

	[Header("-- Grab Interact --")]
	[Range(0, 5f)] [SerializeField] private float m_GrabDistance = 1f;
	[SerializeField] private LayerMask m_BoxMask;
	[Range(0, 1f)] [SerializeField] private float m_SpeedScaleDown = 0.5f;

	[Header("-- ReverPos Skill --")]
	[SerializeField] private float recordTime = 5f;
	[SerializeField] private float timeBetweenEachCloneDrawn = 1f;
	[SerializeField] private GameObject cloneGhostPrefab;

	// Other movement's variables
    [HideInInspector] public bool isGrounded = true;                            // Whether or not the player is grounded.
	private float originRunSpeed = 40f;
    private Rigidbody2D playerRigidbody2D;
	private float horizontalMove = 0f;
	private bool jump = false;
	private float stillGroundedDelayTimer = 0f;
    private bool FacingRight = true;  
    private Vector3 m_Velocity = Vector3.zero;

	// Grab's variables
	private bool prePushPull = false;
	private bool grabbed = false;
	private bool grabbedFacingRight = true;
	private bool grabbedFlag = false;
	private GameObject box;
	private RaycastHit2D hit;

	// Reverse position skill components
	private bool isRewinding = false;
	private List<Vector3> PointInTimes;
	private List<GameObject> GhostCloneList;
	private float CloneGhostTimer = 0f;
	private int CloneInOrder = 0;

	// For Aniamtion
	private Animator playerAnimator;

    void Awake() 
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
		playerAnimator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        originRunSpeed = runSpeed;

		PointInTimes = new List<Vector3>();

		CloneGhostTimer = timeBetweenEachCloneDrawn;

		GameObject obj;
		GhostCloneList = new List<GameObject>();
		for (int i = 0; i < Mathf.RoundToInt(recordTime/timeBetweenEachCloneDrawn); i++)
		{
			obj = Instantiate(cloneGhostPrefab);
			obj.SetActive(false);
			GhostCloneList.Add(obj);
		}
    }

    // Update is called once per frame
    void Update()
    {
		// Input skill
		if (Input.GetButtonDown("ReversePos Skill"))
		{
			StartRewind();
		}
		else if (Input.GetButtonUp("ReversePos Skill"))
		{
			StopRewind();
		}

		// Input push-pull
		if (isGrounded)
		{
			if (Input.GetButtonDown("Push-Pull"))
			{
				prePushPull = true;

				hit = Physics2D.Raycast (transform.position, Vector2.right * transform.localScale.x, m_GrabDistance, m_BoxMask);
				
				try
				{
					if (hit.collider.gameObject != null && hit.collider.gameObject.tag == "Grabbable")
					{
						box = hit.collider.gameObject;
						box.GetComponent<FixedJoint2D>().enabled= true;
						box.GetComponent<FixedJoint2D>().connectedBody= this.GetComponent<Rigidbody2D>();
						grabbed = true;
					}
				}
				catch {}	
			}
			else if (Input.GetButtonUp("Push-Pull"))
			{
				try 
				{
					box.GetComponent<FixedJoint2D>().enabled = false;
				}
				catch {}
				prePushPull = false;
				grabbedFlag = false;
				grabbed = false;
			}

			if (grabbed)
			{
				if (!grabbedFlag)
				{
					grabbedFlag = true;
					grabbedFacingRight = FacingRight;
				}
			}
		}
		else
		{
			try 
			{
				box.GetComponent<FixedJoint2D>().enabled = false;
			}
			catch {}
			prePushPull = false;
			grabbedFlag = false;
			grabbed = false;
		}

		// Input horizontal movement
		if (grabbed)
		{
			runSpeed = m_SpeedScaleDown * originRunSpeed;
		}
		else
		{
			runSpeed = originRunSpeed;
		}

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		// Input vertical movement
		if (!grabbed)
		{
			if (Input.GetButtonDown("Jump"))
			{
				jump = true;
			}
		}

		// At last, check animation for current movement
		CheckAnimation(horizontalMove);
    }

    void FixedUpdate() 
    {
        Move(horizontalMove * Time.fixedDeltaTime, jump, prePushPull);
		jump = false;

		if (isRewinding)
			Rewind();
		else
			Record();
    }

	private void SpawnGhost (Vector3 position, Vector3 scale, Sprite sprite)
	{
		GameObject cloneToSpawn = GhostCloneList[CloneInOrder];
		if (cloneToSpawn)
		{
			cloneToSpawn.GetComponent<SpriteRenderer>().sprite = sprite;
			cloneToSpawn.transform.position = position;
			cloneToSpawn.transform.localScale = scale;
			cloneToSpawn.transform.rotation = Quaternion.identity;
			cloneToSpawn.SetActive(true);

			CloneInOrder++;
			if (CloneInOrder > GhostCloneList.Count - 1)
			{
				CloneInOrder = 0;
			}
		}
		else
		{
			Debug.Log("cloneToSpawn is null to clone !!!!!");
		}
	}

	void Rewind ()
	{
		if (PointInTimes.Count > 0)
		{
            transform.position = PointInTimes[0];
			playerRigidbody2D.velocity = Vector3.zero;
			PointInTimes.RemoveAt(0);

			CloneGhostTimer += Time.fixedDeltaTime;
			if (CloneGhostTimer >= timeBetweenEachCloneDrawn)
			{
				CloneInOrder --;
				if (CloneInOrder < 0)
				{
					CloneInOrder = GhostCloneList.Count - 1;
				}
				GameObject cloneToDelete = GhostCloneList[CloneInOrder];
				cloneToDelete.SetActive(false);
				CloneGhostTimer = 0f;
			}
		} 
		else
		{
			StopRewind();
		}
	}

	void Record ()
	{
		if (PointInTimes.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
		{
			PointInTimes.RemoveAt(PointInTimes.Count - 1);
		}

		PointInTimes.Insert(0, transform.position);
		
		CloneGhostTimer -= Time.fixedDeltaTime;
		if (CloneGhostTimer <= 0)
		{
			SpawnGhost(transform.position, transform.localScale, GetComponent<SpriteRenderer>().sprite);
			CloneGhostTimer = timeBetweenEachCloneDrawn;
		}
	}

	void StartRewind ()
	{
		isRewinding = true;
		playerRigidbody2D.isKinematic = true;
		GetComponent<CapsuleCollider2D>().enabled = false;
	}

	void StopRewind ()
	{
		isRewinding = false;
		playerRigidbody2D.isKinematic = false;
		GetComponent<CapsuleCollider2D>().enabled = true;
	}

    public void Move(float move, bool jump, bool prePushPull)
	{
		// Only control the player if grounded or airControl is turned on
		if (isGrounded || m_AirControl)
		{
			Vector3 targetVelocity = new Vector2(move * 10f, playerRigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			playerRigidbody2D.velocity = Vector3.SmoothDamp(playerRigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left (when not grbbing)
			if (!grabbed)
			{
				if (move > 0 && !FacingRight)
				{
					Flip();
				}
				else if (move < 0 && FacingRight)
				{
					Flip();
				}
			}		
		}

		// Jump if pressed and "still" grounded
		stillGroundedDelayTimer -= Time.fixedDeltaTime;

		if (isGrounded)
		{
			stillGroundedDelayTimer = m_StillGroundedDelay;
		}

		if (stillGroundedDelayTimer > 0 && jump)
		{
			stillGroundedDelayTimer = 0f;
			isGrounded = false;
			playerRigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}

	private void Flip()
	{
		FacingRight = !FacingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void OnDrawGizmos() 
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * transform.localScale.x * m_GrabDistance);
	}

	private void SetAnimatorParameters (float pushpushDirec, bool isJumping, bool isWalking, bool isPushingPulling, bool isPrePushingPulling)
	{
		playerAnimator.SetFloat("Push-PullDirection",pushpushDirec);
		playerAnimator.SetBool("isJumping", isJumping);
		playerAnimator.SetBool("isWalking", isWalking);
		playerAnimator.SetBool("isPushing-Pulling", isPushingPulling);
		playerAnimator.SetBool("isPrePushing-Pulling", isPrePushingPulling);
	}

	private void CheckAnimation(float move)
	{
		playerAnimator.SetFloat("Movement", Mathf.Abs(move));

		if (isGrounded)
		{
			if (prePushPull && grabbed)
			{
				if (Mathf.Abs(move) > 0)
				{
					if (move > 0 )
					{
						if (grabbedFacingRight)
						{
							SetAnimatorParameters(1f,false,false,true,false);
						}
						else
						{
							SetAnimatorParameters(-1f,false,false,true,false);
						}
					}
					else
					{
						if (grabbedFacingRight)
						{
							SetAnimatorParameters(-1f,false,false,true,false);
						}
						else
						{
							SetAnimatorParameters(1f,false,false,true,false);
						}
					}
				}
				else
				{	
					SetAnimatorParameters(1f,false,false,false,true);
				}
			}
			else if (Mathf.Abs(move) > 0)
			{
				SetAnimatorParameters(1f,false,true,false,false);
			}
			else
			{
				SetAnimatorParameters(1f,false,false,false,false);
			}
		}
		else if (!isGrounded)
		{
			SetAnimatorParameters(1f,true,false,false,false);
		}
	}
}
