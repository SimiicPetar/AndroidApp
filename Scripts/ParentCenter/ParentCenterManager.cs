using Patterns.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentCenterManager : Singleton<ParentCenterManager>
{
    // Start is called before the first frame update
    public List<ParentCenterTab> Tabs;

    ParentCenterTab activeTab;

    private void Start()
    {
        activeTab = Tabs[0];
       
    }

    private void OnEnable()
    {
        ParentCenterTab.OnTabClicked += OpenNewTab;
        activeTab = Tabs[0];
    }
    private void OnDisable()
    {
        ParentCenterTab.OnTabClicked -= OpenNewTab;
        CloseActiveTab();
    }


    private void OpenNewTab(ParentCenterTab tab)
    {
        activeTab.TabClosed();
        activeTab = tab;
    }

    

    public void CloseActiveTab()
    {
        activeTab.TabClosed();
    }

    public void OpenFirstTab()
    {
        Tabs[0].InitialTabOpen();
    }
}
