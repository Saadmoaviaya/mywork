using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class LevelSelection : MonoBehaviour
{
    public GameObject LoadingPanel;
    public GameObject LevelSelectionPanel;
    public GameObject[] Levels;
    public Slider LoadingSlider;
    public TextMeshProUGUI LoadingText;
    void Start()
    {

    }   
    void Update()
    {
        //  ...........Unlocking System for Mode 1 .............
        foreach (GameObject x in Levels)
        {
            x.GetComponent<Button>().interactable = false;
            x.transform.GetChild(0).transform.gameObject.SetActive(true);
        }
        //int LevelUnlocked = PlayerPrefs.GetInt("UnlockLevel" + PlayerPrefs.GetInt("ModeNo"), 0);

        int LevelUnlocked = PlayerPrefs.GetInt("UnlockLevel", 0);
        for (int i = 0; i <= LevelUnlocked; i++)
        {
            Levels[i].GetComponent<Button>().interactable = true;
            Levels[i].transform.GetChild(0).transform.gameObject.SetActive(false);
            Levels[i].transform.GetChild(1).transform.gameObject.SetActive(true);
        }
       
       
    }

    public void SelectButton(int Num)
    {
        PlayerPrefs.SetInt("SelectedLevel", Num);
        Debug.Log("Level no is :" + PlayerPrefs.GetInt("SelectedLevel"));
        LoadingPanel.SetActive(true);
        StartCoroutine(LoadSceneAsync("GamePlay 1"));
    }


    IEnumerator LoadSceneAsync(string levelName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(levelName);
        
       
        AdsController.instance.ShowInterstitialAd();
        
        while (!op.isDone)
        {
            LoadingPanel.SetActive(true);
            float progress = Mathf.Clamp01(op.progress / .9f);
            LoadingSlider.value = progress;  // Update the LoadingSlider value directly with progress
            LoadingText.text = "Loading: " + (progress * 100f).ToString("F0") + "%"; // Show loading progress as text
            yield return null;
        }
        LoadingPanel.SetActive(false);
    }
    
}

