
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform mainCamera;

    void Start()
    {
       
        // Find the main camera in the scene
        mainCamera = Camera.main.transform;
    }

    void Update()
    {
        GetComponent<SpriteRenderer>().flipX = true;
        // Calculate the direction from the sprite to the camera
        Vector3 lookDir = mainCamera.position - transform.position;

        // Ensure the sprite faces the camera without flipping upside down
        transform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
    }
}
