using System;
using UnityEngine;

public class UITab : MonoBehaviour
{
    public UITabGroup _tabGroup;

    //window open
    void OnEnable()
    {
        _tabGroup.OpenTab(this);
    }
}
