using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Collection : MonoBehaviour
{
    public static Collection Instance { get; private set; }

    [SerializeField] Dictionary<Animal, bool> _animals = new Dictionary<Animal, bool>();
    [SerializeField] Dictionary<Bug, int> _bugs = new Dictionary<Bug, int>();
    [SerializeField] Dictionary<Bone, int> _bones = new Dictionary<Bone, int>();

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

    public void FoundAnimal(Animal animal)
    {
        _animals[animal] = true;
    }

    public void AddBug(Bug bug)
    {
        _bugs[bug]++;
        Debug.Log(bug._name + " collected");
    }

    public void AddBone(Bone bone)
    {
        _bones[bone]++;
        Debug.Log(bone._name + " collected");
    }

    public void ResetInventory(Item item)
    {

    }
}
