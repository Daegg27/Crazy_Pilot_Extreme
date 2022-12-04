using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DestroyGameObjects();
    }
    void DestroyGameObjects()
    {
        if (gameObject.transform.position.x < -60)
        {
            Destroy(gameObject);
        }
        if (gameObject.transform.position.y > 28)
        {
            Destroy(gameObject);
        }
    }
}
