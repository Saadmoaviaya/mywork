using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening.Core.Easing;
using UnityEngine.UIElements;
using Toggle = UnityEngine.UI.Toggle;

public class GameManager : MonoBehaviour
{
   
    public static GameManager instance;


    [Header("score")]
    public int score = 0;
    public int LevelFinishScore = 50;
    public bool ishiited = false;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI displayTotalScore;
    public TextMeshProUGUI displayLevelScore;
    public TextMeshProUGUI displayLevelTime;


    [Header("Current Level Stats")]
    public float CrrLevelScore;
    public float CrrLevelSpendTime;

    [Header("Idol Condition")]
    public bool PlayingLevel = false;
    bool isGamePaused = false;
    float inactivityTimer = 0f;
    public float inactivityThreshold = 15f;

    [Header("Sound")]
    public AudioSource BGSound;
    private AudioSource audioSource;
    public AudioClip coinSound;
    public AudioClip winSound;
    public GameObject[] MusicToggle;


    [Header("Panels")]
    public GameObject GameDone;
    public GameObject[] levels;
    public GameObject loadingScreen, levelText, RccCanvas, completePanel, pausePanel, gameOverPanel, PauseButton;
    [Header("Sprite Numbers")]
    public Sprite[] sprites;
    public LevelData[] Levels;
    public int CurrentLevel;

    [Header("Level Timer")]
    private bool CheckTimerPanel = false;
    public GameObject SetTimer;
    public GameObject CarSelectionPanle;
    public int L1Time, L2Time, L3Time, L4Time, L5Time, ReqScoreforLevel3 = 200, ReqScoreforLevel4 = 350;
    public GameObject timer, RequiredScorePopup3, RequiredScorePopup4;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI LevelText;
    float currentTime;
    public bool isTimerRunning = true;

    public InputField timeInputField;
    public Toggle timerToggle;
    //public UnityEngine.UI.Toggle timerToggle;

   


    public bool isToggleOn = false ;

    public Dropdown dropdown;
    private UnityEngine.UI.Toggle timeToggle;

    [Header("Level Summary")]
    public TextMeshProUGUI SmryLevel1Score;
    public TextMeshProUGUI SmryLevel1Time;
    public TextMeshProUGUI SmryLevel2Score;
    public TextMeshProUGUI SmryLevel2Time;
    public TextMeshProUGUI SmryLevel3Score;
    public TextMeshProUGUI SmryLevel3Time;
    public TextMeshProUGUI SmryLevel4Score;
    public TextMeshProUGUI SmryLevel4Time;

    void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Time.timeScale = 1;

        

        levels[PlayerPrefs.GetInt("SelectedLevel")].SetActive(true);

        Debug.Log("Activated Level is " + levels[PlayerPrefs.GetInt("SelectedLevel")]);

        int selectedLevel = PlayerPrefs.GetInt("SelectedLevel");
        if(selectedLevel == 0)
        {
            currentTime = 60f;
        }
        if (selectedLevel == 1)
        {
            currentTime = 80f;
        }
        if (selectedLevel == 2)
        {
            currentTime = 120f;
        }
        if (selectedLevel == 3)
        {
            currentTime = 160f;
        }



        CurrentLevel = selectedLevel;

        for (int i = 0; i < Levels[CurrentLevel].LevelCoin.Count; i++)
        {
            Levels[CurrentLevel].LevelCoin[i].sprite = sprites[0];
        }


        LevelText.text = "Level " +(selectedLevel+1).ToString();

        
       
        if (PlayerPrefs.GetInt("StartScore") == 0)
        {
            ModifyScore(50);
            CrrLevelScore = 50f;
           
            Debug.Log("First time give score");

        }

        ////////
        float crntlvlscore = (PlayerPrefs.GetFloat(string.Concat("Lvl", CurrentLevel, "Score")));
        int overallscore = PlayerPrefs.GetInt("PlayerScore");

        int intValue = Mathf.FloorToInt(crntlvlscore);

        overallscore -= intValue;

        PlayerPrefs.SetInt("PlayerScore", overallscore);

        PlayerPrefs.SetFloat(string.Concat("Lvl", CurrentLevel, "Score"), 0);

