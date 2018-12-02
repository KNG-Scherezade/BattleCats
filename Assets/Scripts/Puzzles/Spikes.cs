using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {

    [SerializeField]
    Vector2 VerticalStartDirection;

    [SerializeField]
    float VerticalMaxDistance;

    [SerializeField]
    Vector2 HorizontalStartDirection;

    [SerializeField]
    float HorizontalMaxDistance;

    [SerializeField]
    float speed;

    private List<GameObject> currentCollisions = new List<GameObject>();

    private float minVertical, maxVertical, minHorizontal, maxHorizontal;
    private float timerForStay;    

    private bool moveRight, moveLeft, moveUp, moveDown;
	// Use this for initialization
	void Start ()
	{
	    moveRight = false;
	    moveUp = false;
	    moveLeft = false;
	    moveDown = false;
	    timerForStay = 0;

	    minVertical = transform.position.y;
	    maxVertical = transform.position.y;
	    minHorizontal = transform.position.x;
	    maxHorizontal = transform.position.x;

	    if (VerticalStartDirection == Vector2.up)
	    {
	        maxVertical = transform.position.y + VerticalMaxDistance;
	        moveUp = true;
	    }
        else if (VerticalStartDirection == Vector2.down)
	    {
	        minVertical = transform.position.y - VerticalMaxDistance;
	        moveDown = true;
	    }

	    if (HorizontalStartDirection == Vector2.left)
	    {
	        minHorizontal = transform.position.x - HorizontalMaxDistance;
	        moveLeft = true;
	    }
        else if (HorizontalStartDirection == Vector2.right)
	    {
	        maxHorizontal = transform.position.x + HorizontalMaxDistance;
	        moveRight = true;
	    }
    }
	
	// Update is called once per frame
	void Update ()
	{
	    float step = speed * Time.deltaTime;

	    if (transform.position.x > maxHorizontal)
	    {
	        moveLeft = true;
	        moveRight = false;
	    }
        else if (transform.position.x < minHorizontal)
	    {
	        moveRight = true;
	        moveLeft = false;
	    }

	    if (transform.position.y > maxVertical)
	    {
	        moveDown = true;
	        moveUp = false;
	    }
	    else if (transform.position.y < minVertical)
	    {
	        moveUp = true;
	        moveDown = false;
	    }


        if (moveLeft)
	    {
	        transform.Translate(Vector2.left * step, Space.World);
	    }
	    else if (moveRight)
	    {
	        transform.Translate(Vector2.right * step, Space.World);
	    }

	    if (moveUp)
	    {
	        transform.Translate(Vector2.up * step, Space.World);
	    }
	    else if (moveDown)
	    {
	        transform.Translate(Vector2.down * step, Space.World);
	    }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        currentCollisions.Add(col.gameObject);

        if (!isShieldColliding(currentCollisions))
        {
            col.gameObject.GetComponent<HealthSlider>().DealDamage(100, transform.position, gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (!isShieldColliding(currentCollisions))
        {
            timerForStay += Time.deltaTime;

            if (timerForStay > 5.0f)
            {
				Debug.Log (col.tag);
                col.gameObject.GetComponent<HealthSlider>().DealDamage(100, transform.position, gameObject);
                timerForStay = 0;
            }
                
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        print("out is" + col.gameObject.tag);
        currentCollisions.Remove(col.gameObject);
    }

    private bool isShieldColliding(List<GameObject> listOfCollidedObjects)
    {
        foreach (GameObject gObject in listOfCollidedObjects)
        {
            if (gObject != null)
            {
                if (gObject.tag == "Shield")
                    return true;
            }
        }
        return false;
    }
}
