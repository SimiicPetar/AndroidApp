using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HairstyleObject : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject HairstyleReference;

    public string hairstyleSlotName;

    AvatarAssetManager avatarManager;

    private void Start()
    {
        avatarManager = AvatarAssetManager.Instance;
    }

    public void ActivateThisHairstyle()
    {
        avatarManager.ActivateClickedHair(HairstyleReference, hairstyleSlotName);
    }
}
