using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Collection : MonoBehaviour
{
    public static Collection Instance { get; private set; }

    [SerializeField] Dictionary<AnimalAsset, bool> _animals = new Dictionary<AnimalAsset, bool>();
    [SerializeField] Dictionary<Bug, int> _bugs = new Dictionary<Bug, int>();
    [SerializeField] Dictionary<Bone, int> _bones = new Dictionary<Bone, int>();

    [SerializeField] GameObject _animalIcons;
    [SerializeField] GameObject _bugIcons;
    [SerializeField] GameObject _boneIcons;

    public GameObject _iconPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadCollection();
        UpdateCollection();
    }

    public void LoadCollection()
    {
        AnimalAsset[] allAnimals = Resources.LoadAll<AnimalAsset>("Animals");
        foreach (var animal in allAnimals)
        {
            //add playerpref for saving?
            _animals[animal] = false;
        }
        Bug[] allBugs = Resources.LoadAll<Bug>("Bugs");
        foreach (var bug in allBugs)
        {
            _bugs[bug] = 0;
        }
        Bone[] allBones = Resources.LoadAll<Bone>("Bones");
        foreach (var bone in allBones)
        {
            _bones[bone] = 0;
        }
    }

    public void FoundAnimal(AnimalAsset animal)
    {
        if (_animals[animal] == true) return;
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
        foreach (Transform child in _animalIcons.transform)
        {
            Destroy(child.gameObject);
        }
        foreach(AnimalAsset animal in _animals.Keys)
        {
            GameObject icon = Instantiate(_iconPrefab, _animalIcons.transform);
            icon.GetComponent<Image>().sprite = animal._icon;
            icon.GetComponentInChildren<TMP_Text>().text = animal._name;
        }
    }
    public void UpdateBugs()
    {
        foreach (Transform child in _bugIcons.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Bug bug in _bugs.Keys)
        {
            GameObject icon = Instantiate(_iconPrefab, _bugIcons.transform); //should have question marks
            if (_bugs[bug] >= 1)
            {
                icon.GetComponent<Image>().sprite = bug._icon;
                icon.GetComponentInChildren<TMP_Text>().text = bug._name + "-" + _bugs[bug];
            }
        }
    }
    public void UpdateBones()
    {

        foreach (Transform child in _boneIcons.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Bone bone in _bones.Keys)
        {
            GameObject icon = Instantiate(_iconPrefab, _boneIcons.transform); //should have question marks
            if (_bones[bone] >= 1)
            {
                icon.GetComponent<Image>().sprite = bone._icon;
                icon.GetComponentInChildren<TMP_Text>().text = bone._name + " - " + _bones[bone];
            }
        }
    }

    public void ResetCollection(Item item)
    {
        foreach (AnimalAsset animal in _animals.Keys)
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
