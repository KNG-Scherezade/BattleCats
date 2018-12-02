using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnMovement : MonoBehaviour {

	[SerializeField]
	float moveSpeed;
	[SerializeField]
	LayerMask whatIsGround;


	private Animator anim;
	private Rigidbody2D rb;
	private Vector2 facingDirection;
	private float groundCheckRadius = 0.1f;
	private Transform groundCheck;
	private Transform spriteChild;

	private bool isRolling;
	private bool grounded;

	void Start () {
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		groundCheck = transform.Find ("GroundCheck");
		spriteChild = transform.Find ("OutsideSprite");
		facingDirection = Vector2.right;
	}

	void Update()
	{
		CheckGround ();
		MoveYarn ();
		updateAnimation ();
	}


	void CheckGround()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll (groundCheck.position, groundCheckRadius, whatIsGround);
		foreach (Collider2D col in colliders) {
			if (col.gameObject != gameObject) {
				grounded = true;
				return;
			}
		}
		grounded = false;
	}

	void MoveYarn()
	{
		isRolling = false;
		if (Input.GetButton ("Left")) {
			transform.Translate (-Vector2.right * moveSpeed * Time.deltaTime);
			FaceDirection (-Vector2.right);
			isRolling = true;
		}
		if (Input.GetButton ("Right")) {
			transform.Translate (Vector2.right * moveSpeed * Time.deltaTime);
			FaceDirection (Vector2.right);
			isRolling= true;
		}
	}

	void FaceDirection(Vector2 direction)
	{
		Quaternion rotation3D = direction == Vector2.right ? Quaternion.LookRotation (Vector3.forward) : Quaternion.LookRotation (Vector3.back);
		spriteChild.rotation = rotation3D;
	}

	void updateAnimation()
	{
		anim.SetBool ("isRolling", isRolling);
	}
}
