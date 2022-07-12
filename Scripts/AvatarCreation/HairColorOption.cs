using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HairColorOption : MonoBehaviour
{


    AvatarAssetManager avatarManager;

    private void Start()
    {
        avatarManager = AvatarAssetManager.Instance;
    }

    public void SetHairColor()
    {
        var tex = GetComponent<Image>().sprite.texture;
        Color colorOption = tex.GetPixel((int)tex.width / 2, (int)tex.height / 2); 
        avatarManager.ChangeHairColor(colorOption);
    }
}
