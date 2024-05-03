//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using GoogleMobileAds;
//using GoogleMobileAds.Api;
//public class BannerAdManager : MonoBehaviour
//{
//    public static BannerAdManager instance;

//    void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this;
//            //DontDestroyOnLoad(gameObject);
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }
//    // Start is called before the first frame update
//    public void Start()
//    {
//        // Initialize the Google Mobile Ads SDK.
//        MobileAds.Initialize((InitializationStatus initStatus) =>
//        {
//            // This callback is called once the MobileAds SDK is initialized.
//        });
//        CreateBannerView();


//    }


//    // These ad units are configured to always serve test ads.
//#if UNITY_ANDROID
//    private string _adUnitId = "ca-app-pub-2013299676009153/4701625356";
//#elif UNITY_IPHONE
//  private string _adUnitId = "ca-app-pub-3940256099942544/2934735716";
//#else
//  private string _adUnitId = "unused";
//#endif

//    BannerView _bannerView;

//    /// <summary>
//    /// Creates a 320x50 banner view at top of the screen.
//    /// </summary>
//    public void CreateBannerView()
//    {
//        Debug.Log("Creating banner view");

//        // If we already have a banner, destroy the old one.


//        // Create a 320x50 banner at top of the screen
//        _bannerView = new BannerView(_adUnitId, AdSize.Banner, AdPosition.Top);

//        LoadAd();
//    }


//    /// <summary>
//    /// Creates the banner view and loads a banner ad.
//    /// </summary>
//    public void LoadAd()
//    {
//        // create an instance of a banner view first.
//        if (_bannerView == null)
//        {
//            CreateBannerView();
//        }

//        // create our request used to load the ad.
//        var adRequest = new AdRequest();

//        // send the request to load the ad.
//        Debug.Log("Loading banner ad.");
//        _bannerView.LoadAd(adRequest);
//    }
//}

