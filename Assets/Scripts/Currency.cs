using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Currency : MonoBehaviour
{
    //Public Variables
    public static int airMedals;
    public TextMeshProUGUI currencyText;

    



    //Private Variables

    // Start is called before the first frame update

    private void Awake()
    {
        
    }
    void Start()
    {
        airMedals = PlayerPrefs.GetInt("Airmedals", 0);
        currencyText.text = airMedals.ToString();
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
