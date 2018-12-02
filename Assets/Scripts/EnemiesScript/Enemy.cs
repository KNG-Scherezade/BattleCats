using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private string Name;
    private string Description;

    [SerializeField]
    private bool ShieldPenetration = false;

    [SerializeField]
    private float FiringRate = 1.0f;

    [SerializeField]
    private float Damage = 1.0f;

    [SerializeField]
    private bool HasDamageOnDeath = false;

    [SerializeField]
    private float DamageOnDeath = 0.0f;

    [SerializeField]
    private float Health = 1.0f;

    [SerializeField]
    private bool HasShield = false;

    [SerializeField]
    private float ShieldValue = 1.0f;

    [SerializeField]
    private float mMoveSpeed;

    private float mTimer = 0;
    private float bunnyJumpTimer = 0;

    [SerializeField]
    private GameObject EnemyProjectilePrefab;

    private Rigidbody2D bunnyRigidBody;

    public GameObject ExplosionPrefab;
    public Vector3 YarnPosition;

    private bool isPlacedInEditor = false;
    [SerializeField]
    private GameObject triggerZone;
    private EnemyTrigger trigger = null;

    [SerializeField]
    private bool activeOnStart = true;
    List<Collider2D> colliders = new List<Collider2D>();
    List<SpriteRenderer> renderers = new List<SpriteRenderer>();

    void Start()
    {
        YarnPosition = GameObject.FindGameObjectWithTag("Yarn_inside_container").transform.position;
        if (gameObject.tag != "EnemyC")
        {            
            transform.LookAt(YarnPosition, Vector2.up);
            transform.Rotate(new Vector3(0, 1, 0), 90.0f);
        }
        else
        {
            bunnyRigidBody = GetComponent<Rigidbody2D>();           
        }

        // Not a pretty hack, but it'll do, pig
        if ((transform.parent != null && transform.parent.gameObject.name == "Enemies")
            || (transform.parent.parent != null && transform.parent.gameObject.name == "Enemies"))
        {
            isPlacedInEditor = true;
        }

        if (triggerZone != null)
        {
            trigger = triggerZone.GetComponent<EnemyTrigger>();
        }

        foreach (Collider2D coll in transform.GetComponents<Collider2D>())
        {
            if (coll != null)
            {
                colliders.Add(coll);
            }
        }

        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        if (rend != null)
        {
            renderers.Add(rend);
        }

        foreach (Transform t in transform)
        {
            foreach (Collider2D coll in t.GetComponents<Collider2D>())
            {
                if (coll != null)
                {
                    colliders.Add(coll);
                }
            }

            SpriteRenderer ren = t.GetComponent<SpriteRenderer>();
            if (ren != null)
            {
                renderers.Add(ren);
            }
        }

        if (!activeOnStart)
        {
            foreach (SpriteRenderer ren in renderers)
            {
                ren.enabled = false;
            }

            foreach (Collider2D coll in colliders)
            {
                coll.enabled = false;
            }

            if (bunnyRigidBody != null)
            {
                bunnyRigidBody.simulated = false;
            }
        }
    }

    void Update()
    {
        GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if (!gameManager.isGamePaused())
        {
            YarnPosition = GameObject.FindGameObjectWithTag("Yarn_inside_container").transform.position;
            foreach (Transform child in transform)
            {
                SimpleIdleAnimation(child);
            }

            if (gameObject.tag != "EnemyC")
            {
                float step = mMoveSpeed * Time.deltaTime;
                if (trigger != null && trigger.IsActivated)
                {
                    if (!activeOnStart)
                    {
                        foreach (SpriteRenderer rend in renderers)
                        {
                            rend.enabled = true;
                        }

                        foreach (Collider2D coll in colliders)
                        {
                            coll.enabled = true;
                        }

                        if (bunnyRigidBody != null)
                        {
                            bunnyRigidBody.simulated = true;
                        }
                    }

                    transform.position = Vector3.MoveTowards(transform.position, YarnPosition, step);
                    transform.LookAt(YarnPosition, Vector2.up);
                    transform.Rotate(new Vector3(0, 1, 0), 90.0f);
                }
            }
            else
            {
                YarnPosition = new Vector3(YarnPosition.x, YarnPosition.y - (4.6f / 2.0f), 0);
                transform.LookAt(YarnPosition);
                transform.Rotate(new Vector3(0, 1, 0), 90.0f);

                bunnyJumpTimer += Time.deltaTime;

                if (bunnyJumpTimer >= 0.2 && bunnyJumpTimer <= 1)
                {
                    float step = mMoveSpeed * Time.deltaTime;

                    // Temporary while testing level 1
                    if (trigger != null && trigger.IsActivated)
                    {
                        if (!activeOnStart)
                        {
                            foreach (SpriteRenderer rend in renderers)
                            {
                                rend.enabled = true;
                            }

                            foreach (Collider2D coll in colliders)
                            {
                                coll.enabled = true;
                            }

                            if (bunnyRigidBody != null)
                            {
                                bunnyRigidBody.simulated = true;
                            }
                        }

                        transform.position = Vector3.MoveTowards(transform.position, YarnPosition, step);
                    }                   
                }
                else if (bunnyJumpTimer >= 1.0)
                {
                    bunnyJumpTimer = 0;
                }
            }
        }

    }

    private void SimpleIdleAnimation(Transform child)
    {
        // Temporary while testing level 1
        if (trigger != null && trigger.IsActivated)
        {
            mTimer += Time.deltaTime;
            if (mTimer >= FiringRate)
            {
                mTimer = 0;
                Instantiate(EnemyProjectilePrefab, child.position, Quaternion.identity);
            }
        }  
    }

    public void BulletCollision(string tag)
    {
        if (tag == "YarnABullet")
        {
            TakeDamage(HasShield ? 25 : 50);
        }
        else if (tag == "YarnBBullet")
        {
            TakeDamage(HasShield ? 1000 : 25);
        }
        else if (tag == "YarnCBullet")
        {
            TakeDamage(HasShield ? 50 : 100);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
//        print(col.tag);
        if (col.CompareTag("YarnABullet"))
        {
            TakeDamage(HasShield ? 25 : 50);
        }
        else if (col.CompareTag("YarnBBullet"))
        {
            TakeDamage(HasShield ? 1000 : 25);
        }
        else if (col.CompareTag("YarnCBullet"))
        {
            TakeDamage(HasShield ? 50 : 100);
        }
    }

    private void TakeDamage(float damageDone)
    {
        if (HasShield)
        {
            ShieldValue -= damageDone;
            if (ShieldValue <= 0)
            {
                HasShield = false;

                foreach (Transform child in transform)
                {
                    if (child.tag == "EnemyBShield")
                    {
                        Destroy(child.gameObject);
                        break;
                    }

                }
            }
        }
        else
        {
            Health -= damageDone;
            if (Health <= 0)
            {                         
                Instantiate(ExplosionPrefab, this.transform.position, Quaternion.identity);
                if (!gameObject.CompareTag("Bee"))
                {
                    if (gameObject.transform.parent != null && gameObject.transform.parent.name != "Enemies")
                    {
                        Destroy(transform.parent.gameObject);
                    }
                    Destroy(gameObject);
                }
                else
                {
                    gameObject.GetComponentInParent<EnemyRespawn>().OnHit(); // For bees
                }
            }
        }

    }


}
