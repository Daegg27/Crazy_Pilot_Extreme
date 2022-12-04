using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    //Public Variables


    //Private Variables
    private Vector3 startPos;
    private float repeatWidth;

    
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < startPos.x - 92)
        {
            transform.position = startPos;
        }
    }
}
