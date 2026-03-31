using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimalAsset", menuName = "Scriptable Objects/AnimalAsset")]
public class AnimalAsset : ScriptableObject
{
    public string _name;
    public Sprite _icon;
    public bool _collected;

    //public AnimalAsset(string name, Sprite icon, bool collected)
    //{
    //    _name = name;
    //    _icon = icon;
    //    _collected = collected;
    //}
}