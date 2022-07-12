using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParentCenterTab : MonoBehaviour
{
   
    public delegate void TabClicked(ParentCenterTab tab);
    public static TabClicked OnTabClicked;
    public GameObject ViewToOpen;
    public Sprite ActiveSprite;
    public Sprite DisabledSprite;

    Button TabButton;
    Image TabImage;

    private void Awake()
    {
        TabImage = GetComponent<Image>();
    }
    private void Start()
    {
        TabButton = GetComponent<Button>();

       // TabButton.onClick.AddListener(() => TabOpened());
    }

    private void OnEnable()
    {
        OnTabClicked += OpenTab;
    }

    private void OnDisable()
    {
        OnTabClicked -= OpenTab;
    }

    public void TabOpened()
    {
        OnTabClicked.Invoke(this);
        ViewToOpen.SetActive(true);
        TabImage.sprite = ActiveSprite;
    }

    public void InitialTabOpen()
    {
        ViewToOpen.SetActive(true);
       // TabImage.sprite = ActiveSprite;
    }

    void OpenTab(ParentCenterTab tabToClose)
    {
        if(tabToClose != this)
        {
            TabClosed();
        }
    }

    public void TabClosed()
    {
       /* if(ViewToOpen != null)
            ViewToOpen.SetActive(false);
        TabImage.sprite = DisabledSprite;*/
    }
}
