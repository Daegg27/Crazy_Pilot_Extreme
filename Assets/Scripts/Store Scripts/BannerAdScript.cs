using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class BannerAdScript : MonoBehaviour
{
    
    
    public BannerPosition bannerPosition = BannerPosition.TOP_CENTER;
    [SerializeField] string _androidAdUnitId = "Banner_Android";
    [SerializeField] string _iOSAdUnitId = "Banner_iOS";
    string _adUnitId = null; // This will remain null for unsupported platforms.

   
    private void Awake()
    {
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif
        Advertisement.Banner.SetPosition(bannerPosition);
        
    }
    void Start()
    {
        if (PlayerPrefs.GetInt("NoAds", 0) == 0)
        {
            LoadBanner();
        }
    }

    // Implement a method to call when the Load Banner button is clicked:
    public void LoadBanner()
    {
        
        // Set up options to notify the SDK of load events:
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        // Load the Ad Unit with banner content:
        Advertisement.Banner.Load(_adUnitId, options);


    }

    // Implement code to execute when the loadCallback event triggers:
    void OnBannerLoaded()
    {
        Debug.Log("Banner loaded");
        ShowBannerAd();



    }

    // Implement code to execute when the load errorCallback event triggers:
    void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
        // Optionally execute additional code, such as attempting to load another ad.
    }

    // Implement a method to call when the Show Banner button is clicked:
    public void ShowBannerAd()
    {
        // Set up options to notify the SDK of show events:
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };

        // Show the loaded Banner Ad Unit:v
        Advertisement.Banner.Show(_adUnitId, options);

    }

    // Implement a method to call when the Hide Banner button is clicked:
    public void HideBannerAd()
    {
        // Hide the banner:
        Advertisement.Banner.Hide();


    }

    private void OnDestroy()
    {
        Advertisement.Banner.Hide(true);
    }

    

    void OnBannerClicked() { }
    void OnBannerShown() { }
    void OnBannerHidden() { }


}
