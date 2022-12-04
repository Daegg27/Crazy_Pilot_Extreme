using System.IO;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections.Generic;

public class StoreManager : MonoBehaviour
{

    public UnlockableSkins unlockableSkins;
    public BackupData backupData;
    public static StoreManager instance;


    //Public Variables
    public AudioSource buttonNoise;
    public AudioSource purchaseNoise;
    public Button backButton;
    public Button playButton;
    public Button currencyButton;
    public Button purchaseButton;
    public Button randomizerButton;
    public Button noButton;
    public Button yesButton;
    public Button removeAdsButton;
    public Button cessnaButton;
    public Button airlinerButton;
    public Button mustangButton;
    public Button flyingFortressButton;
    public Button stratotankerButton;
    public Button warthogButton;
    public Button eagleButton;
    public Button viperButton;
    public Button blackbirdButton;
    public GameObject cessna;
    public GameObject airliner;
    public GameObject mustang;
    public GameObject stratotanker;
    public GameObject flyingFortress;
    public GameObject warthog;
    public GameObject eagle;
    public GameObject viper;
    public GameObject blackbird;
    public GameObject confirmPurchase;
    public GameObject[] listOfSkins;
    public GameObject[] listOfNames;
    public List<int> randomizerList;
    public Button[] listOfButtons;
    public GameObject[] purchasedIndicators;
    public GameObject[] unpurchasedIndicators;
    public TextMeshProUGUI testText;
    public TextMeshProUGUI confirmPurchaseText;
    public TextMeshProUGUI availableAirmedals;
    public static int selectedSkin = 8;
    private int desiredSkin = 8;
    public bool scaledUp;
    public bool windowShopping;
   




    //Private Variables
    private string nameOfSkin;
    private string nameOfDesiredSkin;
    private float scaleAmount = 1.3f;
    private int rotateSpeed = 15;

    private string unlockSkinsPath;
    private string backupDataPath;

    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        if (Advertisement.isInitialized == false)
        {
            AdsInitializer.instance.InitializeAds();
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        

        unlockSkinsPath = $"{Application.persistentDataPath}/DefaultTextures.json";
        backupDataPath = $"{Application.persistentDataPath}/MissileTexture.json";

        if (File.Exists(unlockSkinsPath))
        {
            string json = File.ReadAllText(unlockSkinsPath);
            unlockableSkins = JsonUtility.FromJson<UnlockableSkins>(json);
        }
        if (File.Exists(backupDataPath))
        {
            string json = File.ReadAllText(backupDataPath);
            backupData = JsonUtility.FromJson<BackupData>(json);
            UpdateBackup();
            
        }

        GenerateRandomLogic();
        randomizerButton.onClick.AddListener(RandomButton);
        backButton.onClick.AddListener(ReturnToMenu);
        playButton.onClick.AddListener(StartGame);
        purchaseButton.onClick.AddListener(StartPurchase);
        noButton.onClick.AddListener(DeclinePurchase);
        yesButton.onClick.AddListener(FinalizePurchase);
        selectedSkin = PlayerPrefs.GetInt("currentSkin", 8);
        HideSelectedName();
        ChangeButtons();
        


    }

    // Update is called once per frame
    void Update()
    {
        ButtonNoise();
        TrackSkin();
        FindActiveSkin();
        RotateDesiredSkin();
        CheckForAdsRemoved();
        
    }

    

