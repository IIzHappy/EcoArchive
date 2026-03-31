using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[System.Serializable]
public class Save
{
    public bool[] _animals;
    //public List<Bug> _bugs = new List<Bug>();
    //public List<Bone> _bones = new List<Bone>();
    public List<LoadablePhoto> _loadablePhotos = new List<LoadablePhoto>();
}
