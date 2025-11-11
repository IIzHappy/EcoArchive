using UnityEngine;

[CreateAssetMenu(fileName = "Bug", menuName = "Scriptable Objects/Bug")]
public class Bug : ScriptableObject
{
    public string _name;
    public Sprite _icon;
    public int _numCollected;
}
