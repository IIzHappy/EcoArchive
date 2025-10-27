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
        UpdateCollection();
    }

    public void FoundAnimal(Animal animal)
    {
        _animals[animal] = true;
        UpdateAnimals();
        Debug.Log(animal._name + " collected");
    }

    public void AddBug(Bug bug)
    {
        _bugs[bug]++;
        UpdateBugs();
        Debug.Log(bug._name + " collected");
    }

    public void AddBone(Bone bone)
    {
        _bones[bone]++;
        UpdateBones();
        Debug.Log(bone._name + " collected");
    }

    public void UpdateCollection()
    {
        UpdateAnimals();
        UpdateBugs();
        UpdateBones();
    }

    public void UpdateAnimals()
    {

    }
    public void UpdateBugs()
    {

    }
    public void UpdateBones()
    {

    }

    public void ResetCollection(Item item)
    {
        foreach (Animal animal in _animals.Keys)
        {
            _animals[animal] = false;
        }
        foreach (Bug bug in _bugs.Keys)
        {
            _bugs[bug] = 0;
        }
        foreach (Bone bone in _bones.Keys)
        {
            _bones[bone] = 0;
        }
        UpdateCollection();
    }
}
