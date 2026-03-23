using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SkyboxController : MonoBehaviour
{
    public int _skyColourIndex = -1;
    public List<SkyboxColours> _skyColours = new List<SkyboxColours>();
    public Material _skyboxMaterial;

    SkyboxColours _skyboxColour;

    void Update()
    {
        if (_skyColours.Count < 1 || _skyboxMaterial == null) return;
        if (_skyColours.Count <= _skyColourIndex) _skyColourIndex = 0;

        _skyboxMaterial.SetColor("_SkyTopColour", _skyColours[_skyColourIndex]._skyTop);
        _skyboxMaterial.SetColor("_SkyColour", _skyColours[_skyColourIndex]._sky);
        _skyboxMaterial.SetColor("_HorizonColour", _skyColours[_skyColourIndex]._horizon);
        _skyboxMaterial.SetColor("_SunriseColour", _skyColours[_skyColourIndex]._sunrise);
        _skyboxMaterial.SetFloat("_HorizonSunriseRadius", _skyColours[_skyColourIndex]._sunriseRadius);
        _skyboxMaterial.SetFloat("_SkyBlendHeight", _skyColours[_skyColourIndex]._skyBlendHeight);
    }

    public void UpdateTime()
    {


    }
}
