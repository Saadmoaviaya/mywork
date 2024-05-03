using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSimple : MonoBehaviour
{
    public static MainMenuSimple instance;

    public GameObject level, SplashScreen, PrivacyPolicy;
    public GameObject setting;
    public GameObject Rule;
    public GameObject[] MusicToggle;
    public GameObject[] SoundToggle;
    public AudioSource MainMenuSound, Buttonsound;
    //public AudioSource  Buttonsound;
  
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Music", 1);
        instance = this;
        MusicToggler();
        SoundToggler();

        if (PlayerPrefs.GetInt("PrivacyPolicy") == 0)
        {
            PrivacyPolicy.SetActive(true);

            PlayerPrefs.SetInt("PrivacyPolicy", 1);

        }

        //AdsController.instance.ShowBannerAd();
        AdsController.instance.HideMediumRectangleAd();

    }

    // Update is called once per frame



    public void GoLevels()
    {
        ////AdsController.instance.ShowSmartBanner();
        ////AdsController.instance.ShowMediumRectangleBanner();
        //gameObject.SetActive(false);
        level.SetActive(true);
        ClickSound();
    }
    public void GoSetting()
    {
        ////AdsController.instance.ShowSmartBanner();
        ////AdsController.instance.ShowSmallBanner();
        ////AdsController.instance.HideMEDIUM_RECTANGLEBanner();
        //gameObject.SetActive(false);
        setting.SetActive(true);
        ClickSound();
    }

    public void GoRule()
    {
        ////AdsController.instance.ShowSmartBanner();
        Rule.SetActive(true);
        ClickSound();

    }
    public void CloseRule()
    {
        Rule.SetActive(false);
        ClickSound();
    }

    public void ExitNow()
    {
        Application.Quit();
        ClickSound();
    }
    public void GoRateUs()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.fnf.avd" + Application.identifier);
        ClickSound();
    }

    public void Back()
    {
        ////AdsController.instance.ShowSmartBanner();
        ////AdsController.instance.HideMediumRectangleBanner();
        gameObject.SetActive(true);
        level.SetActive(false);
        setting.SetActive(false);
        PrivacyPolicy.SetActive(false);
        Rule.SetActive(false);
        ClickSound();
    }

    //public void MusicToggler()
    //{
    //    if (PlayerPrefs.GetInt("Music") == 0)
    //    {
    //        MainMenuSound.Stop();
    //        //Buttonsound.Stop();

    //    }
    //    else if (PlayerPrefs.GetInt("Music") == 1)
    //    {
    //        MainMenuSound.Play();
    //        //Buttonsound.Play();

    //    }
    //}
    //public void clickMusicOn()
    //{
    //    PlayerPrefs.SetInt("Music", 1);

    //    MusicToggler();
    //}
    //public void clickMusicOff()
    //{
    //    PlayerPrefs.SetInt("Music", 0);

    //    MusicToggler();
    //}
    //public void ClickSound()
    //{
    //    if (PlayerPrefs.GetInt("Music") == 1)
    //    {
    //        //Buttonsound.Play();
    //    }
    //    else if (PlayerPrefs.GetInt("Music") == 0)
    //    {
    //        //Buttonsound.Stop();
    //    }
    //}

    public void MusicToggler()
    {
       
         if (PlayerPrefs.GetInt("Music") == 1)
        {
            MainMenuSound.Play();
            Buttonsound.Play();
            MusicToggle[0].SetActive(true);
            MusicToggle[1].SetActive(false);
        }
        else if(PlayerPrefs.GetInt("Music") == 0)
        {
            MainMenuSound.Stop();
            Buttonsound.Stop();
            MusicToggle[0].SetActive(false);
            MusicToggle[1].SetActive(true);
        }
    }
    public void clickMusicBtn()
    {

        if (PlayerPrefs.GetInt("Music") == 0)
        {
            PlayerPrefs.SetInt("Music", 1);
            Debug.Log("im in zero music");
        }
        else if (PlayerPrefs.GetInt("Music") == 1)
        {
            PlayerPrefs.SetInt("Music", 0);

            Debug.Log("im in One  music Playe ");
        }
        MusicToggler();
    }

    public void clickSoundBtn()
    {

        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            PlayerPrefs.SetInt("Sound", 1);
            Debug.Log("im in zero Sound");
        }
        else if (PlayerPrefs.GetInt("Sound") == 1)
        {
            PlayerPrefs.SetInt("Sound", 0);

            Debug.Log("im in One  music Sound ");
        }
        SoundToggler();
    }

    public void SoundToggler()
    {

        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            MainMenuSound.Play();
            //Buttonsound.Play();
            SoundToggle[0].SetActive(true);
            SoundToggle[1].SetActive(false);
        }
        else if (PlayerPrefs.GetInt("Sound") == 0)
        {
            MainMenuSound.Stop();
            //Buttonsound.Stop();
            SoundToggle[0].SetActive(false);
            SoundToggle[1].SetActive(true);
        }
    }

    public void ClickSound()
    {
        if (PlayerPrefs.GetInt("Music") == 1)
        {
            Buttonsound.Play();
        }
        else if (PlayerPrefs.GetInt("Music") == 0)
        {
            Buttonsound.Stop();
        }
    }

}
