using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainScript : MonoBehaviour
{
    //Public Variables


    //Private Variables
    private GameObject[] numberOfBirds;

   
    // Start is called before the first frame update
    void Start()
    {
        numberOfBirds = GameObject.FindGameObjectsWithTag("Birds");
        DestoryOverlap();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {


        if (collision.gameObject.CompareTag("Runway"))
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Birds"))
        {
            Destroy(collision.gameObject);
        } 
    }

    public void DestoryOverlap()
    {
        foreach (GameObject bird in numberOfBirds)
        {
            float calculatedDistance = gameObject.transform.position.x - bird.transform.position.x;

            if (calculatedDistance >= 0 && calculatedDistance <= 12)
            {
                Destroy(bird.gameObject);
            }
        }
    }

}
