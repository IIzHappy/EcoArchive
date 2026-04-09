using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Achievments : MonoBehaviour
{
    public static Achievments Instance {get; private set;}

    [SerializeField] GameObject book;
    [SerializeField] GameObject prefab;
    [SerializeField] Sprite[] sprites = {null, null};
    GameObject[] entries; 

    bool[] achievments = new bool[2];

    //Achievment: Specialist
    bool Specialist = false;
    string[] specialistReference = {"Bluejay", "Cardinal", "Catfish", "Salmon", "River Trout", "Owl", "Duck", "Goose", "Doe", "Stag", "Fox", "Coyote", "Wolf", "Bear", "Squirrel", "Rat", "Turtle", "Snake"};
    int[] specialistCounter = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    public void IterateSpecialist(string animal)
    {
        Debug.Log("Start");
        for (int i = 0; i < specialistReference.Length; i++)
        {
            Debug.Log("String ref");
            if (specialistReference[i] == animal)
            {
                Debug.Log("int ref");
                specialistCounter[i]++;
                Debug.Log("Complete ref");
                if (specialistCounter[i] >= 10)
                {
                    Specialist = true;
                    achievments[0] = true;
                    Debug.Log("Entries and sprite ref");
                    entries[0].GetComponent<Image>().sprite = sprites[0];
                    entries[0].GetComponentInChildren<TMP_Text>().text = "Specialist";
                }
            }
        }
    }

    //Achievment: Full Animal Collection
    bool AnimalsComplete = false;
    public void CheckAnimalCompletion(bool[] animals)
    {
        for (int i = 0; i < animals.Length;)
        {
            if (!animals[i])
            {
                return;
            }
            else i++;
            if (i == animals.Length)
            {
                AnimalsComplete = true;
                achievments[1] = true;
                entries[1].GetComponent<Image>().sprite = sprites[1];
                entries[1].GetComponentInChildren<TMP_Text>().text = "Animal Collection Complete";
            }
        }
    }



    public void SaveAchievments()
    {
        AchievmentFile file = CreateAchievmentFile();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = File.Create(Application.persistentDataPath + "/achievmentSave.save");
        bf.Serialize(fileStream, file);
    }

    public AchievmentFile CreateAchievmentFile()
    {
        AchievmentFile save = new AchievmentFile();
        save.achievments = achievments;
        save.specialistProgress = specialistCounter;
        return save;
    }

    public void LoadProgress(bool[] achievmentsUnlocked, int[] specProg)
    {
        achievments = achievmentsUnlocked;
        specialistCounter = specProg;
        Specialist = achievments[0];
        if (Specialist)
        {
            entries[0].GetComponent<Image>().sprite = sprites[0];
            entries[0].GetComponentInChildren<TMP_Text>().text = "Specialist";
        }
        AnimalsComplete = achievments[1];
        if (AnimalsComplete)
        {
            entries[1].GetComponent<Image>().sprite = sprites[1];
            entries[1].GetComponentInChildren<TMP_Text>().text = "Animal Collection Complete";
        }
    }

    private void Start()
    {
        entries = new GameObject[achievments.Length];
        for (int i = 0; i < achievments.Length; i++)
        {
            entries[i] = Instantiate(prefab, book.transform);
        }
        if (File.Exists(Application.persistentDataPath + "/achievmentSave.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/achievmentSave.save", FileMode.Open);
            AchievmentFile save = (AchievmentFile)bf.Deserialize(file);
            file.Close();
            LoadProgress(save.achievments, save.specialistProgress);
        }
    }
}
