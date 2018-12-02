using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class YarnPosition
{
    public static float yarnPosX = 0.0f;
    public static float yarnPosY = 0.0f;
    public float DisplayYarnPosX = 0.0f;
    public float DisplayYarnPosY = 0.0f;
}

public class Boundaries
{
    public static float UpperBoundary;
    public static float LeftBoundary;
    public static float RightBoundary;
    public static float LowerBoundary;
}

public class Wave : MonoBehaviour
{
    //for visualisation
    [SerializeField]
    public YarnPosition localYarnPosition;

    GameObject camera;

    [SerializeField]
    GameObject EnemyTypeA;

    [SerializeField]
    GameObject EnemyTypeB;

    [SerializeField]
    GameObject EnemyTypeC;

    [SerializeField]
    float UpperBoundary;

    [SerializeField]
    float LeftBoundary;

    [SerializeField]
    float RightBoundary;

    [SerializeField]
    float LowerBoundary;

    [SerializeField]
    int NumberOfTripletsPerWave;

    private int currentWaveNumber;

    // Idle timer variables
    [SerializeField]
    float mSpawnTime = 5.0f;

    [SerializeField]
    float mSpawnDelay = 15.0f;

    private float mTimer = 0.0f;
    private int counter = 0;

    private bool waveStarted = false;

    // Use this for initialization
    void Start()
    {
        transform.position = new Vector2(camera.transform.position.x, camera.transform.position.y + Boundaries.UpperBoundary);
        currentWaveNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateYarnGlobalPosition();
        UpdateGlobalBoundaries();
        transform.position = new Vector2(YarnPosition.yarnPosX, YarnPosition.yarnPosY + Boundaries.UpperBoundary);

        mTimer += Time.deltaTime;

        if (mTimer > mSpawnTime && currentWaveNumber != NumberOfTripletsPerWave)
        {
            waveStarted = true;
            mTimer = 0;
            SpawnType(1);
            SpawnType(2);
            SpawnType(3);

            currentWaveNumber++;
        }
    }    

    private void UpdateYarnGlobalPosition()
    {
        YarnPosition.yarnPosX = camera.transform.position.x;
        YarnPosition.yarnPosY = camera.transform.position.y;
        localYarnPosition.DisplayYarnPosX = camera.transform.position.x;
        localYarnPosition.DisplayYarnPosY = camera.transform.position.y;
    }

    private void UpdateGlobalBoundaries()
    {
        Boundaries.LeftBoundary = LeftBoundary + YarnPosition.yarnPosX;
        Boundaries.RightBoundary = RightBoundary + YarnPosition.yarnPosX;
        Boundaries.UpperBoundary = UpperBoundary + YarnPosition.yarnPosY;
        Boundaries.LowerBoundary = LowerBoundary + YarnPosition.yarnPosY;
    }

    private void SpawnType(int typeNumber)
    {
        Vector2 spawnPosition = new Vector2(0,0);

        if (typeNumber == 1)
        {
            spawnPosition = GenerateSpawnPositionAB();
            var typeA = Instantiate(EnemyTypeA, spawnPosition, Quaternion.identity);
        }
        else if (typeNumber == 2)
        {
            spawnPosition = GenerateSpawnPositionAB();
            var typeB = Instantiate(EnemyTypeB, spawnPosition, Quaternion.identity);
        }
        else if (typeNumber == 3)
        {
            spawnPosition = GenerateSpawnPositionC();
            var typeC = Instantiate(EnemyTypeC, spawnPosition, Quaternion.identity);
        }
    }

    private Vector2 GenerateSpawnPositionAB()
    {
        var positionSection = UnityEngine.Random.Range(0, 3);
        float xPosition = 0;
        float yPosition = 0;

        if (positionSection == 0)
        {
            xPosition = Boundaries.LeftBoundary - 1.0f;
            yPosition = UnityEngine.Random.Range(Boundaries.LowerBoundary + 6.0f, Boundaries.UpperBoundary - 1.0f);

        }
        else if (positionSection == 1)
        {
            xPosition = UnityEngine.Random.Range(Boundaries.LeftBoundary + 1.0f, Boundaries.RightBoundary - 1.0f);
            yPosition = transform.position.y;
        }
        else if(positionSection == 2)
        {
            xPosition = Boundaries.RightBoundary + 1.0f;
            yPosition = UnityEngine.Random.Range(Boundaries.LowerBoundary + 6.0f, Boundaries.UpperBoundary - 1.0f);
        }

        return new Vector2(xPosition, yPosition);
    }

    private Vector2 GenerateSpawnPositionC()
    {
        var positionSection = UnityEngine.Random.Range(0, 2);
        float xPosition = 0;
        float yPosition = 0;

        if (positionSection == 0)
        {
            xPosition = Boundaries.LeftBoundary - 1.0f;
        }
        else if (positionSection == 1)
        {
            xPosition = Boundaries.RightBoundary + 1.0f;
        }

        yPosition = Boundaries.LowerBoundary + 6.0f;

        return new Vector2(xPosition, yPosition);
    }

    public GameObject GetCamera()
    {
        return this.camera;
    }

    public void SetCamera(GameObject cam)
    {
        camera = cam;
    }
}
