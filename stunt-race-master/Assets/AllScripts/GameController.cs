using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{

    [Header("Score Functionality")]
    private bool checkCollision = true;
    private int playerScore = 0;
    private int coinValue = 10;
    private int obstacleHitValue = 5;
    private int CoinSpriteNumberIndex;
    //public Vec ScorePopuTransform;
    public TextMeshProUGUI textPrefab;
    public RectTransform canvasRect;
    public ParticleSystem CoinparticleSystem;

    [Header("Finish point")]

    //public Rigidbody car;  
    public Camera firstCamera; // Reference to your second camera
    public float rotationSpeed = 5.0f;
    public Vector3 offSet;
    private bool hasReachedFinish = false;
    private bool hasFinish = true;
    public float CamrotationSpeed = 5.0f;
    public GameObject TrophyPreFab;
    
    public List<ParticleSystem> LevelFinishparticleSystem = new List<ParticleSystem>(); // List to hold multiple particle systems
    
    public Camera secondCamera; // Reference to your second camera
    public string objectTag = "YourTagHere"; // Specify the tag of the object to follow
    public float smoothSpeed = 0.125f; // Adjust the smoothness of the camera follow
    public Vector3 offset = new Vector3(0, 2, -5);
    public Vector3 OffSetForCame2 = new Vector3(0, 0, -5 );


    private GameObject targetObject;
    private void Start()
    {

        //targetObject = GameObject.FindGameObjectWithTag("FinishUP");
    }



    private void LateUpdate()
    {
        if(!hasFinish)
        {
            foreach (ParticleSystem p in LevelFinishparticleSystem)
            {
                p.gameObject.SetActive(true);
                p.Play();
            }
        }
        CoinparticleSystem.transform.LookAt(Camera.main.transform);
        // Follow the target object smoothly
        secondCamera.transform.LookAt(transform.position);
        if (targetObject != null)
        {
            Vector3 desiredPosition = targetObject.transform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            secondCamera.transform.position = smoothedPosition;

            secondCamera.transform.LookAt(targetObject.transform);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Vector3 triggerPoint = other.ClosestPointOnBounds(transform.position);

        if (other.CompareTag("Coin"))
        {
            
            playerScore += coinValue;

            //if (!checkCollision)
            //{
            //coinValue += 5; 

            //}
            Vector3 center = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 1.5f, 10f));
            // Set the GameObject's position to the center of the screen
            CoinparticleSystem.transform.position = center;

            CoinparticleSystem.Play();
            GameManager.instance.Levels[GameManager.instance.CurrentLevel].LevelCoin.Remove(other.GetComponent<SpriteRenderer>());

            GameManager.instance.CrrLevelScore += playerScore;
            GameManager.instance.CoinSoundPlay();
            GameManager.instance.ModifyScore(playerScore);

            ///////////////
            Vector3 canvasCenter = canvasRect.position;

            TextMeshProUGUI textObject = Instantiate(textPrefab, canvasCenter, Quaternion.identity, canvasRect);

            textObject.text = "+" + playerScore.ToString();

            // Destroy the instantiated text object after 2 seconds
            Destroy(textObject.gameObject, 2f);

            playerScore = 0;
            Destroy(other.gameObject);

            

        }
        else if (other.CompareTag("obstacletrigger"))
        {

            other.gameObject.SetActive(false);

            coinValue += 5;
            checkCollision = false;
            Debug.Log("CheckCollision is true");

            GameManager gameManager = GameManager.instance;
            CoinSpriteNumberIndex++;
              
            for (int i = 0; i < gameManager.Levels[gameManager.CurrentLevel].LevelCoin.Count; i++)
            {
                gameManager.Levels[gameManager.CurrentLevel].LevelCoin[i].sprite = gameManager.sprites[CoinSpriteNumberIndex];
            }

        }

        if (other.CompareTag("FellDown"))
        {
            Debug.Log("Car Fell Down");

            GameManager.instance.GameOver();
        }

       
        
        if (other.gameObject.CompareTag("Untouch"))
        {
            Debug.Log("State Changed");
                      
        }

        
    }

    private Dictionary<GameObject, bool> collidedObstacles = new Dictionary<GameObject, bool>();

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Obstacle"))
        //{

        //    Debug.Log(collision.transform.childCount);
        //    //collision.transform.GetChild(0).gameObject.SetActive(false);
        //    collision.transform.parent.transform.GetChild(0).gameObject.SetActive(false);
        //    Debug.Log("Cillison done with obstacle");
        //    //checkCollision = true;

        //    GameManager.instance.CrrLevelScore -= obstacleHitValue;

        //    GameManager.instance.score -= obstacleHitValue;

        //    //coinValue = 10;

        //    checkCollision = true;
        //    GameManager.instance.ModifyScore(playerScore);

        //    Vector3 canvasCenter = canvasRect.position;

        //    // Instantiate the text object as a child of the canvas
        //    TextMeshProUGUI textObject = Instantiate(textPrefab, canvasCenter, Quaternion.identity, canvasRect);

        //    // Set text content
        //    textObject.text = "-5";

        //    // Destroy the instantiated text object after 2 seconds
        //    Destroy(textObject.gameObject, 2f);
        //}

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Check if this obstacle has already been collided with
            if (!collidedObstacles.ContainsKey(collision.gameObject) || !collidedObstacles[collision.gameObject])
            {
                // Set this obstacle as collided
                collidedObstacles[collision.gameObject] = true;

                // Your existing code for handling collision with the obstacle
                Debug.Log(collision.transform.childCount);
                //collision.transform.GetChild(0).gameObject.SetActive(false);
                collision.transform.parent.transform.GetChild(0).gameObject.SetActive(false);
                Debug.Log("Collision done with obstacle");

                GameManager.instance.CrrLevelScore -= obstacleHitValue;
                GameManager.instance.score -= obstacleHitValue;

                //coinValue = 10;

                checkCollision = true;
                GameManager.instance.ModifyScore(playerScore);

                Vector3 canvasCenter = canvasRect.position;

                // Instantiate the text object as a child of the canvas
                TextMeshProUGUI textObject = Instantiate(textPrefab, canvasCenter, Quaternion.identity, canvasRect);

                // Set text content
                textObject.text = "-5";

                // Destroy the instantiated text object after 2 seconds
                Destroy(textObject.gameObject, 2f);
            }
        }

        if (collision.gameObject.CompareTag("Finish"))
        {
            Vector3 collisionPoint = collision.contacts[0].point;

            if (hasFinish)
            {
                GameManager.instance.isTimerRunning = false;
                transform.GetChild(0).gameObject.SetActive(true);  

                //RotateSecondCamera.instance.target = transform.position;
                Debug.Log("Reached to finish point");
                StartCoroutine(complete());

                ///////////
                //secondCamera.transform.position = transform.position + offSet;

                hasReachedFinish = true;
                // Activate the second camera

                secondCamera.gameObject.SetActive(true);
                secondCamera.transform.position = transform.position + OffSetForCame2;
                secondCamera.transform.rotation = transform.rotation  ;
                firstCamera.GetComponent<Camera>().enabled = false;
                hasFinish = false;

                GameManager.instance.WinSoundPlay();
            }
            else
            {
                Debug.Log("Vehicle is touching Finish floor again");
            }
           
            

           

            //foreach (ParticleSystem item in LevelFinishparticleSystem)
            //{
            //    Instantiate(item.gameObject, item.transform.position, Quaternion.identity);
            //    item.gameObject.transform.position = transform.position;
            //    item.gameObject.SetActive(true);
            //    item.Play();
            //}

            //RotateSecondCamera rotateScript = secondCamera.GetComponent<RotateSecondCamera>();
            //if (rotateScript != null)
            //{
            //    rotateScript.enabled = true;
            //}


        }

        //if (collision.gameObject.CompareTag("GameEnd"))
        //{
        //    GameManager.instance.completeLevel();
        //    //GameManager.instance.GameEnd();
        //    //GameManager.instance.WinSoundPlay();
        //}
    }

    
    IEnumerator complete()
    {
        yield return new WaitForSeconds(2);
        GameManager.instance.completeLevel();
    }

    public void SoundController()
    {
        //RCC_CarControllerV3.AudioType.Off=0;
    }  
    
}