    public void ChangeButtons()
    {
       
        
        //Allows you to play as the cessna
        if (unlockableSkins.cessnaUnlocked == true)
        {
            cessnaButton.onClick.AddListener(SelectCessna);
            purchasedIndicators[8].gameObject.SetActive(true);
        }
        //Allows you to play as the airliner
        if (unlockableSkins.airlinerUnlocked == true)
        {
            airlinerButton.onClick.AddListener(SelectAirliner);
            purchasedIndicators[0].gameObject.SetActive(true);
        }
        else
        {
            airlinerButton.onClick.AddListener(DisplayAirliner);
            unpurchasedIndicators[0].gameObject.SetActive(true);
        }
        //Allows you to play as the Mustang
        if (unlockableSkins.mustangUnlocked == true)
        {
            mustangButton.onClick.AddListener(SelectMustang);
            purchasedIndicators[1].gameObject.SetActive(true);
        }
        else
        {
            mustangButton.onClick.AddListener(DisplayMustang);
            unpurchasedIndicators[1].gameObject.SetActive(true);
        }
        //Allows you to play as the Flying Fortress
        if (unlockableSkins.flyingFortressUnlocked == true)
        {
            flyingFortressButton.onClick.AddListener(SelectFlyingFortress);
            purchasedIndicators[2].gameObject.SetActive(true);
        }
        else
        {
            flyingFortressButton.onClick.AddListener(DisplayFlyingFortress);
            unpurchasedIndicators[2].gameObject.SetActive(true);
        }
        //Allows you to play as the KC-135
        if (unlockableSkins.stratotankerUnlocked == true)
        {
            stratotankerButton.onClick.AddListener(SelectStratotanker);
            purchasedIndicators[3].gameObject.SetActive(true);
        }
        else
        {
            stratotankerButton.onClick.AddListener(DisplayStratotanker);
            unpurchasedIndicators[3].gameObject.SetActive(true);
        }
        //Allows you to play as the Warthog
        if (unlockableSkins.warthogUnlocked == true)
        {
            warthogButton.onClick.AddListener(SelectWarthog);
            purchasedIndicators[4].gameObject.SetActive(true);
        }
        else
        {
            warthogButton.onClick.AddListener(DisplayWarthog);
            unpurchasedIndicators[4].gameObject.SetActive(true);
        }
        //Allows you to play as the F-15 Eagle
        if (unlockableSkins.eagleUnlocked == true)
        {
            eagleButton.onClick.AddListener(SelectEagle);
            purchasedIndicators[5].gameObject.SetActive(true);
        }
        else
        {
            eagleButton.onClick.AddListener(DisplayEagle);
            unpurchasedIndicators[5].gameObject.SetActive(true);
        }
        //Allows you to play as the F-16 Viper
        if (unlockableSkins.viperUnlocked == true)
        {
            viperButton.onClick.AddListener(SelectViper);
            purchasedIndicators[6].gameObject.SetActive(true);
        }
        else
        {
            viperButton.onClick.AddListener(DisplayViper);
            unpurchasedIndicators[6].gameObject.SetActive(true);
        }
        //Allows you to play as the Blackbird
        if (unlockableSkins.blackbirdUnlocked == true)
        {
            blackbirdButton.onClick.AddListener(SelectBlackbird);
            purchasedIndicators[7].gameObject.SetActive(true);
        }
        else
        {
            blackbirdButton.onClick.AddListener(DisplayBlackird);
            unpurchasedIndicators[7].gameObject.SetActive(true);
        }

        purchasedIndicators[selectedSkin].gameObject.SetActive(false);
    }

    public void TrackSkin()
    {
        PlayerPrefs.SetInt("currentSkin", selectedSkin);
        if (selectedSkin == 8)
        {
            nameOfSkin = "Private Jet";
        }
        else if (selectedSkin == 0)
        {
            nameOfSkin = "Airliner";
        }
        else if (selectedSkin == 1)
        {
            nameOfSkin = "P-51 Mustang";
        }
        else if (selectedSkin == 2)
        {
            nameOfSkin = "B-17 Flying Fortress";
        }
        else if(selectedSkin == 3)
        {
            nameOfSkin = "KC-135 Stratotanker";
        }
        else if (selectedSkin == 4)
        {
            nameOfSkin = "A-10 Warthog";
        }
        else if (selectedSkin == 5)
        {
            nameOfSkin = "F-15 Eagle";
        }
        else if (selectedSkin == 6)
        {
            nameOfSkin = "F-16 Viper";
        }
        else if (selectedSkin == 7)
        {
            nameOfSkin = "SR-71 Blackbird";
        }
        
        testText.text = nameOfSkin;
        
        // This deals with displaying which skin will be purchased when confirming the purchase
        
        if (desiredSkin == 8)
        {
            nameOfDesiredSkin = "Private Jet";
        }
        else if (desiredSkin == 0)
        {
            nameOfDesiredSkin = "Airliner";
        }
        else if (desiredSkin == 1)
        {
            nameOfDesiredSkin = "P-51 Mustang";
        }
        else if (desiredSkin == 2)
        {
            nameOfDesiredSkin = "B-17 Flying Fortress";
        }
        else if (desiredSkin == 3)
        {
            nameOfDesiredSkin = "KC-135 Stratotanker";
        }
        else if (desiredSkin == 4)
        {
            nameOfDesiredSkin = "A-10 Warthog";
        }
        else if (desiredSkin == 5)
        {
            nameOfDesiredSkin = "F-15 Eagle";
        }
        else if (desiredSkin == 6)
        {
            nameOfDesiredSkin = "F-16 Viper";
        }
        else if (desiredSkin == 7)
        {
            nameOfDesiredSkin = "SR-71 Blackbird";
        }

    }

