using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Achievments : MonoBehaviour
{
    public static Achievments Instance {get; private set;}

    bool[] achievments = new bool[2];

    //Achievment: Specialist
    bool Specialist = false;
    string[] specialistReference = {"Bluejay", "Cardinal", "Catfish", "Salmon", "River Trout", "Owl", "Duck", "Goose", "Doe", "Stag", "Fox", "Coyote", "Wolf", "Bear", "Squirrel", "Rat", "Turtle", "Snake"};
    int[] specialistCounter = new int[18];
    public void IterateSpecialist(string animal)
    {
        for (int i = 0; i < specialistReference.Length; i++)
        {
            if (specialistReference[i] == animal)
            {
                specialistCounter[i]++;
                if (specialistCounter[i] >= 10)
                {
                    Specialist = true;
                    achievments[0] = true;
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
        AnimalsComplete = achievments[1];
    }

    private void Start()
    {
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
