using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject warning;
    [SerializeField] Button Load;
    [SerializeField] SceneChanger sceneChanger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            Load.interactable = false;
        }
    }

    public void NewGame()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            warning.SetActive(true);
        }
        else
        {
            sceneChanger.LoadScene("Prototype scene");
        }
    }

    public void Override()
    {
        File.Delete(Application.persistentDataPath + "/gamesave.save");
        File.Delete(Application.persistentDataPath + "/achievmentSave.save");
        System.IO.DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath + "/Player Images");
        foreach (FileInfo fi in dir.EnumerateFiles())
        {
            fi.Delete();
        }
        sceneChanger.LoadScene("Prototype scene");
    }
}
