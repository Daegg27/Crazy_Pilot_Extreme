using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class GameManager : MonoBehaviour
{
    //Public Variables
    public GameObject player;
    public GameObject playerProp;
    public GameObject crashScene;
    public GameObject startScreen;
    public GameObject rocketExplosion;
    public GameObject thankYouScreen;
    public GameObject[] selectableSkins;
    public MeshRenderer playerMesh;
    public ParticleSystem miniExplosion;
    public AudioSource deathSound;
    public AudioSource refuelNoise;
    public int timeElaspedAirMedals;
    public int totalAirMedals;
    public float activeScore;
    public float totalScore;
    public float earningScore;
    public float currencyCounter;
    public float variableSpeed;
    public TextMeshProUGUI totalScoreText;
    public TextMeshProUGUI earnedScore;
    public TextMeshProUGUI highScore;
    public TextMeshProUGUI rewards;
    public TextMeshProUGUI finalRewardText;
    public TMP_FontAsset[] myFonts;
    public Button restartButton;
    public Button menuButton;
    public Button rewardButton;
    public bool highScoreAchieved;
    public bool inputDetected;
    public bool gameHasStarted;
    public bool calculationNeeded;


    [SerializeField] RewardedAdsButton rewardedAdsButton;
    public BannerAdScript bannerAdScript;
    
    
   


    //Private Variables
    private PlayerController playerControllerScript;
    private Currency currencyScript;



    private void Awake()
    {
        variableSpeed = 19;
    }


    // Start is called before the first frame update
    void Start()
    {

        

        





        if (Advertisement.isInitialized == false)
        {
            AdsInitializer.instance.InitializeAds();
        }
        rewardedAdsButton.LoadAd();

        totalScoreText.text = 0.ToString();
        earnedScore.text = activeScore.ToString("0.00");
        highScore.text = "High Score: " + PlayerPrefs.GetFloat("Highscore", 0).ToString("0.00");
        earnedScore.fontSize = 2;
        currencyCounter = 20.0f;
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        miniExplosion.transform.position = rocketExplosion.transform.position;
        
       
        
        // Add Listeners for Buttons
        restartButton.onClick.AddListener(RestartGame);
        menuButton.onClick.AddListener(ReturnToMenu);
        
        
        // Boolean initilizing
        highScoreAchieved = false;
        gameHasStarted = false;
        calculationNeeded = true;

        
        UpdatePlayerSkin();





    }

    // Update is called once per frame
    void Update()
    {
        earnedScore.transform.position = player.transform.position + new Vector3(0, 1.5f, 6);
        GameOver();
        StartDelay();
        AddCurrency();
        ScoreToAirMedals();
        CalculateTotalAirMedals();
        SearchForMissles();

    }
    private void FixedUpdate()
    {
        SetDifficulty();
    }

    public void AddScore(float ScoreToAdd)
    {
       earningScore = ScoreToAdd / 0.93f;

        activeScore += earningScore;
        totalScore += earningScore;
        if (totalScore > PlayerPrefs.GetFloat("Highscore", 0) && highScoreAchieved == false)
        {
            playerControllerScript.highscoreEffect.Play();
            highScoreAchieved = true;
        }
        if (playerControllerScript.goodLanding == false)
        {
            StartCoroutine(RefreshActiveScore());
        }
        
        totalScoreText.text = totalScore.ToString("0.00");
        earnedScore.text = activeScore.ToString("0.00");
        if (totalScore > PlayerPrefs.GetFloat("Highscore", 0))
        {
            PlayerPrefs.SetFloat("Highscore", totalScore);
        }
        if (playerControllerScript.currentTime < 0)
        {
            earnedScore.color = new Color(0f, 1f, 0f);
            earnedScore.font = myFonts[1];
        }
        
        

    }
    IEnumerator RefreshActiveScore()
    {
        yield return new WaitForSeconds(1);
        if (playerControllerScript.goodLanding == false)
        {
            activeScore = 0;
            earnedScore.text = activeScore.ToString("0.00");
            earnedScore.gameObject.SetActive(false);
            earnedScore.color = new Color(0f, 0f, 0f);
            earnedScore.font = myFonts[0];
            earnedScore.fontSize = 2;
        }
        
        
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void ReturnToMenu()
    {
         SceneManager.LoadScene("Menu");
    }
    
    public void GiveReward()
    {
        int finalReward = totalAirMedals * 2;
        finalRewardText.text = finalReward.ToString();
        thankYouScreen.gameObject.SetActive(true);

        Currency.airMedals += totalAirMedals;
        PlayerPrefs.SetInt("Airmedals", Currency.airMedals);
    }

    void GameOver()
    {
        if (playerControllerScript.isGameOver == true)
        {
            crashScene.gameObject.SetActive(true);
        }
    }

    void StartDelay()
    {
        if (inputDetected == false)
        {
            Time.timeScale = 0;
            playerControllerScript.engineNoise.Stop();
            startScreen.gameObject.SetActive(true);
        }
        else if (inputDetected == true && gameHasStarted == false)
        {
            Time.timeScale = 1;
            playerControllerScript.engineNoise.Play();
            startScreen.gameObject.SetActive(false);
            gameHasStarted = true;
        }
        
    } 
    
    void AddCurrency()
    {
        

        
        if (currencyCounter > 0 && playerControllerScript.isGameOver == false)
        {
            
            currencyCounter -= 1f * Time.deltaTime;
            
        }
        if (currencyCounter <= 0)
        {
            timeElaspedAirMedals += 1;
            currencyCounter = 20.0f;
               
        }
    }

    public int ScoreToAirMedals()
    {
        int earnedAirMedals;
        if (playerControllerScript.isGameOver == false)
        {
             return earnedAirMedals = 0;
        }
        else
        {
            return earnedAirMedals = ((int)totalScore) / 75;
            
        }
    }

    // This calculates the how many air medals you accumulated throughout the game, and upa
    void CalculateTotalAirMedals()
    {

        if (playerControllerScript.isGameOver == true && calculationNeeded == true)
        {
            totalAirMedals = timeElaspedAirMedals + ScoreToAirMedals();
            rewards.text = totalAirMedals.ToString();
            Currency.airMedals += totalAirMedals;
            LeaderboardController.scoreToUpload = (int)(totalScore * 100);
            LeaderboardController.SubmitScore();
            if (Currency.airMedals > PlayerPrefs.GetInt("Airmedals"))
            {
                PlayerPrefs.SetInt("Airmedals", Currency.airMedals);
            }
            calculationNeeded = false;
            


        }
    }

    void SearchForMissles()
    {
        GameObject missle;
        missle = GameObject.FindGameObjectWithTag("Missle");
            if (MissleBehaviour.creation == true)
        {
            rocketExplosion.transform.position = missle.transform.position;
        }
         
    }

    void UpdatePlayerSkin()
    {
        if (StoreManager.selectedSkin == 8)
        {
            playerMesh.enabled = true;
            playerProp.SetActive(true);
            playerControllerScript.rightContrail.gameObject.transform.localPosition = new Vector3(8.8f, 3.21f, -4.74f);
            playerControllerScript.leftContrail.gameObject.transform.localPosition = new Vector3(-9.1f, 3.21f, -4.74f);
        }else if (StoreManager.selectedSkin == 0)
        {
            playerMesh.enabled = false;
            playerProp.SetActive(false);
            GameObject newSkin = Instantiate(selectableSkins[0], GameObject.Find("Graphics").transform);
             playerControllerScript.rightContrail.transform.localPosition = new Vector3(5.58f, 1.64f, -5.87f);
             playerControllerScript.leftContrail.transform.localPosition = new Vector3(-5.22f, 1.62f, -7.06f);
        }
        else if (StoreManager.selectedSkin == 1)
        {
            playerMesh.enabled = false;
            playerProp.SetActive(false);
            GameObject newSkin = Instantiate(selectableSkins[1], GameObject.Find("Graphics").transform);
            playerControllerScript.rightContrail.gameObject.transform.localPosition = new Vector3(8.2f, 2.6f, -1.86f);
            playerControllerScript.leftContrail.gameObject.transform.localPosition = new Vector3(-8.62f, 2.7f, -1.98f);

        }
        else if (StoreManager.selectedSkin == 2)
        {
            playerMesh.enabled = false;
            playerProp.SetActive(false);
            GameObject newSkin = Instantiate(selectableSkins[2], GameObject.Find("Graphics").transform);
            playerControllerScript.rightContrail.gameObject.transform.localPosition = new Vector3(7.85f, 2f, -5.16f);
            playerControllerScript.leftContrail.gameObject.transform.localPosition = new Vector3(-7.4f, 2.06f, -3.96f);
        }
        else if (StoreManager.selectedSkin == 3)
        {
            playerMesh.enabled = false;
            playerProp.SetActive(false);
            GameObject newSkin = Instantiate(selectableSkins[3], GameObject.Find("Graphics").transform);
            playerControllerScript.rightContrail.gameObject.transform.localPosition = new Vector3(7.99f, 2.43f, -10.4f);
            playerControllerScript.leftContrail.gameObject.transform.localPosition = new Vector3(-7.73f, 2.43f, -9.66f);
        }
        else if (StoreManager.selectedSkin == 4)
        {
            playerMesh.enabled = false;
            playerProp.SetActive(false);
            GameObject newSkin = Instantiate(selectableSkins[4], GameObject.Find("Graphics").transform);
            playerControllerScript.rightContrail.gameObject.transform.localPosition = new Vector3(6.2f, 2.1f, -6.35f);
            playerControllerScript.leftContrail.gameObject.transform.localPosition = new Vector3(-5.67f, 2.11f, -6.12f);
        }
        else if (StoreManager.selectedSkin == 5)
        {
            playerMesh.enabled = false;
            playerProp.SetActive(false);
            GameObject newSkin = Instantiate(selectableSkins[5], GameObject.Find("Graphics").transform);
            playerControllerScript.rightContrail.gameObject.transform.localPosition = new Vector3(5.39f, 1.86f, -9.38f);
            playerControllerScript.leftContrail.gameObject.transform.localPosition = new Vector3(-5.67f, 2.05f, -9.7f);
        }
        else if (StoreManager.selectedSkin == 6)
        {
            playerMesh.enabled = false;
            playerProp.SetActive(false);
            GameObject newSkin = Instantiate(selectableSkins[6], GameObject.Find("Graphics").transform);
            playerControllerScript.rightContrail.gameObject.transform.localPosition = new Vector3(4.46f, 1.87f, -8.72f);
            playerControllerScript.leftContrail.gameObject.transform.localPosition = new Vector3(-4.36f, 2.05f, -9.21f);
        }
        else if (StoreManager.selectedSkin == 7)
        {
            playerMesh.enabled = false;
            playerProp.SetActive(false);
            GameObject newSkin = Instantiate(selectableSkins[7], GameObject.Find("Graphics").transform);
            playerControllerScript.rightContrail.gameObject.transform.localPosition = new Vector3(2.73f, 2.28f, -11f);
            playerControllerScript.leftContrail.gameObject.transform.localPosition = new Vector3(-2.08f, 2.28f, -10.78f);
        }
    }

    void SetDifficulty()
    {
        if (totalScore <= 300)
        {
            if (variableSpeed >= 18 && playerControllerScript.onRunway == true)
            {
                variableSpeed -= 0.03f;
            }
            if (variableSpeed <= 19 && playerControllerScript.onRunway == false)
            {
                variableSpeed += 0.03f;
            }
        }
        else
        {
            if (variableSpeed <= 19)
            {
                variableSpeed += 0.03f;
            }
        }
        
    }

    
   
}
