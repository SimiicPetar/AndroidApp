using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitObject : MonoBehaviour
{
    public delegate void OutfitChanged();
    public static OutfitChanged OnOutfitChanged;

    public GameObject OutfitReference;

    AvatarAssetManager avatarManager;

    public AvatarSuitInfo suitObject;

    private void Start()
    {
        avatarManager = AvatarAssetManager.Instance;
    }

    public void ActivateOutfit()
    {
        avatarManager.ActivateClickedOutfit(OutfitReference, suitObject);
        AvatarSpriteAttacher.Instance.AdjustColorsOnAvatar();
    }
}
