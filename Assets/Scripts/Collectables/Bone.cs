using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Bone", menuName = "Scriptable Objects/Bone")]
public class Bone : ScriptableObject
{
    public string _name;
    public Sprite _icon;
    public int _numCollected;
}
