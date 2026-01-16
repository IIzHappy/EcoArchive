using UnityEngine;

public class UIWindow : MonoBehaviour
{
    public UIWindow _prevWin;
    protected string _windowName;

    //window open
    public virtual void Awake()
    {
        _prevWin = UIManager.Instance._curWin;
        UIManager.Instance._curWin = this;
    }

    //attach to esc and x button
    public virtual void CloseWindow()
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
