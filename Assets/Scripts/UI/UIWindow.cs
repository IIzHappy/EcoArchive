using UnityEngine;

public class UIWindow : MonoBehaviour
{
    UIWindow _prevWin;
    string _windowName;

    public void Awake()
    {
        _prevWin = UIManager.Instance._curWin;
        UIManager.Instance._curWin = this;
    }

    //attach to esc and x button
    public void CloseWindow()
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
