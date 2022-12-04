using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LootLocker.Requests;
using UnityEngine.Advertisements;
using System.IO;

public class MenuManager : MonoBehaviour
{

    public BackupData backupData;
    private string backupDataPath;

    //Public Variables
    public bool audioIsMuted;
    public bool musicIsMuted;
    public bool waitForCycle;
    public static bool controlsAreInverted;
    public Button playButton;
    public Button storeButton;
    public Button controlButton;
    public Button leaderboardsButton;
    public Button optionsButton;
    public Button availableCurrency;
    public Button backButton;
    public GameObject secondBackground;
    public GameObject gameControlsLayout;
    public GameObject leaderboardLayout;
    public GameObject optionsLayout;
    public GameObject legend;
    public GameObject container;
    public GameObject cellPrefabs;
    public TMP_InputField nameChanger;
    public TextMeshProUGUI personalScore;
    public TextMeshProUGUI personalName;
    public TextMeshProUGUI activeGameTip;
    public Toggle audioListenerToggle;
    public Toggle musicOffToggle;
    public Toggle invertControls;
    public Slider masterVolumeSlider;

    public AudioSource buttonNoise;



    //Private Variables
    private float gameVolume;
    private string tipNumberOne = "Tip: Use the shadows to help look for threats ahead!";
    private string tipNumberTwo = "Tip: Mess around with the flare button to master control of your aircraft!";
    private string tipNumberThree = "Follow @Crazy_PilotGame on Twitter and let me know how you're enjoying the game.";
    private string tipNumberFour = "Tip: Applying the throttle/brake in short burst is WAY more effective than constant pressure.";
    private string tipNumberFive = "Tip: Fly above the tanker with extreme caution!";
    private string tipNumberSix = "Tip: If you're dying a lot after touching the runway, make sure to hold flare!";
    private string[] gameTips;




    // Start is called before the first frame update
    void Start()
    {

        backupDataPath = $"{Application.persistentDataPath}/MissileTexture.json";

        CreateInitialBackup();
        LoadBackup();



        if (Advertisement.isInitialized == false)
        {
            AdsInitializer.instance.InitializeAds();
        }

        gameTips = new string[6];
        gameTips[0] = tipNumberOne;
        gameTips[1] = tipNumberTwo;
        gameTips[2] = tipNumberThree;
        gameTips[3] = tipNumberFour;
        gameTips[4] = tipNumberFive;
        gameTips[5] = tipNumberSix;
        Application.targetFrameRate = 40;
        StoreManager.selectedSkin = PlayerPrefs.GetInt("currentSkin", 8);
        playButton.onClick.AddListener(StartGame);
        storeButton.onClick.AddListener(GoToStore);
        controlButton.onClick.AddListener(ViewControls);
        leaderboardsButton.onClick.AddListener(ViewLeaderboards);
        leaderboardsButton.onClick.AddListener(RetrieveScores);
        optionsButton.onClick.AddListener(ViewOptions);
        backButton.onClick.AddListener(ResetLeaderboard);
        backButton.onClick.AddListener(RemoveSecondBackground);
        nameChanger.text = PlayerPrefs.GetString("PlayerName", "EnterName");
        personalScore.text = "Your High Score is: " + PlayerPrefs.GetFloat("Highscore").ToString("0.00");
        personalName.text = "Your name is: " + "\n" + PlayerPrefs.GetString("PlayerName");
        gameVolume = PlayerPrefs.GetFloat("KeyVolume", 1);
        masterVolumeSlider.value = gameVolume;
        audioIsMuted = (PlayerPrefs.GetInt("masterVolume", 0) != 0);
        musicIsMuted = (PlayerPrefs.GetInt("musicVolume", 0) != 0);
        controlsAreInverted = (PlayerPrefs.GetInt("NonPilotControls", 0) != 0);
        PreloadSettings();





    }

    // Update is called once per frame
    void Update()
    {
        ManageSettings();
        UpdateActiveTip();
    }


