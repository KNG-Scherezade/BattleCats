using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expiration : MonoBehaviour {

    [SerializeField]
    public float TimeToExpire;

    public bool IsExplosion = false;

    private float Timer = 0.0f;

    void Update()
    {

        if (OutsideBoundaries() || IsExplosion)
        {
            Timer += Time.deltaTime;
            if (Timer > TimeToExpire)
                Destroy(gameObject);
        }
        else
        {
            Timer = 0;
        }

    }

    private bool OutsideBoundaries()
    {
        return gameObject.transform.position.x < Boundaries.LeftBoundary - 3.0f || gameObject.transform.position.x > Boundaries.RightBoundary + 3.0f ||
            gameObject.transform.position.y < Boundaries.LowerBoundary || gameObject.transform.position.y > Boundaries.UpperBoundary + 3.0f;
    }
}