    void ReturnToMenu()
    {
        buttonNoise.Play();
        SceneManager.LoadScene("Menu");
    }

    void StartGame()
    {
        buttonNoise.Play();
        SceneManager.LoadScene("My Game");
    }
   
    void ButtonNoise()
    {
        if (selectedSkin != PlayerPrefs.GetInt("currentSkin", 8))
        {
            buttonNoise.Play();
        }
    }
    void SelectCessna()
    {
        listOfSkins[selectedSkin].transform.localScale /= scaleAmount;
        selectedSkin = 8;
        HideActiveIndicator();
        scaledUp = false;
        purchaseButton.gameObject.SetActive(false);
        windowShopping = false;
        ResetInactiveSkins();
        HideSelectedName();
    }

    void SelectAirliner()
    {
        listOfSkins[selectedSkin].transform.localScale /= scaleAmount;
        selectedSkin = 0;
        HideActiveIndicator();
        scaledUp = false;
        purchaseButton.gameObject.SetActive(false);
        windowShopping = false;
        ResetInactiveSkins();
        HideSelectedName();
    }

    void SelectMustang()
    {
        listOfSkins[selectedSkin].transform.localScale /= scaleAmount;
        selectedSkin = 1;
        HideActiveIndicator();
        scaledUp = false;
        purchaseButton.gameObject.SetActive(false);
        windowShopping = false;
        ResetInactiveSkins();
        HideSelectedName();
    }

    void SelectFlyingFortress()
    {
        listOfSkins[selectedSkin].transform.localScale /= scaleAmount;
        selectedSkin = 2;
        HideActiveIndicator();
        scaledUp = false;
        purchaseButton.gameObject.SetActive(false);
        windowShopping = false;
        ResetInactiveSkins();
        HideSelectedName();
    }

    void SelectStratotanker()
    {
        listOfSkins[selectedSkin].transform.localScale /= scaleAmount;
        selectedSkin = 3;
        HideActiveIndicator();
        scaledUp = false;
        purchaseButton.gameObject.SetActive(false);
        windowShopping = false;
        ResetInactiveSkins();
        HideSelectedName();
    }

    void SelectWarthog()
    {
        listOfSkins[selectedSkin].transform.localScale /= scaleAmount;
        selectedSkin = 4;
        HideActiveIndicator();
        scaledUp = false;
        purchaseButton.gameObject.SetActive(false);
        windowShopping = false;
        ResetInactiveSkins();
        HideSelectedName();
    }

    void SelectEagle()
    {
        listOfSkins[selectedSkin].transform.localScale /= scaleAmount;
        selectedSkin = 5;
        HideActiveIndicator();
        scaledUp = false;
        purchaseButton.gameObject.SetActive(false);
        windowShopping = false;
        ResetInactiveSkins();
        HideSelectedName();
    }

    void SelectViper()
    {
        listOfSkins[selectedSkin].transform.localScale /= scaleAmount;
        selectedSkin = 6;
        HideActiveIndicator();
        scaledUp = false;
        purchaseButton.gameObject.SetActive(false);
        windowShopping = false;
        ResetInactiveSkins();
        HideSelectedName();
    }

    void SelectBlackbird()
    {
        listOfSkins[selectedSkin].transform.localScale /= scaleAmount;
        selectedSkin = 7;
        HideActiveIndicator();
        scaledUp = false;
        purchaseButton.gameObject.SetActive(false);
        windowShopping = false;
        ResetInactiveSkins();
        HideSelectedName();
    }

    void HideActiveIndicator()
    {
        for (int i = 0; i < purchasedIndicators.Length; i++)
        {
            if (i == selectedSkin)
            {
                purchasedIndicators[i].gameObject.SetActive(false);
            }
            else
            {
                continue;
            }
        }
    }

    void FindActiveSkin()
    {
        if (windowShopping == false && selectedSkin != 3)
        {
            listOfSkins[selectedSkin].transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
        }
        else if (windowShopping == false && selectedSkin == 3)
        {
            listOfSkins[selectedSkin].transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
        }
        
        if (scaledUp == false)
        {
            listOfSkins[selectedSkin].transform.localScale *= scaleAmount;
            scaledUp = true;
        }


        
         
    }

