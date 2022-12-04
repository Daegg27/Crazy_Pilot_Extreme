using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteShadow : MonoBehaviour
{
    public SpriteRenderer rendererS;
    public bool receiveShadows;
    
   
    // Start is called before the first frame update
    void Start()
    {
        rendererS.receiveShadows = receiveShadows;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
