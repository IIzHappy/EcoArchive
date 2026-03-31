using System;

[Serializable]
public class LoadablePhoto
{
    public string _photoName;
    public string _filePath;
    public float _score;

    public LoadablePhoto(string name, string filePath, float score)
    {
        _photoName = name;
        _filePath = filePath;
        _score = score;
    }
}