    void RotateDesiredSkin()
    {
        if (windowShopping == true && desiredSkin != 3)
        {
            listOfSkins[desiredSkin].transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
        }
        else if (windowShopping == true && desiredSkin == 3)
        {
            listOfSkins[desiredSkin].transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
        }

    }

    void ResetInactiveSkins()
    {
        //This resets the now inactive skins
        for (int i = 0; i < listOfSkins.Length; i++)
        {
            if (i == selectedSkin)
            {
                continue;
            }
            if (i == 1)
            {
                listOfSkins[i].transform.localEulerAngles = new Vector3(10, 0, 0);
            }
            else if (i == 3)
            {
                listOfSkins[i].transform.localEulerAngles = new Vector3(-89.98f, 0, 0);
            }
            else
            {
                listOfSkins[i].transform.localRotation = new Quaternion(0, 0, 0, 0);
            }

        }
        //This resets the indicator of the unselected skin
        if (unlockableSkins.cessnaUnlocked == true && selectedSkin != 8)
        {
            purchasedIndicators[8].gameObject.SetActive(true);
        }
        if (unlockableSkins.airlinerUnlocked == true && selectedSkin != 0)
        {
            purchasedIndicators[0].gameObject.SetActive(true);
        }
        if (unlockableSkins.mustangUnlocked == true && selectedSkin != 1)
        {
            purchasedIndicators[1].gameObject.SetActive(true);
        }
        if (unlockableSkins.flyingFortressUnlocked == true && selectedSkin != 2)
        {
            purchasedIndicators[2].gameObject.SetActive(true);
        }
        if (unlockableSkins.stratotankerUnlocked == true && selectedSkin != 3)
        {
            purchasedIndicators[3].gameObject.SetActive(true);
        }
        if (unlockableSkins.warthogUnlocked == true && selectedSkin != 4)
        {
            purchasedIndicators[4].gameObject.SetActive(true);
        }
        if (unlockableSkins.eagleUnlocked == true && selectedSkin != 5)
        {
            purchasedIndicators[5].gameObject.SetActive(true);
        }
        if (unlockableSkins.viperUnlocked == true && selectedSkin != 6)
        {
            purchasedIndicators[6].gameObject.SetActive(true);
        }
        if (unlockableSkins.blackbirdUnlocked == true && selectedSkin != 7)
        {
            purchasedIndicators[7].gameObject.SetActive(true);
        }
    }

    void HideSelectedName()
    {
        for (int i = 0; i < listOfSkins.Length; i++)
        {
            if (i == selectedSkin)
            {
                listOfNames[i].gameObject.SetActive(false);
            }
            else
            {
                listOfNames[i].gameObject.SetActive(true);
            }

        }
    }

    void ResetDisplayedSkins()
    {
        for (int i = 0; i < listOfSkins.Length; i++)
        {
            if (i == selectedSkin)
            {
                continue;
            }
            if (i == desiredSkin)
            {
                continue;
            }
            if (i == 8)
            {
                continue;
            }
            if (i == 1)
            {
                listOfSkins[i].transform.localEulerAngles = new Vector3(10, 0, 0);
            }
            else if (i == 3)
            {
                listOfSkins[i].transform.localEulerAngles = new Vector3(-89.98f, 0, 0);
            }
            else
            {
                listOfSkins[i].transform.localRotation = new Quaternion(0, 0, 0, 0);
            }

        }
    }




    public void DisplayAirliner()
    {
        purchaseButton.gameObject.SetActive(true);
        desiredSkin = 0;
        windowShopping = true;
        ResetDisplayedSkins();
    }

    public void DisplayMustang()
    {
        purchaseButton.gameObject.SetActive(true);
        desiredSkin = 1;
        windowShopping = true;
        ResetDisplayedSkins();
    }

    public void DisplayFlyingFortress()
    {
        purchaseButton.gameObject.SetActive(true);
        desiredSkin = 2;
        windowShopping = true;
        ResetDisplayedSkins();
    }

    public void DisplayStratotanker()
    {
        purchaseButton.gameObject.SetActive(true);
        desiredSkin = 3;
        windowShopping = true;
        ResetDisplayedSkins();
    }

