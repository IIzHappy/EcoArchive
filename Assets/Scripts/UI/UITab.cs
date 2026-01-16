using UnityEngine;

public class UITab : UIWindow
{
    public string _tabGroup;

    //window open
    public override void Awake()
    {
        UITab oldTab = UIManager.Instance._curWin.gameObject.GetComponent<UITab>();
        if (oldTab != null)
        {
            if (oldTab._tabGroup == _tabGroup)
            {
                _prevWin = oldTab._prevWin;
                oldTab.CloseTab();
            }
        } 
        else
        {
            _prevWin = UIManager.Instance._curWin;
        }
        UIManager.Instance._curWin = this;
    }

    public void CloseTab()
    {

        gameObject.SetActive(false);
    }

    //attach to esc and x button
    public override void CloseWindow()
    {
        if (_prevWin != null)
        {
            _prevWin.gameObject.SetActive(true);
        }
        else
        {
            //unpause game
        }
        gameObject.SetActive(false);
    }
}
