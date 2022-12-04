using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePropellor : MonoBehaviour
{
    // Public Variables
    


    //Private Variables
    private PlayerController playerControllerScript;
    private float rotateSpeed = 1000;


    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControllerScript.fuelAvailable == true)
        {
            gameObject.transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
        }
        
    }
}
