using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToMainMenu : MonoBehaviour
{

    public GameObject SplashImg;

    // Start is called before the first frame update
    void Start()
    {
        SplashImg.SetActive(true);
        Invoke("splash", 7f);
    }

    // Update is called once per frame


    public void splash()
    {
        SplashImg.SetActive(false);
        SceneManager.LoadScene(1);
    }
}
