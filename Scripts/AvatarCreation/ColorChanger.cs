using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteRenderer target;
    public Image sliderBgImage;
    Slider mainSlider;
    Color chosenColor;
    public Image HandleCircle;

    public Color ChosenColor { get => chosenColor; set => chosenColor = value; }

    public static bool SkinColorChosen = false;

    private void Start()
    {
        mainSlider = GetComponent<Slider>();
        mainSlider.onValueChanged.AddListener(delegate { ValueChangedCheck(); });
    }

    private void ValueChangedCheck()
    {
        var slidersprite = sliderBgImage.sprite;
        var texture = slidersprite.texture;
        float modvalue;

        if (!SkinColorChosen)
            SkinColorChosen = true;

        modvalue = mainSlider.value;
        Color currentcolor = slidersprite.texture.GetPixel((int)(modvalue * texture.width), texture.height / 2);
        HandleCircle.color = currentcolor;
        currentcolor.a = 1;
        target.color = currentcolor;
        chosenColor = currentcolor;
        HandleCircle.color = currentcolor;
        AvatarSpriteAttacher.Instance.ChangeAvatarSkinColor(currentcolor);
        //ovde dodati za promenu boje koze i na attachmentima za kozu koji su vidljivi
    }
}
