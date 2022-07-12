
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGameButtonLogic : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("MapScene");
        }
    }

    public void GoBackToMap()
    {
        SceneManager.LoadScene("MapScene");
    }
}
