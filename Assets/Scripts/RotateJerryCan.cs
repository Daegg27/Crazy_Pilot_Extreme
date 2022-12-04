using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateJerryCan : MonoBehaviour
{

    private PlayerController playerControllerScript;


    private int rotateSpeed = 150;
    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControllerScript.isGameOver == false)
        {
            gameObject.transform.Rotate(Vector3.down * rotateSpeed * Time.deltaTime);
        }
        
    }
}
