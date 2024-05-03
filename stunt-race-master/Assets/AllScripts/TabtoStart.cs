using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabtoStart : MonoBehaviour
{
    public static TabtoStart instance;
    public GameObject panelToDeactivate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 0 represents the left mouse button or touch
        {
          
            {
                panelToDeactivate.SetActive(false); // Deactivate the panel

               

                GameManager.instance.SetPanels();
                GameManager.instance.StartTime();
            }
        }

    }
}
