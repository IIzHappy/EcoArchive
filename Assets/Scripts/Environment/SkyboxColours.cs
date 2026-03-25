using UnityEngine;

[CreateAssetMenu(fileName = "SkyboxColours", menuName = "Scriptable Objects/SkyboxColours")]
public class SkyboxColours : ScriptableObject
{
    public string _startTime = "00:00";
    public Color _skyTop = new Color(81, 165, 212, 255);
    public Color _sky = new Color(116, 182, 207, 255);
    public Color _horizon = new Color(214, 242, 245, 255);
    public Color _sunrise = new Color(219, 37, 26, 255);
    public float _sunriseRadius = 0.5f;
    public float _skyBlendHeight = -0.5f;

    [HideInInspector] public float StartTimeSeconds;
}
