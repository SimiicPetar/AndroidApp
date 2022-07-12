using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDisablerAvatarCreation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (AvatarAssetManager.Instance.CheckIfValuesAreChosen())
            GetComponent<Button>().interactable = true;
        else
            GetComponent<Button>().interactable = false;
    }
}
