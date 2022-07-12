using Patterns.Singleton;
using Spine;
using Spine.Unity;
using Spine.Unity.AttachmentTools;
using Spine.Unity.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct AttachmentColorPair
{
    public string attachmentName;
    public Color attachmentColor;

    public AttachmentColorPair(string name, Color color)
    {
        attachmentName = name;
        attachmentColor = color;
    }
}
public class AvatarSpriteAttacher : Singleton<AvatarSpriteAttacher>
{
    // Start is called before the first frame update

    public Dictionary<string, AttachmentColorPair> AvatarDictionary = new Dictionary<string, AttachmentColorPair>();

    SkeletonRenderer skeletonRenderer;

    public AvatarSuitInfo testSuit;

    public AvatarSuitInfo selectedSuit;


    Color currentSkinColor = new Color(217 / 256f, 217 / 256f, 217 / 256f, 1f);

    [Header("Clothes slots")]
    [SpineSlot]
    public string HatSlotName;
    [SpineSlot]
    public string SuitSlotName;
    [SpineSlot]
    public string LArmSlotName;
    [SpineSlot]
    public string RArmSlotName;
    [SpineSlot]
    public string LForearmSlotName;
    [SpineSlot]
    public string RForearmSlotName;
    [SpineSlot]
    public string LThighSlotName;
    [SpineSlot]
    public string RThighSlotName;
    [SpineSlot]
    public string LLegSlotName;
    [SpineSlot]
    public string RLegSlotName;

    [Header("HairSlot")]
    [SpineSlot]
    public string HairSlotName;
    [Header("Body parts for skin color slots")]
    [SpineSlot]
    List<string> BodyPartSlots;


    string defaultLeftDownArmPart = "Avatar - L forearm";
    string defaultRightDownArmPart = "Avatar - R forearm";
    string defaultLeftDownLegPart = "Avatar - L leg";
    string defaultRightDownLegPart = "Avatar - R leg";

    List<string> DefaultBodyPartNames = new List<string> { "Avatar - L forearm", "Avatar - R forearm", "Avatar - L leg", "Avatar - R leg" };
    List<string> BodyPartsForColorChange = new List<string>{"Avatar - R leg", "Avatar - L leg", "Avatar - neck", "Avatar - R ear", "Avatar - L ear", "Avatar - nose"
    , "Avatar - R cheek", "Avatar - L cheek", "Avatar - R forearm", "Avatar - L forearm", "Avatar - head", "Avatar - L hand", "Avatar - R hand"};


    /*
    * takodje treba i navesti default nazive za te slotove pa ako nije default naziv na tom slotu onda ni da ga ne menjam
    delovi kojima se menja boja koze : 
    Avatar - R leg -> moze doci nesto osim default
    Avatar - L leg -> moze doci nesto osim default
    Avatar - neck
    Avatar - R ear
    Avatar - L ear 
    Avatar - nose
    Avatar - R cheek
    Avatar - L cheek
    Avatar - R forearm -> moze doci nesto osim default
    Avatar - L forearm -> moze doci nesto osim default
    Avatar - head
    * 
    * */

    Skeleton skeleton;

    private void Awake()
    {
        skeleton = gameObject.GetComponent<SkeletonRenderer>().skeleton;
    }

    void Start()
    {
        skeleton = gameObject.GetComponent<SkeletonRenderer>().skeleton;
        LoadAvatar();
    }


   
    public void SetSuit(AvatarSuitInfo suit)
    {

        selectedSuit = suit;



        ResetAttachments();

        if(suit.HatSlotName != "")
            skeleton.SetAttachment(HatSlotName, suit.HatSlotName);
        if(suit.SuitSlotName != "")
            skeleton.SetAttachment(SuitSlotName, suit.SuitSlotName);
        if(suit.LArmSlotName != "")
            skeleton.SetAttachment(LArmSlotName, suit.LArmSlotName);
        if(suit.RArmSlotName != "")
            skeleton.SetAttachment(RArmSlotName, suit.RArmSlotName);
        if (suit.LForearmSlotName != "")
            skeleton.SetAttachment(LForearmSlotName, suit.LForearmSlotName);
        if(suit.RForearmSlotName != "")
            skeleton.SetAttachment(RForearmSlotName, suit.RForearmSlotName);
        if(suit.LThighSlotName != "")
            skeleton.SetAttachment(LThighSlotName, suit.LThighSlotName);
        if(suit.RThighSlotName != "")
            skeleton.SetAttachment(RThighSlotName, suit.RThighSlotName);
        if(suit.LLegSlotName != "")
            skeleton.SetAttachment(LLegSlotName, suit.LLegSlotName);
        if(suit.RLegSlotName != "")
            skeleton.SetAttachment(RLegSlotName, suit.RLegSlotName);

       // SaveAvatar();
    }

    void LoadAvatar()
    {
        ChangeAvatarSkinColor(currentSkinColor); //default skin color
    }

