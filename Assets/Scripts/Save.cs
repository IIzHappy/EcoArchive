using System.Collections.Generic;

[System.Serializable]
public class Save
{
    public bool[] _animals;
    public int[] _bugs;
    public int[] _bones;
    public List<LoadablePhoto> _loadablePhotos = new List<LoadablePhoto>();
    public float[] _settings;
}
