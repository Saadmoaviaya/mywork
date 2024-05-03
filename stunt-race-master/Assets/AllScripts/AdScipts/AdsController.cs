using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class AdsController : MonoBehaviour
{
    public static AdsController instance;
    
    [Header("True for test Ads")]
    [SerializeField]
    public bool UseTestAds = true;

    [SerializeField]
    public String testBannerAdId = "ca-app-pub-3940256099942544/6300978111";
    [SerializeField]
    public String testInterstitialAdId = "ca-app-pub-3940256099942544/1033173712";

    public String productionBannerAdId;
    public String productionInterstitialAdId;

    private String bannerAdId;
    private String interstitialAdId;

    BannerView bannerAdView;
    BannerView mediumRectangleAdView;
    InterstitialAd interstitialAd;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        try
        {
            setUpAdIds();

            MobileAds.RaiseAdEventsOnUnityMainThread = true;

            MobileAds.Initialize((initStatus) =>
            {
                LoadBannerAd();
                LoadMediumRectangleAd();
                LoadInterstitialAd();
            });
        }
        catch(Exception e)
        {
        }
    }

    private void setUpAdIds()
    {
        if (UseTestAds)
        {
            bannerAdId = testBannerAdId;
            interstitialAdId = testInterstitialAdId;
        }
        else
        {
            bannerAdId = productionBannerAdId;
            interstitialAdId = productionInterstitialAdId;
        }

        Debug.Log("Setup Banner Ad Id = " + bannerAdId + " , Interstitial Ad Id = " + interstitialAdId);
    }

    private void CreateBannerAd()
    {
        if (bannerAdView != null)
        {
            DestroyBannerAd();
        }

        bannerAdView = new BannerView(bannerAdId, AdSize.SmartBanner, AdPosition.Top);
    }

    private void LoadBannerAd()
    {
        if (bannerAdView == null)
        {
            CreateBannerAd();
        }

        bannerAdView.LoadAd(new AdRequest());
    }

    private void CreateMediumRectangleAd()
    {
        if (mediumRectangleAdView != null)
        {
            DestroyMediumRectangleAd();
        }

        Vector2 screenMidpoint = new Vector2(Screen.width / 6f, Screen.height / 3f);

        //mediumRectangleAdView = new BannerView(bannerAdId, AdSize.MediumRectangle, ((int)screenMidpoint.x) - 350, ((int)screenMidpoint.y) - 280);
        mediumRectangleAdView = new BannerView(bannerAdId, AdSize.MediumRectangle, AdPosition.TopRight);
    }

    private void LoadMediumRectangleAd()
    {
        if (mediumRectangleAdView == null)
        {
            CreateMediumRectangleAd();
        }

        mediumRectangleAdView.LoadAd(new AdRequest());
    }

    private void DestroyBannerAd()
    {
        if (bannerAdView != null)
        {
            bannerAdView.Destroy();
            bannerAdView = null;
        }
    }

    private void DestroyMediumRectangleAd()
    {
        if (mediumRectangleAdView != null)
        {
            mediumRectangleAdView.Destroy();
            mediumRectangleAdView = null;
        }
    }

    public void ShowBannerAd()
    {
        if (bannerAdView != null)
        {
            bannerAdView.Show();
        }
    }

    public void HideBannerAd()
    {
        if (bannerAdView != null)
        {
            bannerAdView.Hide();
        }
    }

    public void ShowMediumRectangleAd()
    {
        if (mediumRectangleAdView != null)
        {
            mediumRectangleAdView.Show();
        }
    }

    public void HideMediumRectangleAd()
    {
        if (mediumRectangleAdView != null)
        {
            mediumRectangleAdView.Hide();
        }
    }

    public void LoadInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        var adRequest = new AdRequest();

        InterstitialAd.Load(interstitialAdId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                return;
            }

            interstitialAd = ad;
            RegisterEventHandlers(ad);
        });
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            LoadInterstitialAd();
        }
    }

    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            
        };
        
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            
        };
        
        interstitialAd.OnAdClicked += () =>
        {
            
        };
        
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            
        };
        
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            LoadInterstitialAd();
        };
        
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            LoadInterstitialAd();
        };

    }

}