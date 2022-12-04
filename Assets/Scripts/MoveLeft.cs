using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    //Public Variables
    

    //Private Variable
    private PlayerController playerControllerScript;
    private GameManager gameManager;
    private float speed = 19;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (playerControllerScript.isGameOver == false)
        {
            gameObject.transform.Translate(Vector3.left * gameManager.variableSpeed * Time.deltaTime, Space.World);
        }

    }
}
