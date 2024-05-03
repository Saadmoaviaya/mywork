using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Menu : MonoBehaviour
{
    public GameObject SplashScreen, VehicleSelection, PrivacyPolicyPanel, settingPanel, buttonhidePanel, thumbimg;
    public GameObject ExitPanel;
    public GameObject MainMenu;
    public GameObject[] MusicToggle;
    public AudioSource MainMenuSound, Buttonsound;
    public static Main_Menu instance;

    void Start()
    {
        if (PlayerPrefs.GetInt("PrivacyOff") == 1)
        {
            PrivacyPolicyPanel.SetActive(false);
        }
        if (PlayerPrefs.GetInt("TutorialImg") == 1)
        {
            buttonhidePanel.SetActive(false);
        }
        instance = this;
        SplashScreen.SetActive(true);
        Invoke("splashScreen",2f);

        MusicToggler();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void splashScreen()
    {
        SplashScreen.SetActive(false);
    }
    public void ExitNow()
    {
        Application.Quit();
        ClickSound();
    }
    public void NotOk()
    {
        ExitPanel.SetActive(false);
        ClickSound();
    }
    public void PlayButton()
    {
        VehicleSelection.SetActive(true);
        MainMenu.SetActive(false);
        buttonhidePanel.SetActive(false);
        PlayerPrefs.SetInt("TutorialImg", 1);
        ClickSound();
    }

    public void goToLevel()
    {

    }

    public void ExitButton()
    {
        ExitPanel.SetActive(true);
        ClickSound();
    }
    public void OkButton()
    {
        ClickSound();
        ExitPanel.SetActive(false);
    }
    public void SettingBtn()
    {
        ClickSound();
        settingPanel.SetActive(true);
    }
    public void BackSetting()
    {
        ClickSound();
        settingPanel.SetActive(false);
    }
    public void PrivacyPolicy()
    {

        Application.OpenURL("https://venturegamestudios.blogspot.com/2023/05/privacy-policy-for-venture-game-studios.html");
        ClickSound();
    }
    public void Rateus()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.vgs.indianbikes.vehicledriving.bikegame&hl=en&gl=US");
        ClickSound();
    }
    public void MoreGames()
    {
        Application.OpenURL("https://play.google.com/store/apps/developer?id=Venture+Game+Studios&hl=en&gl=US");
        ClickSound();
    }
    public void AgreeForPRivacy()
    {
        PrivacyPolicyPanel.SetActive(false);
        PlayerPrefs.SetInt("PrivacyOff", 1);
        ClickSound();
    }


    public void MusicToggler()
    {
        if (PlayerPrefs.GetInt("Music") == 0)
        {
            MainMenuSound.Stop();
            Buttonsound.Stop();
            //MusicToggle[0].SetActive(true);
            //MusicToggle[1].SetActive(false); 

            MusicToggle[0].SetActive(false);
            MusicToggle[1].SetActive(true);
        }
        else if (PlayerPrefs.GetInt("Music") == 1)
        {
            MainMenuSound.Play();
            Buttonsound.Play();
            //MusicToggle[0].SetActive(false);
            //MusicToggle[1].SetActive(true);
            MusicToggle[0].SetActive(true);
            MusicToggle[1].SetActive(false);
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
    public void ThumbButton()
    {
        buttonhidePanel.SetActive(false);
        thumbimg.SetActive(false);
    }


}
