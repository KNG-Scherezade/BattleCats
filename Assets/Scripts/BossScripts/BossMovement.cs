using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour {

	public Transform[] fig8PathPoints;
	public Transform fig8PathParent;
	public Transform[] hoverPathPoints;
	public Transform hoverPathParent;
	public Transform strikeAttackStart;
	private Transform yarnTransform;
	private Rigidbody2D yarnRigidbody;
	private Rigidbody2D boss;

	private HealthSlider yarnHealthScript;
	private GameObject strikeAttackTarget;
	private DynamicCamera cameraScript;

	private BossHealth healthScript;

	private AudioSource dashImpactSound;

	public bool hover;
	public bool fig8Path;
	public bool followYarnOnPath;
	public bool hoverPath;
	public bool randomizePath;
	public bool strikeAttack;
	public bool verticalFollow;

	private bool strikeAttackInit;
	private bool strikeAttackTimerOn;
	private bool rotateRight;
	private bool reached;


	private float hoverTime;
	private float timestamp;
	public float hoverSpeed;
	public float hoverHeight;
	public float pathPointRadius;
	public float hoverPathPointRadius;
	public float pathSpeed;
	public float strikeAttackTimer;
	public float strikeAttackSpeed;
	public float strikeAttackRadius;
	public float strikeRotationSpeed;
	public float rightRotateBound;
	public float leftRotateBound;
	public float strikeDamageRadius;


	public int currentFig8Path;
	public int currentHoverPath;


	// Use this for initialization
	void Start () {
		hover = true;
		followYarnOnPath = true;
		hoverPath = false;
		randomizePath = false;
		strikeAttack = false;
		strikeAttackInit = true;
		strikeAttackTimerOn = false;
		rotateRight = false;
		reached = false;
		verticalFollow = false;

		strikeAttackTarget = new GameObject();
		healthScript = gameObject.GetComponent<BossHealth>();
		yarnRigidbody = GameObject.FindGameObjectWithTag ("YarnPhysics").GetComponent<Rigidbody2D>();
		yarnHealthScript = GameObject.FindGameObjectWithTag ("YarnPhysics").GetComponent<HealthSlider>();
		yarnTransform = GameObject.FindGameObjectWithTag ("Yarn_inside_container").GetComponent<Transform> ();
		cameraScript = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<DynamicCamera> ();
		boss = gameObject.GetComponent<Rigidbody2D> ();
		dashImpactSound = gameObject.GetComponent<AudioSource> ();


		hoverTime = 0f;
		hoverSpeed = 2.82f;
		hoverHeight = 0.07f;
		pathPointRadius = 0.2f;
		hoverPathPointRadius = 2.5f;
		strikeAttackRadius = 0.3f;
		strikeDamageRadius = 1.8f;

		currentFig8Path = 0;
		currentHoverPath = 0;

		pathSpeed = 5f; //0.21f without Time.fixedDeltaTime, 5f with Time.fixedDeltaTime
		strikeAttackSpeed = 25f;
		strikeAttackTimer = 2f;
		timestamp = 0;
		strikeRotationSpeed = 180f;
		rightRotateBound = -0.05f;
		leftRotateBound = 0.05f;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate() {
		if (hover) {
			hoverTime += Time.deltaTime*hoverSpeed;
			Hover ();
		}
			
		if (fig8Path) {
			Fig8PathMovement ();
		}
		if (strikeAttack) {
			if (strikeAttackTimerOn) {
				if (Time.time - timestamp >= strikeAttackTimer) {
					strikeAttackTimerOn = false;
				}
			}
			StrikeAttack ();

		}

		if (hoverPath) {
			HoverPathMovement ();
		}
	}

	void Hover(){
		float y = Mathf.Sin (hoverTime) * hoverHeight;

		transform.position = new Vector2 (transform.position.x, transform.position.y + y);
	}
		

	void Fig8PathMovement(){
		Vector3 direction;
		Vector3 directionNormalized;

		if (followYarnOnPath) {
			fig8PathParent.position = (Vector2)GameObject.FindGameObjectWithTag("Yarn_inside_container").transform.position + new Vector2(-10, 10);
		}

		//		Hold position if yarn is moving up
		if (verticalFollow)
		{
			direction = (fig8PathPoints [currentFig8Path].position - transform.position);
			directionNormalized = direction.normalized;
		} else if(yarnRigidbody.velocity.y <= boss.velocity.y) 
			{
				direction = (fig8PathPoints [currentFig8Path].position - transform.position);
				directionNormalized = direction.normalized;
			} else 
				{
					direction = new Vector3(0f,0f,0f);
					directionNormalized = direction.normalized;
				}

		transform.Translate (directionNormalized * pathSpeed * Time.fixedDeltaTime);


		if (direction.magnitude <= pathPointRadius) {
			if (randomizePath) {
				currentFig8Path = Random.Range (0, (fig8PathPoints.Length - 1));
				
			} else {
				currentFig8Path++;
			}

			if (currentFig8Path >= fig8PathPoints.Length) {
				currentFig8Path = 0;
			}
		}
	}

	void StrikeAttack(){
		if (strikeAttackInit) {
			hover = false;
			strikeAttackStart.position = hoverPathPoints[currentHoverPath].position + new Vector3(0f,5f, 0f);
			Vector3 direction = (strikeAttackStart.position - transform.position);
			Vector3 directionNormalized = direction.normalized;

			transform.Translate (directionNormalized * strikeAttackSpeed * Time.fixedDeltaTime);

			if (direction.magnitude <= strikeAttackRadius) {
				strikeAttackInit = false;
				timestamp = Time.time;
				strikeAttackTimerOn = true;
				strikeAttackTarget.transform.position = (Vector2)GameObject.FindGameObjectWithTag ("Yarn_inside_container").transform.position;
				healthScript.StartInvulnerabilty ();
			}
		} else {
			//shake animation
			if (transform.rotation.z <= rightRotateBound) {
				rotateRight = false;
			}
			if (transform.rotation.z >= leftRotateBound) {
				rotateRight = true;
			} 
			if (rotateRight) {
				transform.Rotate (0, 0, -Time.deltaTime * strikeRotationSpeed);
			} else {
				transform.Rotate (0, 0, Time.deltaTime * strikeRotationSpeed);
			}

			if (!strikeAttackTimerOn) {
				transform.rotation = new Quaternion();
				
				Vector3 directionB = (strikeAttackTarget.transform.position - transform.position);
				Vector3 directionNormalizedB = directionB.normalized;

				transform.Translate (directionNormalizedB * strikeAttackSpeed * Time.fixedDeltaTime);
				if (directionB.magnitude <= strikeAttackRadius) {
					dashImpactSound.Play ();
					Vector3 VectorToYarnCenter = yarnTransform.position - transform.position;
					if(VectorToYarnCenter.magnitude <= strikeDamageRadius)
					{
						yarnHealthScript.BulletDamage("BossStrike", transform.position, new GameObject());
					}
					strikeAttack = false;
					strikeAttackInit = true;
					hoverPath = true;
					hover = true;
					healthScript.EndInvulnerability ();
				}
			}
		}
	}

	void HoverPathMovement()
	{
//		Debug.Log ("Yarn velocity: " + yarn.velocity);
		Vector3 direction;
		Vector3 directionNormalized;

		if (followYarnOnPath) 
		{
			hoverPathParent.position = (Vector2)GameObject.FindGameObjectWithTag("Yarn_inside_container").transform.position;
		}
	
		if (yarnRigidbody.velocity.x < 0) 
		{
			//set to right of yarn point in hoverPath
			currentHoverPath = 0;
			cameraScript.origonal_position.x = 2.47f;
		} else 
		{
			currentHoverPath = 1;
			cameraScript.origonal_position.x = -2.47f;
		}

//		Hold position if yarn is moving up unless verticalFollow is true
		if (verticalFollow) {
			direction = (hoverPathPoints [currentHoverPath].position - transform.position);
			directionNormalized = direction.normalized;
		} else if (yarnRigidbody.velocity.y <= boss.velocity.y) 
			{
				direction = (hoverPathPoints [currentHoverPath].position - transform.position);
				directionNormalized = direction.normalized;
			} else
				{
					direction = new Vector3(0f,0f,0f);
					directionNormalized = direction.normalized;
				}

		if (!reached) 
		{
			hover = false;

			transform.Translate (directionNormalized * pathSpeed * Time.fixedDeltaTime);

			if (direction.magnitude <= hoverPathPointRadius) {
				reached = true;
				hover = true;
			}
		} else {
			if (direction.magnitude >= hoverPathPointRadius) {
				reached = false;
			}
		}

	}

	void OnDrawGizmos(){
		if (fig8PathPoints == null)
			return;
		foreach (Transform pathPoint in fig8PathPoints) {
			if (pathPoint) {
				Gizmos.DrawSphere (pathPoint.position, pathPointRadius);
			}
		}
		if (hoverPathPoints == null)
			return;
		foreach (Transform pathPoint in hoverPathPoints) {
			if (pathPoint) {
				Gizmos.DrawSphere (pathPoint.position, pathPointRadius);
			}
		}
	}
}
