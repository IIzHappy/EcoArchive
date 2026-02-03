using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] string _mainMenu;

    //maybe adding smth that tells other scripts the scene changed

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene(_mainMenu);
    }

    void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
