using UnityEngine;

// Lock the cameras horizontal field of view so it will frame the same view in the horizontal regardless of aspect ratio.

[RequireComponent(typeof(Camera))]
public class HorizontalFOVLock : MonoBehaviour
{

    public int targetWidth = 640;
    public float pixelsToUnits = 100;

    void Awake()
    {
       // GetComponent<Camera>().fieldOfView = 2 * Mathf.Atan(Mathf.Tan(fixedHorizontalFOV * Mathf.Deg2Rad * 0.5f) / GetComponent<Camera>().aspect) * Mathf.Rad2Deg;
    }

    void Start()
    {
     /*   float aspect = (float)Screen.width / (float)Screen.height;

    if (aspect< 1.5f)
        Camera.main.orthographicSize = 3.6f;
    else
        Camera.main.orthographicSize = 3.2f;

    float vertRatio = Screen.height / 320.0f;
       */ 
    }

    void Update()
    {
        int height = Mathf.RoundToInt(targetWidth / (float)Screen.width * Screen.height);

        Camera.main.orthographicSize = height / pixelsToUnits / 2;
    }
}