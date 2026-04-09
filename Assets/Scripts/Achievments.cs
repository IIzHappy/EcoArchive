using UnityEngine;

public class Achievments : MonoBehaviour
{
    public static Achievments Instance {get; private set;}

    bool[] achievments = new bool[1];

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
                }
            }
        }
    }

    //public 

    public void LoadProgress(bool[] achievmentsUnlocked, int[] specProg)
    {
        achievments = achievmentsUnlocked;
        specialistCounter = specProg;
    }
}
