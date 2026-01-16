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
                _curWin.gameObject.SetActive(false);
            }
            else
            {
                //open options menu
            }
        }
    }

    public void OpenWindow(UIWindow window)
    {

    }
}
