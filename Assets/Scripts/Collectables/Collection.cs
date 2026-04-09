using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Collection : MonoBehaviour
{
    public static Collection Instance { get; private set; }

    public List<Photo> _photos;
    List<LoadablePhoto> _loadablePhotos;
    Dictionary<AnimalAsset, GameObject> _animals = new Dictionary<AnimalAsset, GameObject>();
    private bool[] _animalsFound;
    [SerializeField] Dictionary<Bug, GameObject> _bugs = new Dictionary<Bug, GameObject>();
    private int[] _bugsFound;
    [SerializeField] Dictionary<Bone, GameObject> _bones = new Dictionary<Bone, GameObject>();
    private int[] _bonesFound;

    [SerializeField] OptionsManager settings;
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
        //AnimalAsset[] allAnimals = Resources.LoadAll<AnimalAsset>("Animals");
        //foreach (var animal in allAnimals)
        //{
        //    _animals.Add(animal, null);
        //}

        //Bug[] allBugs = Resources.LoadAll<Bug>("Bugs");
        //foreach (var bug in allBugs)
        //{
        //    _bugs.Add(bug, null);
        //}

        //Bone[] allBones = Resources.LoadAll<Bone>("Bones");
        //foreach (var bone in allBones)
        //{
        //    _bones.Add(bone, null);
        //}

        //_loadablePhotos = new List<LoadablePhoto>();

        _animalsFound = new bool[_animals.Count()];
        _bugsFound = new int[_bugs.Count()];
        _bonesFound = new int[_bones.Count()];
        LoadCollection();
    }

    public void LoadCollection()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            if (save._animals != null)
            {
                for (int k = 0; k < save._animals.Length; k++)
                {
                    _animalsFound[k] = save._animals[k];
                }
            }

            int i = 0;
            foreach (AnimalAsset animal in _animals.Keys)
            {
                animal._collected = _animalsFound[i];
                i++;
            }

            i = 0;
            if (save._bugs != null)
            {
                foreach (Bug bug in _bugs.Keys)
                {
                    if (_bugsFound[i] > 0)
                    {
                        bug._numCollected = _bugsFound[i];
                    }
                    i++;
                }
            }

            i = 0;
            if (save._bones != null)
            {
                foreach (Bone bone in _bones.Keys)
                {
                    if (_bonesFound[i] > 0)
                    {
                        bone._numCollected = _bonesFound[i];
                    }
                    i++;
                }
            }

            if (save._loadablePhotos != null)
            {
                _loadablePhotos = save._loadablePhotos;
                foreach (LoadablePhoto loadablePhoto in save._loadablePhotos)
                {
                    Photo photo = ScriptableObject.CreateInstance<Photo>();
                    photo.name = loadablePhoto._photoName;
                    photo._score = loadablePhoto._score;
                    Texture2D image = LoadTexture(loadablePhoto._filePath);
                    photo._photo = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f), 100f);
                    AddPhoto(photo);
                }
            }
            else
            {
                _loadablePhotos = new List<LoadablePhoto>();
            }

            if (save._settings != null)
            {
                settings.SetSliders(save._settings);
            }

            InstantiateAnimals();
            InstantiateBugs();
            InstantiateBones();

            Debug.Log("Game save loaded.");
        }
        else
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

            _loadablePhotos = new List<LoadablePhoto>();

            InstantiateAnimals();
            InstantiateBugs();
            InstantiateBones();

            Debug.Log("New game loaded.");
        }
    }

    public Texture2D LoadTexture(string FilePath)
    {
        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);
            if (Tex2D.LoadImage(FileData))  return Tex2D;
        }
        return null;
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

    public void FoundAnimal(string name)
    {
        int i = 0;
        foreach (AnimalAsset key in _animals.Keys)
        {
            if (key._name == name)
            {
                _animalsFound[i] = true;
                key._collected = true;
                Debug.Log(key._name + key._collected);
                InstantiateAnimals();
                Achievments.Instance.CheckAnimalCompletion(_animalsFound);
                return;
            }
            i++;
        }
        //animal._collected = true;
        //_animals[animal].GetComponent<Image>().sprite = animal._icon;
        //_animals[animal].GetComponentInChildren<TMP_Text>().text = animal._name;
        //EventFeed.Instance.makeNotif(animal._icon, animal._name + " discovered");
    }

    public void AddBug(Bug bug)
    {
        int i = 0;
        foreach (Bug key in _bugs.Keys)
        {
            if (key == bug)
            {
                _bugsFound[i] += 1;
                return;
            }
            i++;
        }
        bug._numCollected++;
        _bugs[bug].GetComponent<Image>().sprite = bug._icon;
        _bugs[bug].GetComponentInChildren<TMP_Text>().text = bug._name + "-" + bug._numCollected;
        EventFeed.Instance.makeNotif(bug._icon, bug._name + " collected");
        InstantiateBugs();
    }

    public void AddBone(Bone bone)
    {
        int i = 0;
        foreach (Bone key in _bones.Keys)
        {
            if (key == bone)
            {
                _bonesFound[i] += 1;
            }
            i++;
        }
        bone._numCollected++;
        _bones[bone].GetComponent<Image>().sprite = bone._icon;
        _bones[bone].GetComponentInChildren<TMP_Text>().text = bone._name + " - " + bone._numCollected;
        EventFeed.Instance.makeNotif(bone._icon, bone._name + " collected");
        InstantiateBones();
    }

    public void AddPhoto(Photo newPhoto)
    {
        _photos.Add(newPhoto);
        GameObject _newPhoto = Instantiate(_iconPrefab, _photoIcons.transform);
        _newPhoto.GetComponent<Image>().sprite = newPhoto._photo;
        _newPhoto.transform.GetComponentInChildren<TMP_Text>().text = newPhoto.name;
    }

    public void AddLoadablePhoto(LoadablePhoto newLoadablePhoto)
    {
        _loadablePhotos.Add(newLoadablePhoto);
    }

    public void RenamePhoto(string newName, int index)
    {
        _loadablePhotos[index]._photoName = newName;
        _photos[index]._photoName = newName;
        _photos[index].name = newName;
    }

    public bool[] GetAnimals()
    {
        return _animalsFound;
    }
    public int[] GetBugs()
    {
        return _bugsFound;
    }
    public int[] GetBones()
    {
        return _bonesFound;
    }

    public List<LoadablePhoto> GetPhotos()
    {
        return _loadablePhotos;
    }
}