        PlayerPrefs.Save();

        ///////

        ModifyScore(PlayerPrefs.GetInt("PlayerScore"));

        PauseButton.SetActive(false);
        completePanel.SetActive(false);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);


        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        

        CheckGameMusic();
        //AdsController.instance.ShowBannerAd();

    }


    private void Update()
    {

        if (PlayingLevel)
        {
            Debug.Log("Checking for idol state is started");

            if (Input.touchCount > 0 || Input.anyKeyDown)
            {
                // Reset the inactivity timer when there's input
                inactivityTimer = 0f;
                if (isGamePaused)
                {

                    isGamePaused = false;

                }
            }
            else
            {
                // If no touch input, start counting inactivity
                inactivityTimer += Time.deltaTime;


                if (inactivityTimer >= inactivityThreshold)
                {
                    isGamePaused = true;

                    pause();
                }
            }
        }
    }

    

    public void CheckGameMusic()
    {
        if (PlayerPrefs.GetInt("Music") != 0)
        {
            AudioListener.volume = 1;

        }
        else
        {
            AudioListener.volume = 0;
        }
    }

   
    public void SetPanels()
    {
        PauseButton.SetActive(true);
        completePanel.SetActive(false);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void StartTime()
    {
        PlayingLevel = true;
        InvokeRepeating(nameof(UpdateTimer), 1f, 1f);
    }

    //public void ChangeState()
    //{
    //    //for toggle state
    //    ishiited = !ishiited;
     
    //    //ishiited = false;
    //    Debug.Log("State Changed =" + ishiited);
    //}


    public void GameEnd()
    {
        PlayingLevel = false;
        GameDone.SetActive(true);
        Time.timeScale = 0;    
    }

   
    

    public void ModifyScore(int amount)
    {

        score += amount;
        
        // Ensure the score doesn't go below zero
        if (score < 0)
        {
            score = 0;
        }

        UpdateScoreUI();
    }
    public void UpdateScoreUI()
    {
        
        ScoreText.text = "Score: " + score.ToString();
    }


    public void UpdateTimer()
    {
        if (isTimerRunning)
        {
            

            CrrLevelSpendTime += 1;

            UpdateTimerDisplay();

        }
    }

    public void UpdateTimerDisplay()
    {
        

        
        int minutes1 = Mathf.FloorToInt(currentTime / 60);
        int seconds1 = Mathf.FloorToInt(currentTime % 60);
        int minutes2 = Mathf.FloorToInt(CrrLevelSpendTime / 60);
        int seconds2 = Mathf.FloorToInt(CrrLevelSpendTime % 60);
        string formattedTimetotal = string.Format("{0:00}:{1:00}", minutes1, seconds1);
        string formattedTimeSpend = string.Format("{0:00}:{1:00}", minutes2, seconds2);

        timerText.text = formattedTimetotal + " / " + formattedTimeSpend;
        if (currentTime <= CrrLevelSpendTime)
        {
            timerText.color = Color.red;

        }



    }

    


    public void GameOver()
    {
        PlayingLevel = false;
        gameOverPanel.SetActive(true);
        isTimerRunning = false;
        PauseButton.SetActive(false);
       
    }

    public void WinSoundPlay()
    {
        if (coinSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(winSound);
        }
    }

    public void CoinSoundPlay()
    {
        if (coinSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(coinSound);
        }
    }

    public void completeLevel()
    {
        PlayerPrefs.SetInt("StartScore", 1);
       
        PlayingLevel = false;
        int next = PlayerPrefs.GetInt("SelectedLevel");
        if(next <= 1)
        {
            next++;
            PlayerPrefs.SetInt("UnlockLevel", next);
        }

        Debug.Log("LevelComplete is called");
        //isTimerRunning = false;
        CrrLevelScore += LevelFinishScore;
        score += LevelFinishScore;
       

        PauseButton.SetActive(false);
        completePanel.SetActive(true);
        levelText.SetActive(false);
        timerText.enabled = false;

        PlayerPrefs.SetInt("PlayerScore", score);
        
        if (next == 3)
        {
            completePanel.SetActive(false);
            GameEnd();
        }




        PlayerPrefs.SetFloat(string.Concat("Lvl",CurrentLevel,"Score"), CrrLevelScore);
        
        Debug.Log("Scores at the end of the level = " + CrrLevelScore.ToString());

        PlayerPrefs.SetFloat(string.Concat("Lvl",CurrentLevel,"Time"), CrrLevelSpendTime);

        float WeightFormula = (2 * (1000 - CrrLevelSpendTime) + 4 * (CrrLevelScore)) / 6;
        Debug.Log("Weight Formula result is = "+ WeightFormula);
        float LastBestPoint = PlayerPrefs.GetFloat(string.Concat("Lvl", CurrentLevel, "BestPoint"), WeightFormula);

        if(WeightFormula > LastBestPoint)
        {
            PlayerPrefs.SetFloat(string.Concat("Lvl", CurrentLevel, "BestPoint"), WeightFormula);
        }


        for (int i = 0; i < levels.Length; i++)
        {
            Debug.Log("Lvl " + i + 1 + " Score " + PlayerPrefs.GetFloat(string.Concat("Lvl", i, "Score")));
            Debug.Log("Lvl " + i + 1 + " Time " + PlayerPrefs.GetFloat(string.Concat("Lvl", i, "Time")));
            Debug.Log("Lvl " + i + 1 + " BestPoint " + PlayerPrefs.GetFloat(string.Concat("Lvl", i, "BestPoint")));
        }

        if (CurrentLevel == levels.Length-1)
        {
            for (int i = 0; i < levels.Length; i++)
            {
                print("Lvl " + i + 1 + " Score " + PlayerPrefs.GetFloat(string.Concat("Lvl", i, "Score")));
                print("Lvl " + i + 1 + " Time " + PlayerPrefs.GetFloat(string.Concat("Lvl", i, "Time")));
                print("Lvl " + i + 1 + " BestPoint " + PlayerPrefs.GetFloat(string.Concat("Lvl", i, "BestPoint")));
            }

        }


        //PlayerPrefs.SetFloat(string.Concat("Lvl", CurrentLevel, "Time"), CrrLevelSpendTime);


     


        displayTotalScore.text = PlayerPrefs.GetInt("PlayerScore").ToString();
        displayLevelScore.text = (PlayerPrefs.GetFloat(string.Concat("Lvl", next - 1, "Score"))).ToString();

        float totalTimeInSeconds = PlayerPrefs.GetFloat(string.Concat("Lvl", CurrentLevel, "Time"));
        int minutes = Mathf.FloorToInt(totalTimeInSeconds / 60); 
        int seconds = Mathf.FloorToInt(totalTimeInSeconds % 60);
        string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        displayLevelTime.text =  formattedTime;

        
        SmryLevel1Score.text =  "Score = " + (PlayerPrefs.GetFloat(string.Concat("Lvl", 0, "Score"))).ToString();

        float totalTimeInSecondsL1 = (PlayerPrefs.GetFloat(string.Concat("Lvl", 0, "Time")));
        int minutesL1 = Mathf.FloorToInt(totalTimeInSecondsL1 / 60);
        int secondsL1 = Mathf.FloorToInt(totalTimeInSecondsL1 % 60);
        string formattedTimeL1 = "Time = " + string.Format("{0:00}:{1:00}", minutesL1, secondsL1);
        SmryLevel1Time.text = formattedTimeL1 + " sec ";
        //SmryLevel1Time.text = "Time = " + formattedTimeL1 + " sec ";

        //SmryLevel1Time.text = "Time = " + (PlayerPrefs.GetFloat(string.Concat("Lvl", 0, "Time"))).ToString() + " sec ";

        SmryLevel2Score.text = "Score = " + (PlayerPrefs.GetFloat(string.Concat("Lvl", 1, "Score"))).ToString();

        float totalTimeInSecondsL2 = (PlayerPrefs.GetFloat(string.Concat("Lvl", 1, "Time")));
        int minutesL2 = Mathf.FloorToInt(totalTimeInSecondsL2 / 60);
        int secondsL2 = Mathf.FloorToInt(totalTimeInSecondsL2 % 60);
        string formattedTimeL2 = string.Format("{0:00}:{1:00}", minutesL2, secondsL2);
        //displayLevelTime.text =  formattedTimeL2;
        SmryLevel2Time.text = "Time = " + formattedTimeL2 + " sec ";
        //SmryLevel2Time.text = "Time = " + (PlayerPrefs.GetFloat(string.Concat("Lvl",  1, "Time"))).ToString() + " sec ";

        SmryLevel3Score.text = "Score = " + (PlayerPrefs.GetFloat(string.Concat("Lvl", 2, "Score"))).ToString();
        float totalTimeInSecondsL3 = (PlayerPrefs.GetFloat(string.Concat("Lvl", 2, "Time")));
        int minutesL3 = Mathf.FloorToInt(totalTimeInSecondsL3 / 60);
        int secondsL3 = Mathf.FloorToInt(totalTimeInSecondsL3 % 60);
        string formattedTimeL3 = string.Format("{0:00}:{1:00}", minutesL3, secondsL3);
        //displayLevelTime.text =  formattedTimeL3;
        SmryLevel3Time.text = "Time = " + formattedTimeL3 + " sec ";
        //SmryLevel3Time.text = "Time = " + (PlayerPrefs.GetFloat(string.Concat("Lvl", 2, "Time"))).ToString() + " sec ";

        SmryLevel4Score.text = "Score = " + (PlayerPrefs.GetFloat(string.Concat("Lvl", 3, "Score"))).ToString();
        float totalTimeInSecondsL4 = (PlayerPrefs.GetFloat(string.Concat("Lvl", 3, "Time")));
        int minutesL4 = Mathf.FloorToInt(totalTimeInSecondsL4 / 60);
        int secondsL4 = Mathf.FloorToInt(totalTimeInSecondsL4 % 60);
        string formattedTimeL4 = string.Format("{0:00}:{1:00}", minutesL4, secondsL4);
        //displayLevelTime.text =  formattedTimeL4;

        SmryLevel4Time.text = "Time = " + formattedTimeL4 + " sec ";
        //SmryLevel4Time.text = "Time = " + (PlayerPrefs.GetFloat(string.Concat("Lvl", CurrentLevel, "Time"))).ToString() + " sec ";


        if (CrrLevelSpendTime > currentTime)
        {

            float LCTP = ((CrrLevelSpendTime - currentTime / currentTime) * 100);

            if (LCTP > 0 && LCTP <= 20)
            {
                score -= 2;
                CrrLevelScore -= 2;
            }
            else if (LCTP > 20 && LCTP <= 40)
            {
                score -= 4;
                CrrLevelScore -= 4;
            }
            else if (LCTP > 40 && LCTP <= 60)
            {
                score -= 6;
                CrrLevelScore -= 6;
            }
            else if (LCTP > 60 && LCTP <= 80)
            {
                score -= 8;
                CrrLevelScore -= 8;
            }
            else if (LCTP > 80)
            {
                score -= 10;
                CrrLevelScore -= 10;
            }
        }

        PlayerPrefs.Save();

        //AdsController.instance.ShowInterstitialAd();
        //AdsController.instance.ShowMediumRectangleAd();
        //AdsController.instance.HideBannerAd();

        if (next == 3)
        {
            //AdsController.instance.ShowBannerAd();
            //AdsController.instance.HideMediumRectangleAd();
        }
    }


    public void pause()
    {
        
        PlayingLevel = false;
        PauseButton.SetActive(false);
        pausePanel.SetActive(true);


        //AdsController.instance.ShowInterstitialAd();
        AdsController.instance.ShowMediumRectangleAd();
        //AdsController.instance.HideBannerAd();


        Time.timeScale = 0;
    }

    public void resume()
    {
        //AdsController.instance.ShowBannerAd();
        //AdsController.instance.HideMediumRectangleAd();
        inactivityTimer = 0;
        PlayingLevel = true;
        PauseButton.SetActive(true);
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }


    public void restart()
    {
        
        Time.timeScale = 1;

        float crntlvlscore = (PlayerPrefs.GetFloat(string.Concat("Lvl", CurrentLevel, "Score")));
        int overallscore = PlayerPrefs.GetInt("PlayerScore");

        int intValue = Mathf.FloorToInt(crntlvlscore);

        overallscore -= intValue;

        PlayerPrefs.SetInt("PlayerScore", overallscore);

        PlayerPrefs.SetFloat(string.Concat("Lvl", CurrentLevel, "Score"), 0);

        PlayerPrefs.Save();


        //AdsController.instance.HideMediumRectangleAd();

                StartCoroutine(Load());
        Debug.Log("Current level is "+ CurrentLevel );

    }

    IEnumerator RestardGame() {
        yield return new WaitForSecondsRealtime(3f);
       
        SceneManager.LoadScene("GamePlay 1");

    }

    public void home()
    {
        //AdsController.instance.ShowInterstitialAd();
        //AdsController.instance.HideMediumRectangleAd();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void RateUs()
    {
        Application.OpenURL("wwww.google.com");
    }
    public void MoreGames()
    {
        Application.OpenURL("wwww.facebook.com");
    }

    

    public void nextlevel()
    {
        int next = PlayerPrefs.GetInt("SelectedLevel");

        Debug.Log("Currently playing Level is " + next.ToString()); 
        
        if (next == 0)
        {
            levels[next].SetActive(false);
            next++; 
            levels[next].SetActive(true);
            PlayerPrefs.SetInt("SelectedLevel", next);
            //PlayerPrefs.SetInt("UnlockLevel", next);
            Debug.Log("Current level: " + PlayerPrefs.GetInt("SelectedLevel"));
            SetPanels();
            StartCoroutine(Load());
        }

        else if(next == 1) 
        {
            int playerScore = PlayerPrefs.GetInt("PlayerScore");

            if (playerScore >= ReqScoreforLevel3) 
            {
                levels[next].SetActive(false);
                next++; 
                levels[next].SetActive(true);
                PlayerPrefs.SetInt("SelectedLevel", next);
                //PlayerPrefs.SetInt("UnlockLevel", next);
                Debug.Log("Unlocked level 3. Current level: " + PlayerPrefs.GetInt("SelectedLevel"));
                SetPanels();
                StartCoroutine(Load());
            }
            else
            {
                Debug.Log("Insufficient score to unlock level 3. Current level: " + PlayerPrefs.GetInt("SelectedLevel"));
                // Handle displaying a message or preventing the unlocking of level 3
                // For example, show a message that the player needs a score of 300 to unlock this level
                RequiredScorePopup3.SetActive(true); 
            }
        }
        else if (next == 2)
        {
            int playerScore = PlayerPrefs.GetInt("PlayerScore");

            if (playerScore >= ReqScoreforLevel4) // Check if the player has a score of at least 300
            {
                levels[next].SetActive(false);
                next++; 
                levels[next].SetActive(true);
                PlayerPrefs.SetInt("SelectedLevel", next);
                PlayerPrefs.SetInt("UnlockLevel", next);
                Debug.Log("Unlocked level 4. Current level: " + PlayerPrefs.GetInt("SelectedLevel"));
                SetPanels();
                StartCoroutine(Load());
            }
            else
            {
                Debug.Log("Insufficient score to unlock level 4. Current level: " + PlayerPrefs.GetInt("SelectedLevel"));
                // Handle displaying a message or preventing the unlocking of level 3
                // For example, show a message that the player needs a score of 300 to unlock this level
                RequiredScorePopup4.SetActive(true);
            }
        }
        
    }
    public void closeRequiredscorePopup()
    {
        RequiredScorePopup3.SetActive(false);
        RequiredScorePopup4.SetActive(false);
    }

    private IEnumerator Load()
    {
        loadingScreen.SetActive(true);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("GamePlay 1");

        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            //loadingProgressBar.value = progress;

            yield return null;
        }

        loadingScreen.SetActive(false);
    }
}

[System.Serializable]
public class LevelData
{
    public string Name;
    public GameObject LevelObject;
    public List<SpriteRenderer> LevelCoin = new List<SpriteRenderer>();
    //public int LevelReward;
    //public int LevelTime;
}