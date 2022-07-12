using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeOfFontTagButton : MonoBehaviour
{
    SpriteRenderer tagImage;

    private void Awake()
    {
        tagImage = GetComponent<SpriteRenderer>();
    }
    public void SetSprite(Sprite tagSprite)
    {
        tagImage.sprite = tagSprite;
    }
}
