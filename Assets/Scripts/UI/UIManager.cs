using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    public UIWindow _curWin;

    [SerializeField] UIWindow _pauseMenu;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_curWin != null)
            {
                _curWin.CloseWindow();
            }
            else
            {
                _pauseMenu.gameObject.SetActive(true);
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public void UnpauseGame()
    {
        if (_curWin == null)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    /*
    public void OpenWindow(UIWindow window)
    {
        if (window._prevWin == null)
        {
            window._prevWin = UIManager.Instance._curWin;
        }
        UIManager.Instance._curWin = window;
    }
    */
}