    public void DisplayWarthog()
    {
        purchaseButton.gameObject.SetActive(true);
        desiredSkin = 4;
        windowShopping = true;
        ResetDisplayedSkins();
    }

    public void DisplayEagle()
    {
        purchaseButton.gameObject.SetActive(true);
        desiredSkin = 5;
        windowShopping = true;
        ResetDisplayedSkins();
    }

    public void DisplayViper()
    {
        purchaseButton.gameObject.SetActive(true);
        desiredSkin = 6;
        windowShopping = true;
        ResetDisplayedSkins();
    }

    public void DisplayBlackird()
    {
        purchaseButton.gameObject.SetActive(true);
        desiredSkin = 7;
        windowShopping = true;
        ResetDisplayedSkins();
    }

    void StartPurchase()
    {

        

        if (Currency.airMedals >= 700)
        {
            buttonNoise.Play();
            confirmPurchase.gameObject.SetActive(true);
            confirmPurchaseText.text = "Confirm purchase of:" + "\n" + nameOfDesiredSkin;
        }
    }

    void FinalizePurchase()
    {

        
      
        if (desiredSkin == 0)
        {
            unlockableSkins.airlinerUnlocked = true;
            Currency.airMedals -= 700;
            listOfSkins[selectedSkin].transform.localScale /= scaleAmount;
            selectedSkin = 0;
            purchasedIndicators[selectedSkin].gameObject.SetActive(false);
            scaledUp = false;
            windowShopping = false;
            ResetInactiveSkins();
            HideSelectedName();
            purchaseButton.gameObject.SetActive(false);
            confirmPurchase.gameObject.SetActive(false);
        }
        else if (desiredSkin == 1)
        {
            unlockableSkins.mustangUnlocked = true;
            Currency.airMedals -= 700;
            listOfSkins[selectedSkin].transform.localScale /= scaleAmount;
            selectedSkin = 1;
            purchasedIndicators[selectedSkin].gameObject.SetActive(false);
            scaledUp = false;
            windowShopping = false;
            ResetInactiveSkins();
            HideSelectedName();
            purchaseButton.gameObject.SetActive(false);
            confirmPurchase.gameObject.SetActive(false);
        }
        else if (desiredSkin == 2)
        {
            unlockableSkins.flyingFortressUnlocked = true;
            Currency.airMedals -= 700;
            listOfSkins[selectedSkin].transform.localScale /= scaleAmount;
            selectedSkin = 2;
            purchasedIndicators[selectedSkin].gameObject.SetActive(false);
            scaledUp = false;
            windowShopping = false;
            ResetInactiveSkins();
            HideSelectedName();
            purchaseButton.gameObject.SetActive(false);
            confirmPurchase.gameObject.SetActive(false);
        }
        else if (desiredSkin == 3)
        {
            unlockableSkins.stratotankerUnlocked = true;
            Currency.airMedals -= 700;
            listOfSkins[selectedSkin].transform.localScale /= scaleAmount;
            selectedSkin = 3;
            purchasedIndicators[selectedSkin].gameObject.SetActive(false);
            scaledUp = false;
            windowShopping = false;
            ResetInactiveSkins();
            HideSelectedName();
            purchaseButton.gameObject.SetActive(false);
            confirmPurchase.gameObject.SetActive(false);
        }
        else if (desiredSkin == 4)
        {
            unlockableSkins.warthogUnlocked = true;
            Currency.airMedals -= 700;
            listOfSkins[selectedSkin].transform.localScale /= scaleAmount;
            selectedSkin = 4;
            purchasedIndicators[selectedSkin].gameObject.SetActive(false);
            scaledUp = false;
            windowShopping = false;
            ResetInactiveSkins();
            HideSelectedName();
            purchaseButton.gameObject.SetActive(false);
            confirmPurchase.gameObject.SetActive(false);
        }
        else if (desiredSkin == 5)
        {
            unlockableSkins.eagleUnlocked = true;
            Currency.airMedals -= 700;
            listOfSkins[selectedSkin].transform.localScale /= scaleAmount;
            selectedSkin = 5;
            purchasedIndicators[selectedSkin].gameObject.SetActive(false);
            scaledUp = false;
            windowShopping = false;
            ResetInactiveSkins();
            HideSelectedName();
            purchaseButton.gameObject.SetActive(false);
            confirmPurchase.gameObject.SetActive(false);
        }
        else if (desiredSkin == 6)
        {
            unlockableSkins.viperUnlocked = true;
            Currency.airMedals -= 700;
            listOfSkins[selectedSkin].transform.localScale /= scaleAmount;
            selectedSkin = 6;
            purchasedIndicators[selectedSkin].gameObject.SetActive(false);
            scaledUp = false;
            windowShopping = false;
            HideSelectedName();
            ResetInactiveSkins();
            purchaseButton.gameObject.SetActive(false);
            confirmPurchase.gameObject.SetActive(false);
        }
        else if (desiredSkin == 7)
        {
            unlockableSkins.blackbirdUnlocked = true;
            Currency.airMedals -= 700;
            listOfSkins[selectedSkin].transform.localScale /= scaleAmount;
            selectedSkin = 7;
            purchasedIndicators[selectedSkin].gameObject.SetActive(false);
            scaledUp = false;
            windowShopping = false;
            ResetInactiveSkins();
            HideSelectedName();
            purchaseButton.gameObject.SetActive(false);
            confirmPurchase.gameObject.SetActive(false);
        }

        purchaseNoise.Play();
        PlayerPrefs.SetInt("Airmedals", Currency.airMedals);
        availableAirmedals.text = PlayerPrefs.GetInt("Airmedals").ToString();
        //Removes all functionalities from the buttons, to be replaced 
        foreach (Button gameButtons in listOfButtons)
        {
            gameButtons.onClick.RemoveAllListeners();
        }
        //Disables all purchased and non purchased indicators, to be replaced
        foreach (GameObject purchased in purchasedIndicators)
        {
            purchased.gameObject.SetActive(false);
        }
        foreach (GameObject notPurchased in unpurchasedIndicators)
        {
            notPurchased.gameObject.SetActive(false);
        }
        randomizerList.Clear();
        GenerateRandomLogic();
        ChangeButtons();
        UpdateBackup();
        SaveJson();
        
    }

