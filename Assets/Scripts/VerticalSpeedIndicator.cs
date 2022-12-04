using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VerticalSpeedIndicator : MonoBehaviour
{
    //Public Variables
    public Image indicatorUp;
    public Image indicatorDown;


    //Private Variables
    private Rigidbody playerRb;
    

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody>();
        indicatorUp.fillAmount = 0;
        indicatorDown.fillAmount = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        DisplaySpeed();
    }
    void DisplaySpeed()
    {
        // This displays your climb rate
        if (playerRb.velocity.y > 0)
        {
            indicatorUp.fillAmount = Mathf.Clamp(playerRb.velocity.y / 20, 0, 1);
        }

        // This displays your descent rate
        if (playerRb.velocity.y < 0)
        {
            indicatorDown.fillAmount = Mathf.Clamp(playerRb.velocity.y / -20, 0, 1);
        }
    }
}
