using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    public UIWindow _curWin;

    [SerializeField] UIWindow _escMenu;
    [SerializeField] UIWindow _collectionMenu;

    [SerializeField] GameObject _dayNight;
    [SerializeField] GameObject _eventFeed;

    [SerializeField] bool _inGame = true;
    public bool _freezeOnPause = true;

    private void Start()
    {
        Instance = this;
        PlayerInputs.Instance.UpdateUIManager(this);
    }

    public void OpenWindows()
    {
        if (_freezeOnPause)
        {
            PauseGame();
        }
        if (_inGame)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
    public void CloseWindows()
    {
        if (_freezeOnPause && _curWin == null)
        {
            UnpauseGame();
        }
        if (_inGame)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        PlayerInputs.Instance._playerActive = false;
        _dayNight.SetActive(false);
        _eventFeed.SetActive(false);
    }
    public void UnpauseGame()
    {
        if (_curWin == null)
        {
            Time.timeScale = 1;
            PlayerInputs.Instance._playerActive = true;
            _dayNight.SetActive(true);
            _eventFeed.SetActive(true);
        }
    }

    public void EscapePressed()
    {
        if (_curWin != null)
        {
            _curWin.CloseWindow();
        }
        else
        {
            _escMenu.gameObject.SetActive(true);
            OpenWindows();
        }
    }
    public void CollectionPressed()
    {
        if (_inGame)
        {
            if (_collectionMenu == null)
            {
                Debug.LogWarning("Set collection menu");
                return;
            }

            if (_curWin == null) {
                _collectionMenu.gameObject.SetActive(true);
            }
            else if (_curWin == _collectionMenu)
            {
                _collectionMenu.CloseWindow();
            }
        }
    }
}
