using UnityEngine;

[CreateAssetMenu(fileName = "Photo", menuName = "Scriptable Objects/Photo")]
public class Photo : ScriptableObject
{
    public Sprite _photo;
    public string _photoName;
    public float _score;
}
