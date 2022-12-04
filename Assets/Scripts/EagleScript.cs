using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleScript : MonoBehaviour
{
    private GameObject[] numberOfMountains;
    private PlayerController playerControllerScript;
    


    public Animator eagleOne;
    public Animator eagleTwo;
    public Animator eagleThree;
    


    

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        numberOfMountains = GameObject.FindGameObjectsWithTag("Mountain");
        DestoryOverlap();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControllerScript.isGameOver == true)
        {
            eagleOne.speed = 0;
            eagleTwo.speed = 0;
            eagleThree.speed = 0;
        }
        
    }

    public void DestoryOverlap()
    {
        foreach (GameObject mountain in numberOfMountains)
        {
            float calculatedDistance = mountain.transform.position.x - gameObject.transform.position.x;

            if (calculatedDistance >= 0 && calculatedDistance <= 12)
            {
                Destroy(gameObject);
            }
        }

 
    }
}