    void SlotSetup(string slotName, AttachmentColorPair pair)
    {
        //item.Attachment == null ? "" : item.Attachment.ToString()
        skeleton.SetAttachment(slotName, pair.attachmentName);
        skeleton.FindSlot(slotName).SetColor(pair.attachmentColor);
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
       // SaveAvatar();
    }

    void ResetAttachments()
    {
        /*foreach (var slotName in BodyPartsForColorChange)
            skeleton.FindSlot(slotName).SetColor(Color.white);*/
        
        skeleton.SetAttachment(HatSlotName, null);
        skeleton.SetAttachment(SuitSlotName, null);
        skeleton.SetAttachment(LArmSlotName, null);
        skeleton.SetAttachment(RArmSlotName, null);
        skeleton.SetAttachment(LForearmSlotName, defaultLeftDownArmPart);
        skeleton.SetAttachment(RForearmSlotName, defaultRightDownArmPart);
        skeleton.SetAttachment(LThighSlotName, null);
        skeleton.SetAttachment(RThighSlotName, null);
        skeleton.SetAttachment(LLegSlotName, defaultLeftDownLegPart);
        skeleton.SetAttachment(RLegSlotName, defaultRightDownLegPart);
    }

    public Dictionary<string, AttachmentColorPair> SaveAvatarDictionary()
    {
        
        foreach (var item in skeleton.Slots.Items)
        {
    
            AvatarDictionary.Add(item.ToString(),
                new AttachmentColorPair(item.Attachment == null ? "" : item.Attachment.ToString(), item.GetColor()));
        }

        return AvatarDictionary;
    }

    public Dictionary<string, AttachmentColorPair> GetAvatarDictionary()
    {
        if(AvatarDictionary.Count != 0)
            return AvatarDictionary;
        else
        {
            return SaveAvatarDictionary();
        }
    }

    public void SaveAvatar()
    {
        Dictionary<string, string> slotAttachmentDictionary = new Dictionary<string, string>();
        Dictionary<string, AttachmentColorPair> slotAttachmentDictionary2 = new Dictionary<string, AttachmentColorPair>();
        //skin.GetAttachments (int slotIndex, List<SkinEntry> attachments)
        foreach (var item in skeleton.Slots.Items) {
            Debug.Log($"{item.ToString()} : {item.Attachment}");
            slotAttachmentDictionary.Add(item.ToString(), item.Attachment == null ? "" : item.Attachment.ToString());
            slotAttachmentDictionary2.Add(item.ToString(),
                new AttachmentColorPair(item.Attachment == null ? "" : item.Attachment.ToString(), item.GetColor()));
        }
        ES3.Save<Dictionary<string, string>>("SpineAvatarSaveTest", slotAttachmentDictionary);
        //ES3.Save<SkeletonData>("TestSkeletonData", skeleton.Data);
        ES3.Save<Dictionary<string, AttachmentColorPair>>("SpineAvatarSaveTest2", slotAttachmentDictionary2);
            
    }

    public void AdjustColorsOnAvatar() //called on outfit changed
    {
        if (selectedSuit.CheckIfSuitContainsLegs())
        {
            skeleton.FindSlot(defaultLeftDownLegPart).SetColor(Color.white);
            skeleton.FindSlot(defaultRightDownLegPart).SetColor(Color.white);
        }else
        {
            skeleton.FindSlot(defaultLeftDownLegPart).SetColor(currentSkinColor);
            skeleton.FindSlot(defaultRightDownLegPart).SetColor(currentSkinColor);
        }

        if (selectedSuit.CheckIfSuitContainsSleeves())
        {
                skeleton.FindSlot(defaultRightDownArmPart).SetColor(Color.white);
                skeleton.FindSlot(defaultLeftDownArmPart).SetColor(Color.white);
        }
        else
        {
            skeleton.FindSlot(defaultRightDownArmPart).SetColor(currentSkinColor);
            skeleton.FindSlot(defaultLeftDownArmPart).SetColor(currentSkinColor);
        }
    }

    public void ChangeAvatarSkinColor(Color newColor)
    {
        currentSkinColor = newColor;
        foreach (var slotName in BodyPartsForColorChange)
            skeleton.FindSlot(slotName).SetColor(Color.white);

        foreach(var slotName in BodyPartsForColorChange)
        {
            string pom = skeleton.FindSlot(slotName).Attachment.Name;
            if (DefaultBodyPartNames.Contains(slotName))
            {
                if (!DefaultBodyPartNames.Contains(skeleton.FindSlot(slotName).Attachment.Name))
                    continue;
                else
                    skeleton.FindSlot(slotName).SetColor(newColor);

            }
            else
                skeleton.FindSlot(slotName).SetColor(newColor);
        }

    }

    public void ChangeAvatarHairstyleColor(Color newColor)
    {
        skeleton.FindSlot(HairSlotName).SetColor(newColor);
    }

    public void ChangeAvatarHairstyle(string hairstyleName)
    {
       
    
        var skeleton = GetComponent<SkeletonRenderer>().skeleton;
        
        skeleton.SetAttachment(HairSlotName, hairstyleName);
    }

    void ResetAvatarHairStyle()
    {
        skeleton.SetAttachment(HairSlotName, null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
