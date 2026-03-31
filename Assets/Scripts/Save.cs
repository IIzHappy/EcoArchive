using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[System.Serializable]
public class Save
{
    public bool[] _animals;
    public int[] _bugs;
    public int[] _bones;
    public List<LoadablePhoto> _loadablePhotos = new List<LoadablePhoto>();
}