    void DeclinePurchase()
    {
        buttonNoise.Play();
        confirmPurchase.gameObject.SetActive(false);
    }

    void CheckForAdsRemoved()
    {
        if (PlayerPrefs.GetInt("NoAds") == 1)
        {
            removeAdsButton.gameObject.SetActive(false);
        }
        else
        {
            removeAdsButton.gameObject.SetActive(true);
        }
    }

    void GenerateRandomLogic()
    {
        randomizerList = new List<int>();

        //This always adds the default skin to the list
        randomizerList.Add(8);
        
        //This allows the system to check for which skins are unlocked
        if (unlockableSkins.airlinerUnlocked == true)
        {
            randomizerList.Add(0);
        }
        if (unlockableSkins.mustangUnlocked == true)
        {
            randomizerList.Add(1);
        }
        if (unlockableSkins.flyingFortressUnlocked == true)
        {
            randomizerList.Add(2);
        }
        if (unlockableSkins.stratotankerUnlocked == true)
        {
            randomizerList.Add(3);
        }
        if (unlockableSkins.warthogUnlocked == true)
        {
            randomizerList.Add(4);
        }
        if (unlockableSkins.eagleUnlocked == true)
        {
            randomizerList.Add(5);
        }
        if (unlockableSkins.viperUnlocked == true)
        {
            randomizerList.Add(6);
        }
        if (unlockableSkins.blackbirdUnlocked == true)
        {
            randomizerList.Add(7);
        }

    }

    void RandomButton()
    {
        if (randomizerList.Count > 1)
        {
            randomizerList.Remove(selectedSkin);
        }
        listOfSkins[selectedSkin].transform.localScale /= scaleAmount;
        windowShopping = false;
        selectedSkin = randomizerList[Random.Range(0, randomizerList.Count)];
        scaledUp = false;
        GenerateRandomLogic();
        HideActiveIndicator();
        ResetInactiveSkins();
        HideSelectedName();
    }

    private void SaveJson()
    {
        string json = JsonUtility.ToJson(unlockableSkins);
        File.WriteAllText(unlockSkinsPath, json);
    }

    private void UpdateBackup()
    {
        backupData.currencyEarned = PlayerPrefs.GetInt("Airmedals", 0);
        if (PlayerPrefs.GetInt("NoAds", 0) == 1)
        {
            backupData.adsRemoved = true;
        }
        else
        {
            backupData.adsRemoved = false;
        }
        string newJson = JsonUtility.ToJson(backupData);
        File.WriteAllText(backupDataPath, newJson);
    }
}
