using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMovement : MonoBehaviour {
	[SerializeField]
	float moveSpeed;
    [SerializeField]
    float jumpVelocity;
    [SerializeField]
	LayerMask whatIsGround;
    [SerializeField]
    GameObject m_insideCameraDisplay;

    private Coroutine m_cameraCoroutine = null;
    private float m_cameraChangeSmoothing = 256.0f;
    private float m_cameraOriginalSize;
    private float m_cameraChangeIncrement = 0.1f;

	public int controller_number;
	public float groundCheckDistance = 0.1f;
	private Vector3 warp_location;

    private Animator anim;
	private Rigidbody2D rb;
	public Vector2 facingDirection;
	private Transform groundCheck;
	private Transform spriteChild;

    private bool isOnLadder;
    private bool isInStation;

	private bool isWalking;
	private bool isClimbing;
    private bool isClimbingIdle;
    private bool isDriving;
	private bool isGrounded;
	private bool isHiding;
	private bool isPushing;

    private void Awake()
    {
        m_insideCameraDisplay = GameObject.Find("InsideCameraDisplay");
    }

    void Start () {
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		groundCheck = transform.Find ("GroundCheck");
		spriteChild = transform.Find ("SpriteChild");
		facingDirection = Vector2.right;
        isOnLadder = false;
        isInStation = false;
        //m_insideCameraDisplay.SetActive(false);
        m_cameraOriginalSize = Camera.main.orthographicSize;
    }
		

	void Update()
	{
        GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		if (!gameManager.isGamePaused() && controller_number > 0)
        {
            if (Input.GetButtonDown("ToggleInsideCamera_P" + controller_number))
            {
                bool currentlyActive = m_insideCameraDisplay.activeInHierarchy;

                if (currentlyActive)
                {
                    m_insideCameraDisplay.SetActive(false);
                    if (m_cameraCoroutine != null)
                    {
                        StopCoroutine(m_cameraCoroutine);
                    }
                    m_cameraCoroutine = StartCoroutine(ChangeCameraSize(CameraChange.Shrink));
                }
                else
                {
                    m_insideCameraDisplay.SetActive(true);
                    if (m_cameraCoroutine != null)
                    {
                        StopCoroutine(m_cameraCoroutine);
                    }
                    m_cameraCoroutine = StartCoroutine(ChangeCameraSize(CameraChange.Grow));
                }
            }
        }
	}

    public enum CameraChange
    {
        Grow,
        Shrink
    }

    IEnumerator ChangeCameraSize(CameraChange change)
    {
        if (change == CameraChange.Grow)
        {
            while (Camera.main.orthographicSize < 2.0f * m_cameraOriginalSize)
            {
                Camera.main.orthographicSize += m_cameraChangeIncrement * m_cameraChangeSmoothing * Time.deltaTime;

                yield return null;
            }
        }
        else
        {
            while (Camera.main.orthographicSize > 0.5f * m_cameraOriginalSize)
            {
                Camera.main.orthographicSize -= m_cameraChangeIncrement * m_cameraChangeSmoothing * Time.deltaTime;

                yield return null;
            }
        }
    }

    private void FixedUpdate()
    {
		if (controller_number > 0) {
			if (!isInStation) {
				MoveCharacter ();
				CheckJump ();
				CheckGround ();
			}
		}
		UpdateAnimation ();
		TestAnimation ();
		updateSortingOrder ();
    }

	void LateUpdate(){
		if (controller_number > 0) {
			if (!isInStation) {
			}
		}
	}

	void updateSortingOrder(){
		if (!isInStation) {
			GameObject[] cat_overlap_objects = getCatOverlaps ();
			bool cat_overlap = cat_overlap_objects [0] != null;
			if (!cat_overlap && !isWalking && !isClimbing) {
				this.transform.Find ("SpriteChild").GetComponent<SpriteRenderer> ().sortingOrder = 5;
			} else if (cat_overlap && isClimbing) {
				this.transform.Find ("SpriteChild").GetComponent<SpriteRenderer> ().sortingOrder = 6;
				foreach (GameObject cat_overlap_object in cat_overlap_objects) {
					if (cat_overlap_object == null)
						continue;
					else if (cat_overlap_object.transform.Find ("SpriteChild").GetComponent<SpriteRenderer> ().sortingOrder
						== this.transform.Find ("SpriteChild").GetComponent<SpriteRenderer> ().sortingOrder) {
						this.transform.Find ("SpriteChild").GetComponent<SpriteRenderer> ().sortingOrder++;
					}
				}
			}
			else if (cat_overlap && isWalking) {
				this.transform.Find ("SpriteChild").GetComponent<SpriteRenderer> ().sortingOrder = 11;
				foreach (GameObject cat_overlap_object in cat_overlap_objects) {
					if (cat_overlap_object == null)
						continue;
					else if (cat_overlap_object.transform.Find ("SpriteChild").GetComponent<SpriteRenderer> ().sortingOrder
					   == this.transform.Find ("SpriteChild").GetComponent<SpriteRenderer> ().sortingOrder) {
						this.transform.Find ("SpriteChild").GetComponent<SpriteRenderer> ().sortingOrder++;
					}
				}
			}
		} else {
			this.transform.Find ("SpriteChild").GetComponent<SpriteRenderer> ().sortingOrder = 4;
		}

	}

	bool checkCatOverlap(){
		Collider2D [] overlaps = new Collider2D[10];
		this.GetComponent<BoxCollider2D> ().OverlapCollider (new ContactFilter2D (), overlaps);
		foreach(Collider2D overlap in overlaps){
			if (overlap == null)
				continue;
			else if (overlap.gameObject.tag.ToUpper() ==  "CAT")
				return true;
		}	
		return false;
	}

	GameObject []  getCatOverlaps(){
		Collider2D [] overlaps = new Collider2D[20];
		GameObject [] cat_overlaps = new GameObject[3];
		this.GetComponent<BoxCollider2D> ().OverlapCollider (new ContactFilter2D (), overlaps);
		int index = 0;
		foreach(Collider2D overlap in overlaps){
			if (overlap == null)
				continue;
			else if (overlap.gameObject.tag.ToUpper () == "CAT") {
				cat_overlaps [index] = overlap.gameObject;
				index++;
			}
		}	
		return cat_overlaps;
	}

    void CheckGround()
	{
//		Collider2D[] colliders = Physics2D.OverlapCircleAll (groundCheck.position, groundCheckRadius, whatIsGround);
	
		//Check if there's a raycast collision on either right or left of sprite
		isGrounded = Physics2D.Raycast(this.transform.position + new Vector3(this.GetComponent<BoxCollider2D>().bounds.extents.x, 0, 0), new Vector3(0, -1, 0), groundCheckDistance, whatIsGround);
		if (!isGrounded) {
			isGrounded = Physics2D.Raycast(this.transform.position - new Vector3(this.GetComponent<BoxCollider2D>().bounds.extents.x, 0, 0), new Vector3(0, -1, 0), groundCheckDistance, whatIsGround);

		}
//		foreach (Collider2D col in colliders) {
//			if (col.gameObject != gameObject) {
//				//Debug.Log (this.transform.localPosition);
//                isGrounded = true;
//				return;
//			}
//		}
//        isGrounded = false;
	}

	void MoveCharacter()
	{
		isWalking = false;
        isClimbing = false;
        isClimbingIdle = false;
		isDriving = false;
        isHiding = false;
		isPushing = false;
        
        CheckHorizontalMovement();
		CheckIsClimbing();
		if(isClimbing)
			this.transform.Find ("SpriteChild").GetComponent<SpriteRenderer> ().sortingOrder = 6;
		if(isWalking)
			this.transform.Find ("SpriteChild").GetComponent<SpriteRenderer> ().sortingOrder = 7;
	}

	void FaceDirection(Vector2 direction)
	{
		Quaternion rotation3D = direction == Vector2.right ? Quaternion.LookRotation (Vector3.forward) : Quaternion.LookRotation (Vector3.back);
		facingDirection = direction;
		spriteChild.rotation = rotation3D;
	}
			
	public void setStationAnimation(bool drive_state, string station_category, 
		string station_name, Vector3 warp_location){
        isInStation = drive_state;
		this.warp_location = warp_location;
		if (this.warp_location != Vector3.zero) {
			this.transform.position = warp_location;
		}
		switch (station_category) {
		case "StationNav":  // [House] - Yarn control - Hiding animation
			isHiding = drive_state;
			isDriving = false;
			isPushing = false;
			isWalking = false;
			isClimbing = false;
			FaceDirection (Vector2.right);
			break;
        case "StationShield": // [Box] - Shield control - Driving animation
            isHiding = false;
            isDriving = drive_state;
            isPushing = false;
            isWalking = false;
            isClimbing = false;
            break;
        case "TurretStation": // [Whackamole/scratch thing/fish] - Turret control - Pushing animation
            isHiding = false;
            isDriving = false;
            isPushing = drive_state;
            isWalking = false;
            isClimbing = false;
			switch (station_name) {
			case "TurretStation_Right":
				FaceDirection (Vector2.right);
				break;
			case  "TurretStation_Left":
				FaceDirection (Vector2.left);
				break;
			case "TurretStation_Top":
				FaceDirection (Vector2.left);
				break;
			}
            break;
        default:
            isHiding = false;
            isDriving = false;
            isPushing = false;
            isWalking = false;
            isClimbing = false;
            break;
		}
	}

	void UpdateAnimation()
	{
		anim.SetBool ("isWalking", isWalking);
		anim.SetBool ("isClimbing", isClimbing);
        anim.SetBool("isClimbingIdle", isClimbingIdle);
        anim.SetBool ("isDriving", isDriving);
		anim.SetBool ("isHiding", isHiding);
		anim.SetBool ("isPushing", isPushing);
	}

    void CheckHorizontalMovement()
    {
		float axis = Input.GetAxisRaw("Horizontal_P"  + controller_number);
        if (axis < 0)
        {
            transform.Translate(-Vector2.right * moveSpeed * Time.deltaTime);
            FaceDirection(-Vector2.right);
            isWalking = true;
        }
        if (axis > 0)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            FaceDirection(Vector2.right);
            isWalking = true;
        }
    }

    void CheckIsClimbing()
    {
        if (isOnLadder)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(0f, 0f);
            isClimbingIdle = true;
			float axis = Input.GetAxisRaw("Vertical_P"  + controller_number);
            if (axis > 0)
            {
                isClimbing = true;
                isClimbingIdle = false;
                transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
            }
            if (axis < 0)
            {
                isClimbing = true;
                isClimbingIdle = false;
                transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
            }
        }
        else
        {
            rb.gravityScale = 1f;
            isClimbingIdle = false;
            isClimbing = false;
        }
    }

    void CheckJump()
    {
		if (Input.GetButton("Jump_P" + controller_number) && isGrounded && !isOnLadder)
        {
            rb.velocity += new Vector2(0, jumpVelocity);
            isWalking = false;
//            isGrounded = false;
        }
		else if (!Input.GetButton("Jump_P" + controller_number) && rb.velocity.y > 0)
        {
            rb.velocity -= new Vector2(0, jumpVelocity * 0.2f);
        }
    }

	void TestAnimation()
	{
//		if (Input.GetKey (KeyCode.Z)) {
//			isDriving = true;
//		}
		if (Input.GetKey (KeyCode.Q)) {
			isHiding = true;
		}
		if (Input.GetKey (KeyCode.R)) {
			isPushing = true;
		}
	}

    public void SetOnLadder(bool value)
    {
        isOnLadder = value;
		if (isOnLadder){
            isGrounded = false;
		}
    }
}


	
