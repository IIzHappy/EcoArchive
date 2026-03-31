using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameSave : MonoBehaviour
{
    Collection _collection;

    public void SaveGame()
    {
        Save save = CreateSaveGameObject();

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        binaryFormatter.Serialize(file, save);
        file.Close();
    }

    private Save CreateSaveGameObject()
    {
        Save save = new Save();
        save._animals = _collection.GetAnimals();
        //save._bugs = _collection.GetBugs();
        //save._bones = _collection.GetBones();
        save._loadablePhotos = Collection.Instance.GetPhotos();

        return save;
    }
}
