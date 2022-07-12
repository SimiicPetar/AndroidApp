using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayInfoDot : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite ActiveSprite;
    public Sprite DisabledSprite;
    
    public void EnableDot()
    {
        GetComponent<Image>().sprite = ActiveSprite;
    }

    public void DisableDot()
    {
        GetComponent<Image>().sprite = DisabledSprite;
    }
}
