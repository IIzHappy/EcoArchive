using System.Collections.Generic;
using UnityEngine;

public class UITabGroup : UIWindow
{
    public UITab _curTab;

    public void OpenTab(UITab openTab)
    {
        if (_curTab != null && _curTab != openTab)
        {
            _curTab.gameObject.SetActive(false);
        }
        _curTab = openTab;
    }
}
