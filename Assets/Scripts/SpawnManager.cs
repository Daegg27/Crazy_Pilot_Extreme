using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    //Public Variables
    public GameObject mountainPrefabs;
    public GameObject birdPrefabs;
    public GameObject runwayPrefabs;
    public GameObject misslePrefabs;
    public GameObject tankerPrefab;
    public GameObject[] plantPrefabs;
    public GameObject[] mountainsGenerated;
    public GameObject[] runwaysGenerated;
     public bool spawnActiveRunway = false;
    public bool spawnMountain = false;
    public bool spawnBird = false;
    public bool generateScenery = false;
    public bool fireAway = false;
    public bool missilesHaveStarted = false;


    //Private Variables
    private PlayerController playerControllerScript;
    private GameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //This turns all of the booleans to true to begin the game
        Invoke("StartRunways", 2.0f);
        Invoke("StartMountains", .5f);
        Invoke("StartBirds", 2.5f);
        Invoke("StartScenery", 0.5f);
        InvokeRepeating("SpawnTanker", 10.0f, 6.0f);
    }

    // Update is called once per frame
    void Update()
    {
        SpawnRunways();
        SpawnMountains();
        SpawnBirds();
        SpawnScenery();
        SpawnMissles();
        StartMissles();


    }

    void SpawnBirds()
    {
        float spawnUpper = 7.0f;
        float spawnLower = 2.0f;
        float spawnRight = 90.0f;
        float spawnLeft = 80.0f;
        float SpawnPosY = Random.Range(spawnLower, spawnUpper);
        float SpawnPosX = Random.Range(spawnLeft, spawnRight);
        Vector3 spawnPos = new Vector3(SpawnPosX, SpawnPosY, 0);

        if (spawnBird == true && playerControllerScript.isGameOver == false)
        {
            Instantiate(birdPrefabs, spawnPos, birdPrefabs.transform.rotation);
            spawnBird = false;
            StartCoroutine(SpawnBirdsTimer());
        }
        
    }

    void SpawnMountains()
    {
        float spawnUpper = -13.0f;
        float spawnLower = -11.5f;
        float spawnRight = 80.0f;
        float spawnLeft = 70.0f;
        float spawnPosY = Random.Range(spawnLower, spawnUpper);
        float spawnPosX = Random.Range(spawnLeft, spawnRight);
        Vector3 spawnPos = new Vector3(spawnPosX, spawnPosY, 0);
        if (spawnMountain == true && playerControllerScript.isGameOver == false)
        {
            Instantiate(mountainPrefabs, spawnPos, mountainPrefabs.transform.rotation);
            spawnMountain = false;
            StartCoroutine(SpawnMountainsTimer());
        }
        
    }

    void SpawnMissles()
    {
        float spawnUpper = 10.0f;
        float spawnLower = 1.0f;
        float spawnRight = -35.0f;
        float spawnLeft = -45.0f;
        float spawnPosX = Random.Range(spawnLeft, spawnRight);
        float spawnPosY = Random.Range(spawnLower, spawnUpper);
        Vector3 spawnPos = new Vector3(spawnPosX, spawnPosY, 0);
        if (fireAway == true && playerControllerScript.isGameOver == false)
        {
            Instantiate(misslePrefabs, spawnPos, misslePrefabs.transform.rotation);
            fireAway = false;
            MissleBehaviour.creation = true;
            StartCoroutine(SpawnMisslesTimer());
        }
        
    }

    void SpawnRunways()
    {
        float spawnPosX = Random.Range(55.0f, 85.0f);
        Vector3 spawnPos = new Vector3(spawnPosX, -2.85f, 0);

        runwayPrefabs.transform.localScale = new Vector3(Random.Range(3.3f, 4.5f), 1, 1);


        if (spawnActiveRunway == true && playerControllerScript.isGameOver == false)
        {
            Instantiate(runwayPrefabs, spawnPos, runwayPrefabs.transform.rotation);
            spawnActiveRunway = false;
            StartCoroutine(SpawnRunwaysTimer());
            
        }
        
    }

    void SpawnScenery()
    {
        float spawnPosX = Random.Range(65.0f, 80.0f);
        Vector3 spawnPos = new Vector3(spawnPosX, -3.0f, 6);
        int plantIndex = Random.Range(0, plantPrefabs.Length);

        if (generateScenery == true && playerControllerScript.isGameOver == false)
        {
            Instantiate(plantPrefabs[plantIndex], spawnPos, plantPrefabs[plantIndex].transform.rotation);
            generateScenery = false;
            StartCoroutine(SpawnSceneryTimer());
        }
    }

    void SpawnTanker()
    {
        float spawnUpper = 15.5f;
        float spawnLower = 14.0f;
        float spawnRight = 80.0f;
        float spawnLeft = 70.0f;
        float spawnPosX = Random.Range(spawnLeft, spawnRight);
        float spawnPosY = Random.Range(spawnLower, spawnUpper);
        Vector3 spawnPos = new Vector3(spawnPosX, spawnPosY, 0);


        if (DetermineAirRefueling() == 4 && playerControllerScript.isGameOver == false)
        {
            Instantiate(tankerPrefab, spawnPos, tankerPrefab.transform.rotation);
        }
    }


    IEnumerator SpawnRunwaysTimer()
    {
        yield return new WaitForSeconds(4);
        spawnActiveRunway = true;
        

    }

    IEnumerator SpawnMountainsTimer()
    {
        yield return new WaitForSeconds(2);
        spawnMountain = true;
    }

    IEnumerator SpawnBirdsTimer()
    {
        yield return new WaitForSeconds(BirdsAndMountains());
        spawnBird = true;
    }

    IEnumerator SpawnSceneryTimer()
    {
        yield return new WaitForSeconds(2.8f);
        generateScenery = true;
    }

    IEnumerator SpawnMisslesTimer()
    {
        yield return new WaitForSeconds(GenerateNumber());
        fireAway = true;
    }

   public float GenerateNumber()
    {
        float generatedNumber = Random.Range(8.0f, 14.0f);
        return generatedNumber;
        
    }

    public float BirdsAndMountains()
    {
        float spawnTimer = Random.Range(2.0f, 3.5f);
        return spawnTimer;
    }

    void StartRunways()
    {
        spawnActiveRunway = true;
    }

    void StartMountains()
    {
        spawnMountain = true;
    }

    void StartBirds()
    {
        spawnBird = true;
    }

    void StartScenery()
    {
        generateScenery = true;
    }

    void StartMissles()
    {
        if (gameManager.totalScore > 600 && missilesHaveStarted == false)
        {
            fireAway = true;
            missilesHaveStarted = true;
        }
       
            
    }

    int DetermineAirRefueling()
    {
        int factor = Random.Range(1, 5);

        
        return factor;
    }

}