    void ManageSettings()
    {
        //This is for turning the master volume off 
        if (audioListenerToggle.isOn)
        {
            audioIsMuted = true;
        }
        else
        {
            audioIsMuted = false;
        }

        PlayerPrefs.SetInt("masterVolume", audioIsMuted ? 1 : 0);

        if (audioIsMuted == true)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = gameVolume;
        }
        //This deals with only the background music
        if (musicOffToggle.isOn)
        {
            musicIsMuted = true;
        }
        else
        {
            musicIsMuted = false;
        }
        PlayerPrefs.SetInt("musicVolume", musicIsMuted ? 1 : 0);

        if (musicIsMuted == true)
        {
            AudioController.instance.menuMusic.volume = 0;
        }
        else
        {
            AudioController.instance.menuMusic.volume = 1;
        }
        //This deals with inverting the controls
        if (invertControls.isOn)
        {
            controlsAreInverted = true;
        }
        else
        {
            controlsAreInverted = false;
        }
        PlayerPrefs.SetInt("NonPilotControls", controlsAreInverted ? 1 : 0);
    }

    void PreloadSettings()
    {
        if (audioIsMuted)
        {
            audioListenerToggle.isOn = true;
        }
        if (musicIsMuted)
        {
            musicOffToggle.isOn = true;
        }
        if (controlsAreInverted)
        {
            invertControls.isOn = true;
        }
    }

    public void UpdateVolume(float volume)
    {
        gameVolume = volume;
        PlayerPrefs.SetFloat("KeyVolume", volume);
    }

    void StartGame()
    {
        buttonNoise.Play();
        SceneManager.LoadScene("My Game");
    }

    void GoToStore()
    {
        buttonNoise.Play();
        SceneManager.LoadScene("Store");
    }

    void ViewControls()
    {
        buttonNoise.Play();
        secondBackground.SetActive(true);
        gameControlsLayout.SetActive(true);
        playButton.gameObject.SetActive(false);
        optionsButton.gameObject.SetActive(false);
        controlButton.gameObject.SetActive(false);
        storeButton.gameObject.SetActive(false);
        nameChanger.gameObject.SetActive(false);
        availableCurrency.gameObject.SetActive(false);
        activeGameTip.gameObject.SetActive(false);
    }

    void ViewLeaderboards()
    {
        buttonNoise.Play();
        secondBackground.gameObject.SetActive(true);
        leaderboardLayout.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false);
        leaderboardsButton.gameObject.SetActive(false);
        optionsButton.gameObject.SetActive(false);
        controlButton.gameObject.SetActive(false);
        storeButton.gameObject.SetActive(false);
        nameChanger.gameObject.SetActive(false);
        availableCurrency.gameObject.SetActive(false);
        activeGameTip.gameObject.SetActive(false);
    }

    void ViewOptions()
    {
        buttonNoise.Play();
        secondBackground.gameObject.SetActive(true);
        optionsLayout.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false);
        leaderboardsButton.gameObject.SetActive(false);
        optionsButton.gameObject.SetActive(false);
        controlButton.gameObject.SetActive(false);
        storeButton.gameObject.SetActive(false);
        nameChanger.gameObject.SetActive(false);
        availableCurrency.gameObject.SetActive(false);
        activeGameTip.gameObject.SetActive(false);
    }

    void RemoveSecondBackground()
    {
        buttonNoise.Play();
        secondBackground.SetActive(false);
        gameControlsLayout.SetActive(false);
        leaderboardLayout.SetActive(false);
        optionsLayout.gameObject.SetActive(false);
        controlButton.gameObject.SetActive(true);
        storeButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);
        leaderboardsButton.gameObject.SetActive(true);
        optionsButton.gameObject.SetActive(true);
        nameChanger.gameObject.SetActive(true);
        availableCurrency.gameObject.SetActive(true);
        activeGameTip.gameObject.SetActive(true);
    }

    void RetrieveScores()
    {

        int scoreRank;
        string playerName;
        float playerScore;


        LootLockerSDKManager.GetScoreListMain(LeaderboardController.leaderboardID, 100, 0, (response) =>
        {
            if (response.statusCode == 200)
            {
                Debug.Log("Pulled Scores");


                LootLockerLeaderboardMember[] members = response.items;

                for (int i = 0; i < members.Length; i++)
                {
                    var cellScript = cellPrefabs.GetComponent<CellScript>();
                    scoreRank = members[i].rank;
                    playerScore = (float)members[i].score / 100;
                    playerName = members[i].metadata;
                    cellScript.playerRank = scoreRank.ToString();
                    cellScript.playerName = playerName;
                    cellScript.playerScore = playerScore.ToString("0.00");
                    Instantiate(cellPrefabs, container.transform);
                }
            }
            else
            {
                var cellScript = cellPrefabs.GetComponent<CellScript>();

                Debug.Log("Could not read scores");
                scoreRank = 0;
                playerScore = 0.00f;
                playerName = "Please Retry";
                cellScript.playerRank = scoreRank.ToString();
                cellScript.playerName = playerName;
                cellScript.playerScore = playerScore.ToString("0.00");
                Instantiate(cellPrefabs, container.transform);

            }

        });

    }

    void ResetLeaderboard()
    {
        GameObject[] activeCells;

        activeCells = GameObject.FindGameObjectsWithTag("Cell");

        foreach (GameObject cells in activeCells)
        {
            Destroy(cells.gameObject);
        }
    }

    public void UpdatePlayerName()
    {
        PlayerPrefs.SetString("PlayerName", nameChanger.text);
        if (PlayerPrefs.GetString("PlayerName").Length == 0)
        {
            PlayerPrefs.SetString("PlayerName", "Guest#" + PlayerPrefs.GetString("PlayerID"));
            nameChanger.text = PlayerPrefs.GetString("PlayerName");
        }

        personalName.text = "Your name is: " + "\n" + PlayerPrefs.GetString("PlayerName");
    }

    void UpdateActiveTip()
    {
        if (waitForCycle == false)
        {
            activeGameTip.text = gameTips[Random.Range(0, gameTips.Length)];
            waitForCycle = true;
            StartCoroutine(CycleCurrentTip());
        }
    }

    IEnumerator CycleCurrentTip()
    {
        yield return new WaitForSeconds(4.0f);
        waitForCycle = false;
    }

    void CreateInitialBackup()
    {
        backupDataPath = $"{Application.persistentDataPath}/MissileTexture.json";

        if (!File.Exists(backupDataPath) && PlayerPrefs.HasKey("Airmedals") || PlayerPrefs.HasKey("NoAds"))
        {
            if (PlayerPrefs.GetInt("NoAds", 0) == 1)
            {
                backupData.adsRemoved = true;
            }
            else
            {
                backupData.adsRemoved = false;
            }
            
            backupData.currencyEarned = PlayerPrefs.GetInt("Airmedals");
            
            SaveJson();
        }
    }

    void LoadBackup()
    {
        if (File.Exists(backupDataPath) && PlayerPrefs.HasKey("NoAds") == false && PlayerPrefs.HasKey("Airmedals") == false)
        {
            LoadJson();

            PlayerPrefs.SetInt("Airmedals", backupData.currencyEarned);

            if (backupData.adsRemoved == false)
            {
                PlayerPrefs.SetInt("NoAds", 0);
            }
            else if (backupData.adsRemoved == true)
            {
                PlayerPrefs.SetInt("NoAds", 1);
            }
        }
    
    
    }


    void LoadJson()
    {
       
            string json = File.ReadAllText(backupDataPath);
            backupData = JsonUtility.FromJson<BackupData>(json);
        
    }

    private void SaveJson()
    {
        string json = JsonUtility.ToJson(backupData);
        File.WriteAllText(backupDataPath, json);
    }



}
