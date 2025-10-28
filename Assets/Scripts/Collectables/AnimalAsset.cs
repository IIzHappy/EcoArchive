using UnityEngine;

[CreateAssetMenu(fileName = "AnimalAsset", menuName = "Scriptable Objects/AnimalAsset")]
public class AnimalAsset : ScriptableObject
{
    public string _name;
    public Sprite _icon;
}