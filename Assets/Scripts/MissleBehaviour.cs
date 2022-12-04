using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleBehaviour : MonoBehaviour
{
    //Public Variables
    public float missleSpeed = 2;
    public float rotateSpeed = 2;
    public AudioSource radarWarning;
    public AudioSource explosionNoise;
    public static bool creation;

    //Private Variables
    private Rigidbody playerRb;
    private Rigidbody missleRb;
    private  PlayerController playerControllerScript;
    private GameManager gameManager;
    
    
    
    
    


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody>();
        missleRb = gameObject.GetComponent<Rigidbody>();
        explosionNoise = GameObject.Find("rocketExplosion").GetComponent<AudioSource>();
        radarWarning.Play();
    }

    private void FixedUpdate()
    {
        TrackPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        MissleFuel();
        
    }

    void TrackPlayer()
    {
        Vector3 towardsPlayer = (playerRb.gameObject.transform.position - gameObject.transform.position).normalized;
        gameObject.transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
        if (playerControllerScript.isGameOver == false)
        {
            missleRb.AddForce(towardsPlayer * missleSpeed);
        }
        else
        {
            Explode();
            
        }
        
        
    }

    void MissleFuel()
    {
        if (creation == true)
        {
            StartCoroutine(MissleFizzOut());
            
        }
    }
    public void Explode()
    {
        explosionNoise.Play();
        gameManager.miniExplosion.Play();
        Destroy(gameObject);
        creation = false;
    }

    IEnumerator MissleFizzOut()
    {
        yield return new WaitForSeconds(2.2f);
        Explode();
    }
}

