using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class VerifyInternet : MonoBehaviour
{
    public static VerifyInternet instance;


    [SerializeField] private Canvas errorCanvas;


    public bool internetConnected;
    private int failedAttempts;
    private float countTimer;


    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        
    }

    private void Start()
    {
        internetConnected = true;
        InvokeRepeating("CheckInternetConnection", .1f, 5f);
    }

    private void Update()
    {

        if (internetConnected == false && PlayerPrefs.GetInt("NoAds", 0) == 0) 
        {
            countTimer += Time.deltaTime;
            errorCanvas.gameObject.SetActive(true);
        }
        if (internetConnected == true)
        {
            failedAttempts = 0;
            errorCanvas.gameObject.SetActive(false);
        }
        
       while (internetConnected == false && countTimer >= 5.0f)
        {
            Invoke("CheckForBoot", 0.1f);
            countTimer = 0.0f;
            
        }

        CloseApp();
    }

    public void CheckInternetConnection()
    {
        StartCoroutine(CheckRoutine());
    }

    public void CheckForBoot()
    {
        StartCoroutine(ConfirmNoInternet());
    }


    IEnumerator CheckRoutine()
    {
        UnityWebRequest request = new UnityWebRequest("https://www.google.com/");
        yield return request.SendWebRequest();

        if (string.IsNullOrEmpty(request.error))
        {
            internetConnected = true;
        }
        else
        {
            internetConnected = false;
        }
    }

    IEnumerator ConfirmNoInternet()
    {
        UnityWebRequest request = new UnityWebRequest("https://www.google.com/");
        yield return request.SendWebRequest();

        if (string.IsNullOrEmpty(request.error))
        {
            failedAttempts = 0;
            Debug.Log("Welcome back: " + failedAttempts);
        }
        else
        {
            failedAttempts += 1;
            Debug.Log("Still no connection: " + failedAttempts);
        }
    }


    void CloseApp()
    {
        if (failedAttempts > 3)
        {
            Application.Quit();
        }
    }

}
