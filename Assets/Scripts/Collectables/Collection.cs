using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Collection : MonoBehaviour
{
    public static Collection Instance { get; private set; }

    public List<Photo> _photos;
    [SerializeField] Dictionary<AnimalAsset, GameObject> _animals = new Dictionary<AnimalAsset, GameObject>();
    [SerializeField] Dictionary<Bug, GameObject> _bugs = new Dictionary<Bug, GameObject>();
    [SerializeField] Dictionary<Bone, GameObject> _bones = new Dictionary<Bone, GameObject>();

    [SerializeField] GameObject _photoIcons;
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
    }
    private void Start()
    {
        LoadCollection();
    }

    public void LoadCollection()
    {
        AnimalAsset[] allAnimals = Resources.LoadAll<AnimalAsset>("Animals");
        foreach (var animal in allAnimals)
        {
            _animals.Add(animal, null);
        }

        Bug[] allBugs = Resources.LoadAll<Bug>("Bugs");
        foreach (var bug in allBugs)
        {
            _bugs.Add(bug, null);
        }

        Bone[] allBones = Resources.LoadAll<Bone>("Bones");
        foreach (var bone in allBones)
        {
            _bones.Add(bone, null);
        }

        InstantiateAnimals();
        InstantiateBugs();
        InstantiateBones();
    }

    public void ResetCollection()
    {
        ResetAnimals();
        ResetBugs();
        ResetBones();
    }

    public void ResetAnimals()
    {
        foreach (AnimalAsset animal in _animals.Keys)
        {
            animal._collected = false;
        }
        InstantiateAnimals();
    }
    public void ResetBugs()
    {
        foreach (Bug bug in _bugs.Keys)
        {
            bug._numCollected = 0;
        }
        InstantiateBugs();
    }
    public void ResetBones()
    {
        foreach (Bone bone in _bones.Keys)
        {
            bone._numCollected = 0;
        }
        InstantiateBones();
    }

    void InstantiateAnimals()
    {
        foreach (AnimalAsset animal in _animals.Keys.ToList())
        {
            if (_animals[animal] == null) _animals[animal] = Instantiate(_iconPrefab, _animalIcons.transform);
            if (animal._collected)
            {
                _animals[animal].GetComponent<Image>().sprite = animal._icon;
                _animals[animal].GetComponentInChildren<TMP_Text>().text = animal._name;
            }
        }
    }
    void InstantiateBugs()
    {
        foreach (Bug bug in _bugs.Keys.ToList())
        {
            if (_bugs[bug] == null) _bugs[bug] = Instantiate(_iconPrefab, _bugIcons.transform);
            if (bug._numCollected >= 1)
            {
                _bugs[bug].GetComponent<Image>().sprite = bug._icon;
                _bugs[bug].GetComponentInChildren<TMP_Text>().text = bug._name + "-" + bug._numCollected;
            }
        }
    }
    void InstantiateBones()
    {
        foreach (Bone bone in _bones.Keys.ToList())
        {
            if (_bones[bone] == null) _bones[bone] = Instantiate(_iconPrefab, _boneIcons.transform);
            if (bone._numCollected >= 1)
            {
                _bones[bone].GetComponent<Image>().sprite = bone._icon;
                _bones[bone].GetComponentInChildren<TMP_Text>().text = bone._name + " - " + bone._numCollected;
            }
        }
    }

    public void FoundAnimal(AnimalAsset animal)
    {
        if (animal._collected) return;
        animal._collected = true;
        _animals[animal].GetComponent<Image>().sprite = animal._icon;
        _animals[animal].GetComponentInChildren<TMP_Text>().text = animal._name;
        Debug.Log(animal._name + " collected");
    }

    public void AddBug(Bug bug)
    {
        bug._numCollected++;
        _bugs[bug].GetComponent<Image>().sprite = bug._icon;
        _bugs[bug].GetComponentInChildren<TMP_Text>().text = bug._name + "-" + bug._numCollected;
        Debug.Log(bug._name + " collected");
    }

    public void AddBone(Bone bone)
    {
        bone._numCollected++;
        _bones[bone].GetComponent<Image>().sprite = bone._icon;
        _bones[bone].GetComponentInChildren<TMP_Text>().text = bone._name + " - " + bone._numCollected;
        Debug.Log(bone._name + " collected");
    }

    public void AddPhoto(Photo newPhoto)
    {
        _photos.Add(newPhoto);
        GameObject _newPhoto = Instantiate(_iconPrefab, _photoIcons.transform);
        _newPhoto.GetComponent<Image>().sprite = newPhoto._photo;
        _newPhoto.transform.GetComponentInChildren<TMP_Text>().text = newPhoto.name;
    }
}
