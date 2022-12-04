using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissleIndicator : MonoBehaviour
{

    //Public Variables
    public GameObject missileIndicatorPlaceholder;
    public Image missileIndicator;
    


    //Private Variables
    private GameObject player;
    private GameObject activeMissle;




    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        missileIndicatorPlaceholder.transform.position = player.transform.position;
        SearchForMissles();
       
    }

    void SearchForMissles()
    {
       
        
        

        if (MissleBehaviour.creation == true)
        {
            activeMissle = GameObject.FindGameObjectWithTag("Missle");
            missileIndicator.gameObject.SetActive(true);
            ConfigureIndicator();
            
        }
        else
        {
            missileIndicator.gameObject.SetActive(false);
            
        }

    }
    void ConfigureIndicator()
    {
        float towardsMissle = (player.transform.position.y) - activeMissle.transform.position.y;

        missileIndicator.transform.localEulerAngles = new Vector3(0, 0, towardsMissle);
    }
}
