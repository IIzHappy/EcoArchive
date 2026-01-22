using UnityEngine;

public class UIWindow : MonoBehaviour
{
    public UIWindow _prevWin;
    public string _windowName;

    //window open
    public virtual void OnEnable()
    {
        if (_prevWin == null)
        {
            _prevWin = UIManager.Instance._curWin;
        }
        UIManager.Instance._curWin = this;
        Debug.Log(_windowName);
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
            UIManager.Instance._curWin = null;
            UIManager.Instance.UnpauseGame();
        }
        _prevWin = null;
        gameObject.SetActive(false);
    }
}
